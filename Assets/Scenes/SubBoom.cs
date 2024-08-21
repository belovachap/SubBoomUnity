using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

static class Utilities
{

    public static Texture2D blankSquareTexture = Resources.Load<Texture2D>("blank_square");
    public static Texture2D blankCircleTexture = Resources.Load<Texture2D>("blank_circle");

    public static GameObject newSpriteGameObject(string name, Texture2D texture, Vector3 localScale, Vector3 position, Color color)
    {
        GameObject go = new GameObject(name);
        go.transform.localScale = localScale;
        go.transform.position = position;

        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create
        (
            texture,
            new UnityEngine.Rect(0.0f,0.0f,texture.width,texture.height),
            new Vector2(0.5f, 0.5f),
            (float) texture.width
        );

        renderer.sprite = sprite;
        renderer.color = color;
        go.AddComponent<AudioSource>();

        return go;
    }

    public static GameObject newSpriteGameObjectWithPhysics(string name, Texture2D texture, Vector3 localScale, Vector3 position, Color color)
    {
        GameObject go = newSpriteGameObject(name, texture, localScale, position, color);

        Rigidbody2D body = go.AddComponent<Rigidbody2D>();
        body.gravityScale = 0f;

        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        return go;
    }

}

public class Submarine
{
    float velocity;
    public GameObject submarine;

    public Submarine()
    {
        float depth = UnityEngine.Random.Range(-4.5f, 2);

        velocity = UnityEngine.Random.Range(0.5f, 1.5f);

        if (UnityEngine.Random.Range(0.0f, 1.0f) >= 0.5f)
        {
            velocity *= -1;
        }

        Vector3 initialPosition;
        if (velocity < 0)
        {
            initialPosition = new Vector3(12, depth, 0);
        }
        else
        {
            initialPosition = new Vector3(-12, depth, 0);
        }

        submarine = Utilities.newSpriteGameObjectWithPhysics
        (
            "Submarine",
            Utilities.blankSquareTexture,
            new Vector3(2, 0.5f, 1),
            initialPosition,
            new Color(1.0f, 0.0f, 0.0f, 1.0f)
        );

        // Play sonar sound to introduce new submarine
        AudioSource audio = submarine.GetComponent<AudioSource>();
        AudioClip sonar = Resources.Load<AudioClip>("submarine_sonar");
        audio.PlayOneShot(sonar);
    }

    public void UpdateMovement(float dt, List<Bubble> bubbles)
    {
        Vector3 pos = submarine.transform.position;
        pos.x += dt * velocity;

        // Wrap the subs around if they've wandered off screen
        if (pos.x < -12)
        {
            pos.x = 12;
        }

        if (pos.x > 12)
        {
            pos.x = -12;
        }

        submarine.transform.position = pos;

        // Add a bubble
        BoxCollider2D collider = submarine.GetComponent<BoxCollider2D>();
        Vector3 bubblePos = pos;
        if (velocity > 0)
        {
            bubblePos.x -= (collider.bounds.size.x / 2.0f);
        }
        else
        {
            bubblePos.x += (collider.bounds.size.x / 2.0f);
        }
        bubbles.Add(new Bubble(bubblePos));
    }
}

public class Bubble
{
    public float timeExisted = 0f;
    public float timeToExist;
    public GameObject go;

    public Bubble(Vector2 position)
    {
        timeToExist = UnityEngine.Random.Range(1.0f, 2.0f);
        go = Utilities.newSpriteGameObject
        (
            "Bubble",
            Utilities.blankSquareTexture,
            new Vector3(0.1f, 0.1f, 1f),
            position,
            new Color(0.5f, 0.0f, 1.0f, 0.5f)
        );
    }

    public void UpdateMovement(float dt)
    {
        timeExisted += dt;
        Vector3 pos = go.transform.position;
        pos.x += UnityEngine.Random.Range(-1f, 1f) * dt;
        pos.y += UnityEngine.Random.Range(0.0f, 0.5f) * dt;
        go.transform.position = pos;
    }
}

public class Torpedo
{
    public float timeExisted = 0;
    public float timeToExist;
    public GameObject torpedo;

    public Torpedo(Vector2 submarinePosition)
    {
        torpedo = Utilities.newSpriteGameObject
        (
            "Torpedo",
            Utilities.blankSquareTexture,
            new Vector3(0.1f, 0.1f, 1),
            submarinePosition,
            new Color(1, 0, 0, 1)
        );
    }

    public void UpdateMovement(float duration, Vector2 destroyerPosition, List<Bubble> bubbles)
    {
        timeExisted += duration;
        Vector3 torpedoMovement = torpedo.transform.position;
        torpedoMovement.x = destroyerPosition.x * duration;
        torpedoMovement.y = destroyerPosition.y * duration;
        torpedo.transform.position = torpedoMovement;

        SpriteRenderer renderer = torpedo.GetComponent<SpriteRenderer>();
        Vector2 bubblePosition = torpedoMovement;
        bubblePosition.y += (renderer.bounds.size.y / 2.0f);
        bubbles.Add(new Bubble(bubblePosition));
    }
}

public class SubBoom : MonoBehaviour
{
    [SerializeField]
    Text scoreText;

    GameObject ocean;
    GameObject destroyer;

    List<Submarine> submarines;
    List<DepthCharge> depthCharges;
    List<ExplosionEffect> explosions;
    List<Torpedo> torpedos;
    List<Bubble> bubbles;

    GameObject currentDepthCharge;

    float timePlayed = 0.0f;
    float timeSinceSubAdded = 0.0f;
    ulong score = 0;

    float torpedoSpawnTime;
    float torpedoTimeUntilLaunch;

    // Start is called before the first frame update
    void Start()
    {
        ocean = Utilities.newSpriteGameObject
        (
            "Ocean",
            Utilities.blankSquareTexture,
            new Vector3(22, 8, 1),
            new Vector3(0, -1, 100),
            new Color(0.0f, 0.0f, 1.0f, 1.0f)
        );
    
        destroyer = Utilities.newSpriteGameObjectWithPhysics
        (
            "Destroyer",
            Utilities.blankSquareTexture,
            new Vector3(3, 0.5f, 1),
            new Vector3(0, 3.1f, 0),
            new Color(0.0f, 0.0f, 0.0f, 1.0f)
        );

        submarines = new List<Submarine>();
        submarines.Add(new Submarine());
        depthCharges = new List<DepthCharge>();
        explosions = new List<ExplosionEffect>();
        torpedos = new List<Torpedo>();
        bubbles = new List<Bubble>();
    }

    // Update is called once per frame
    void Update()
    {
        timePlayed += Time.deltaTime;

        Vector2 pos = destroyer.transform.position;

        // Handle user input
        if (Input.GetKeyDown("space"))
        {
            depthCharges.Add(new DepthCharge(pos));
        }

        if (Input.GetKey("space") && depthCharges.Count > 0)
        {
            //depthCharge moves downward until space key is released 
            //depthCharges[index].transform.Translate(Vector2.down * Time.deltaTime * 1.1f);
            //int chargeIndex = depthCharges.IndexOf(pos);
        }

        if (Input.GetKeyUp("space") && depthCharges.Count > 0)
        {
            //call ExplodeCharge() method
            //clear depthCharges List and currentDepthCharge
        }

        if (Input.GetKey("left"))
        {
            pos.x -= Time.deltaTime * 3;
            pos.x = Mathf.Max(pos.x, -9.25f);
            destroyer.transform.position = pos;
        }

        if (Input.GetKey("right"))
        {
            pos.x += Time.deltaTime * 3;
            pos.x = Mathf.Min(pos.x, 9.25f);
            destroyer.transform.position = pos;
        }

        if (Input.GetKey("escape"))
        {
            GameData gd = GameDataFileHandler.Load();
            DateTime now = DateTime.Now;
            gd.totalGamesPlayed += 1;
            gd.totalSecondsPlayed += (ulong)timePlayed;
            gd.lastScore = (ulong)score;
            gd.lastScoreDateTime = now.ToString();
            if (score >= gd.highScore)
            {
                gd.highScore = score;
                gd.highScoreDateTime = now.ToString();
            }
            GameDataFileHandler.Save(gd);

            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        }

        // Update submarine positions
        foreach (var sub in submarines)
        {
            sub.UpdateMovement(Time.deltaTime, bubbles);

            torpedoTimeUntilLaunch += Time.deltaTime;
            if (torpedoTimeUntilLaunch >= torpedoSpawnTime)
            {
                torpedos.Add(new Torpedo(sub.submarine.transform.position));
                foreach (var torp in torpedos)
                {
                    torp.UpdateMovement(Time.deltaTime, pos, bubbles);
                }

                torpedoTimeUntilLaunch = 0;
                torpedoSpawnTime = UnityEngine.Random.Range(1, 4);
            }
        }

        // Add a new submarine if it's been at least 10 seconds
        timeSinceSubAdded += Time.deltaTime;
        if (timeSinceSubAdded > 10)
        {
            timeSinceSubAdded = 0.0f;
            submarines.Add(new Submarine());
        }

        // Update depth charges, keep track of exploded charges
        List<DepthCharge> explodedCharges = new List<DepthCharge>();
        foreach (var dc in depthCharges)
        {
            dc.UpdateMovement(Time.deltaTime, bubbles);
            if (dc.secondsSinceDropped >= dc.timeUntilExplode)
            {
                explodedCharges.Add(dc);
            }
        }

        // Clear exploded depth charges
        // TODO: create an explosion object in its place
        foreach (var dc in explodedCharges)
        {
            explosions.Add(new ExplosionEffect(new Vector2
            (
                dc.depthCharge.transform.position.x,
                dc.depthCharge.transform.position.y))
            );

            Destroy(dc.depthCharge);
            depthCharges.Remove(dc);
        }

        // Create a foreach loop for the explosions list
        // That increases the scale of the explosion effect for a specified duration
        List<ExplosionEffect> deadExplosions = new List<ExplosionEffect>();
        foreach (var ec in explosions)
        {
            ec.UpdateMovement(Time.deltaTime);
            if (ec.secondsSinceDropped >= ec.timeUntilExplode)
            {
                deadExplosions.Add(ec);
            }
        }

        foreach (var ec in deadExplosions)
        {
            Destroy(ec.explodeCharge);
            explosions.Remove(ec);
        }

        //loop that checks if any items in explosions list is touching a submarine collider or destroyer collider
        //if so, destroy submarine and/or if destroyer, game over
        HashSet<Submarine> deadSubmarines = new HashSet<Submarine>();
        foreach (var ec in explosions)
        {
            foreach (var sub in submarines)
            {
                if (ec.explodeCharge.GetComponent<BoxCollider2D>().IsTouching
                   (sub.submarine.GetComponent<BoxCollider2D>()))
                {
                    deadSubmarines.Add(sub);
                }
            }
        }

        foreach (var sub in deadSubmarines)
        {
            Destroy(sub.submarine);
            submarines.Remove(sub);
            score += 1;
        }

        // Bubbles
        List<Bubble> expiredBubbles = new List<Bubble>();
        foreach (var bubble in bubbles) 
        {
            bubble.UpdateMovement(Time.deltaTime);
            if (bubble.timeExisted > bubble.timeToExist ||
                bubble.go.transform.position.y > 2.9) 
            {
                expiredBubbles.Add(bubble);
            }
        }

        foreach (var bubble in expiredBubbles)
        {
            Destroy(bubble.go);
            bubbles.Remove(bubble);
        }

        scoreText.text = "Score: " + score.ToString();
    }
}

public class DepthCharge
{
    public GameObject depthCharge;
    float velocity = -0.5f;
    public float secondsSinceDropped = 0.0f;
    public float timeUntilExplode = 10.0f;

    public DepthCharge(Vector2 destroyerPosition)
    {
        depthCharge = Utilities.newSpriteGameObject
        (
            "Explosion",
            Utilities.blankCircleTexture,
            new Vector3(0.5f, 0.5f, 1),
            destroyerPosition,
            new Color(0.0f, 0.0f, 0.0f, 1.0f)
        );

        // Play depth charge drop sound!
        AudioSource audio = depthCharge.GetComponent<AudioSource>();
        AudioClip sound = Resources.Load<AudioClip>("depth_charge_drop");
        audio.PlayOneShot(sound);
    }

    public void UpdateMovement(float dt, List<Bubble> bubbles)
    {
        secondsSinceDropped += dt;
        Vector2 pos = depthCharge.transform.position;
        pos.y += dt * velocity;
        depthCharge.transform.position = pos;

        // Add a bubble
        SpriteRenderer renderer = depthCharge.GetComponent<SpriteRenderer>();
        Vector2 bubblePos = pos;
        bubblePos.y += (renderer.bounds.size.y / 2.0f);
        bubbles.Add(new Bubble(bubblePos));
    }
}

public class ExplosionEffect
{
    public GameObject explodeCharge;

    public float secondsSinceDropped = 0.0f;
    public float timeUntilExplode = 3.0f;

    public ExplosionEffect(Vector2 explosionPosition)
    {
        explodeCharge = Utilities.newSpriteGameObjectWithPhysics
        (
            "Explosion",
            Utilities.blankSquareTexture,
            new Vector3(0.5f, 0.5f, 1),
            explosionPosition,
            new Color(1.0f, 0.5f, 0.0f, 1.0f)
        );

        // Play an explosion sound!
        string[] explosionSounds =
        {
            "dynamite1",
            "dynamite2",
            "dynamite3",
            "dynamite4",
            "dynamite5"
        };

        AudioSource audio = explodeCharge.GetComponent<AudioSource>();
        AudioClip sound = Resources.Load<AudioClip>(explosionSounds[UnityEngine.Random.Range(0, 5)]);
        audio.PlayOneShot(sound);
    }

    //maybe add another variable to the method that will grab the current explodeCharge's collider component?
    public void UpdateMovement(float explosionDuration)
    {
        secondsSinceDropped += explosionDuration;
        explodeCharge.transform.localScale += new Vector3 (explosionDuration, explosionDuration, 0);
    }
}

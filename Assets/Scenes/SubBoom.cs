using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEditorInternal;

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
    public float timeSinceLastTorpedo = 0f;
    public float timeUntilNextTorpedo;
    public GameObject submarine;

    public Submarine()
    {
        float depth = UnityEngine.Random.Range(-4.5f, 2);

        velocity = UnityEngine.Random.Range(0.5f, 1.5f);

        timeUntilNextTorpedo = UnityEngine.Random.Range(5.0f, 10.0f);

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
        AudioSource audio = submarine.AddComponent<AudioSource>();
        AudioClip sonar = Resources.Load<AudioClip>("submarine_sonar");
        audio.PlayOneShot(sonar);
    }

    public void UpdateMovement(float dt, List<Bubble> bubbles)
    {
        timeSinceLastTorpedo += dt;
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
        timeToExist = UnityEngine.Random.Range(0.5f, 1.0f);
        go = Utilities.newSpriteGameObject
        (
            "Bubble",
            Utilities.blankCircleTexture,
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
    public Vector2 direction;
    public GameObject torpedo;

    public Torpedo(Vector2 submarinePosition, Vector2 destroyerPosition)
    {
        direction = destroyerPosition - submarinePosition;
        direction.Normalize();
        torpedo = Utilities.newSpriteGameObject
        (
            "Torpedo",
            Utilities.blankSquareTexture,
            new Vector3(0.1f, 0.1f, 1),
            submarinePosition,
            new Color(1, 0, 0, 1)
        );
    }

    public void UpdateMovement(float duration, List<Bubble> bubbles)
    {
        Vector3 torpedoMovement = torpedo.transform.position;
        torpedoMovement.x += direction.x * 2 * duration;
        torpedoMovement.y += direction.y * 2 * duration;
        torpedo.transform.position = torpedoMovement;

        SpriteRenderer renderer = torpedo.GetComponent<SpriteRenderer>();
        Vector2 bubblePosition = torpedoMovement;
        bubblePosition.y -= (renderer.bounds.size.y / 2.0f);
        bubbles.Add(new Bubble(bubblePosition));
    }
}

public class SubBoom : MonoBehaviour
{
    [SerializeField]
    Text scoreText;

    [SerializeField]
    public Text depthText;

    GameObject ocean;
    GameObject destroyer;

    MainMenu mainMenuScript;

    List<Submarine> submarines;
    List<DepthCharge> depthCharges;
    List<ExplosionEffect> explosions;
    List<Torpedo> torpedos;
    List<Bubble> bubbles;

    GameObject currentDepthCharge;

    float timePlayed = 0.0f;
    float timeSinceSubAdded = 0.0f;
    ulong score = 0;
    ulong depth = 0;

    public GameObject gameOverScreen;
    public bool isGameActive = false;

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

        isGameActive = true;
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timePlayed += Time.deltaTime;

        Vector2 pos = destroyer.transform.position;

        //if the game is active, the player can interact using the destroyer
        if (isGameActive == true)
        {
            // Handle user input
            if (Input.GetKeyDown("space"))
            {
                depthCharges.Add(new DepthCharge(pos));
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
        }

        // Update torpedos
        List<Torpedo> explodedTorpedos = new List<Torpedo>();
        foreach (var torp in torpedos)
        {
            torp.UpdateMovement(Time.deltaTime, bubbles);
            if (torp.torpedo.transform.position.y > 2.9)
            {
                explodedTorpedos.Add(torp);
            }
        }

        foreach (var torp in explodedTorpedos)
        {
            explosions.Add(new ExplosionEffect(new Vector2
            (
                torp.torpedo.transform.position.x,
                torp.torpedo.transform.position.y))
            );

            Destroy(torp.torpedo);
            torpedos.Remove(torp);
        }

        // Update submarine positions
        foreach (var sub in submarines)
        {
            sub.UpdateMovement(Time.deltaTime, bubbles);

            if (sub.timeSinceLastTorpedo >= sub.timeUntilNextTorpedo)
            {
                torpedos.Add(new Torpedo(sub.submarine.transform.position, pos));
                sub.timeSinceLastTorpedo = 0f;
                sub.timeUntilNextTorpedo = UnityEngine.Random.Range(5.0f, 10.0f);
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
            dc.UpdateMovement(bubbles);

            //need to figure out a way for the DepthCharge() GetKeyUp movement to finish before exploding it
            if (Input.GetKeyUp("space") && depthCharges.Count > 0 && DepthCharge.hasChargeFullyTraveled == true)
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
        foreach (var ec in explosions)
        {
            if (ec.secondsSinceDropped >= ec.timeUntilExplode)
            {
                Destroy(ec.explodeCharge);
                explosions.Remove(ec);
                break;
            }
            ec.UpdateMovement();
        }

        //loop that checks if any items in explosions list is touching a submarine collider or destroyer collider
        //if so, destroy submarine and/or if destroyer, game over

        //update: it may be easier to try the method OnCollisionEnter2D
        //if we are able to get both box colliders somehow
        HashSet<Submarine> deadSubmarines = new HashSet<Submarine>();
        foreach (var ec in explosions)
        {
            foreach (var sub in submarines)
            {
                //this isn't working for some reason? not even the debug message is showing up.
                if (ec.explodeCharge.GetComponent<BoxCollider2D>().IsTouching
                   (sub.submarine.GetComponent<BoxCollider2D>()) == true)
                {
                    deadSubmarines.Add(sub);
                }
            }

            foreach (var sub in deadSubmarines)
            {
                Destroy(sub.submarine);
                submarines.Remove(sub);

                score += 1;
            }

            if (ec.explodeCharge.GetComponent<BoxCollider2D>().IsTouching(destroyer.GetComponent<BoxCollider2D>()))
            {
                destroyer.SetActive(false);
            }
        }

        //instead of destroying the player object, we can deactivate it instead
        //that way, the game can be optimized a little bit
        if (destroyer.activeInHierarchy == false && isGameActive)
        {
            isGameActive = false;
            gameOverScreen.SetActive(true);
            SaveGameStats();
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

    public void GameOverClick()
    {
        gameOverScreen.SetActive(true);
    }

    public void RestartClick()
    {
        SceneManager.LoadScene("SubBoomScene", LoadSceneMode.Single);
    }

    public void MainMenuClick()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }

    void SaveGameStats()
    {
        GameData gd = GameDataFileHandler.Load();
        DateTime now = DateTime.Now;

        gd.totalGamesPlayed += 1;
        gd.totalSecondsPlayed += (ulong)timePlayed;
        gd.lastScore = score;
        gd.lastScoreDateTime = now.ToString();

        if (score >= gd.highScore)
        {
            gd.highScore = score;
            gd.highScoreDateTime = now.ToString();
        }

        GameDataFileHandler.Save(gd);
    }

    void OnApplicationQuit()
    {
        if (isGameActive)
        {
            SaveGameStats();
        }
    }
}

public class DepthCharge
{
    public GameObject depthCharge;

    public bool hasChargeFullyTraveled = false;

    public DepthCharge(Vector2 destroyerPosition)
    {
        depthCharge = Utilities.newSpriteGameObject
        (
            "Depth Charge",
            Utilities.blankCircleTexture,
            new Vector3(0.5f, 0.5f, 1),
            destroyerPosition,
            new Color(0.0f, 0.0f, 0.0f, 1.0f)
        );

        // Play depth charge drop sound!
        AudioSource audio = depthCharge.AddComponent<AudioSource>();
        AudioClip sound = Resources.Load<AudioClip>("depth_charge_drop");
        audio.PlayOneShot(sound);
    }

    public void UpdateMovement(List<Bubble> bubbles)
    {
        Vector2 pos = depthCharge.transform.position;
        float speed = -0.5f;
        float velocity = 0f;

        SpriteRenderer renderer = depthCharge.GetComponent<SpriteRenderer>();
        if (Input.GetKey("space"))
        {
            velocity += Time.deltaTime * speed;
        }

        if (Input.GetKeyUp("space"))
        {
            //insert math equation here
            pos.y = velocity;
            depthCharge.transform.position = pos;

            //add bubbles
            Vector2 bubblePos = pos;
            bubblePos.y += (renderer.bounds.size.y / 2.0f);
            bubbles.Add(new Bubble(bubblePos));
        }

        hasChargeFullyTraveled = true;

        //depthText.text = "Depth: " + depth.ToString();
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

        AudioSource audio = explodeCharge.AddComponent<AudioSource>();
        AudioClip sound = Resources.Load<AudioClip>(explosionSounds[UnityEngine.Random.Range(0, 5)]);
        audio.PlayOneShot(sound);
    }

    //maybe add another variable to the method that will grab the current explodeCharge's collider component?
    public void UpdateMovement()
    {
        secondsSinceDropped += Time.deltaTime;
        explodeCharge.transform.localScale += new Vector3 (Time.deltaTime, Time.deltaTime, 0);
    }
}

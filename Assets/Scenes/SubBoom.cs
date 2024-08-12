using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

static class Utilities {
    public static GameObject newSpriteGameObject(string name, Vector3 localScale, Vector3 position, Color color) {
        GameObject go = new GameObject(name);
        go.transform.localScale = localScale;
        go.transform.position = position;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        Texture2D tex = Resources.Load<Texture2D>("blank_square");
        Sprite sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f, 0.5f), (float) tex.width);
        renderer.sprite = sprite;
        renderer.color = color;
        Rigidbody2D body = go.AddComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        return go;
    }
}

public class Submarine {
    float velocity;
    public GameObject submarine;

    public Submarine() {
        float depth = UnityEngine.Random.Range(-4.5f, 2);

        velocity = UnityEngine.Random.Range(0.5f, 1.5f);

        if (UnityEngine.Random.Range(0.0f, 1.0f) >= 0.5f) {
            velocity *= -1;
        }

        Vector3 initialPosition;
        if (velocity < 0) {
            initialPosition = new Vector3(12, depth, 0);
        } else {
            initialPosition = new Vector3(-12, depth, 0);
        }

        submarine = Utilities.newSpriteGameObject(
            "Submarine",
            new Vector3(2, 0.5f, 1),
            initialPosition,
            new Color(1.0f, 0.0f, 0.0f, 1.0f)
        );
    }

    public void UpdatePosition(float dt) {
        Vector3 pos = submarine.transform.position;
        pos.x += dt * velocity;

        // Wrap the subs around if they've wandered off screen
        if (pos.x < -12) {
            pos.x = 12;
        }

        if (pos.x > 12) {
            pos.x = -12;
        }

        submarine.transform.position = pos;
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

    GameObject currentDepthCharge;

    float timePlayed = 0.0f;
    float timeSinceSubAdded = 0.0f;
    ulong score = 0;

    // Start is called before the first frame update
    void Start()
    {
        ocean = Utilities.newSpriteGameObject(
            "Ocean",
            new Vector3(22, 8, 1),
            new Vector3(0, -1, 100),
            new Color(0.0f, 0.0f, 1.0f, 1.0f)
        );
    
        destroyer = Utilities.newSpriteGameObject(
            "Destroyer",
            new Vector3(3, 0.5f, 1),
            new Vector3(0, 3.1f, 0),
            new Color(0.0f, 0.0f, 0.0f, 1.0f)
        );

        submarines = new List<Submarine>();
        submarines.Add(new Submarine());

        depthCharges = new List<DepthCharge>();
        explosions = new List<ExplosionEffect>();
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
            score += 1;
        }

        if (Input.GetKey("right"))
        {
            pos.x += Time.deltaTime * 3;
            pos.x = Mathf.Min(pos.x, 9.25f);
            destroyer.transform.position = pos;
            score += 1;
        }

        if (Input.GetKey("escape")) {
            GameData gd = GameDataFileHandler.Load();
            DateTime now = DateTime.Now;
            gd.totalGamesPlayed += 1;
            gd.totalSecondsPlayed += (ulong)timePlayed;
            gd.lastScore = (ulong)score;
            gd.lastScoreDateTime = now.ToString();
            if (score >= gd.highScore) {
                gd.highScore = score;
                gd.highScoreDateTime = now.ToString();
            }
            GameDataFileHandler.Save(gd);

            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        }

        // Update submarine positions
        foreach (var sub in submarines) {
            sub.UpdatePosition(Time.deltaTime);
        }

        // Add a new submarine if it's been at least 10 seconds
        timeSinceSubAdded += Time.deltaTime;
        if (timeSinceSubAdded > 10) {
            timeSinceSubAdded = 0.0f;
            submarines.Add(new Submarine());
        }

        // Update depth charges, keep track of exploded charges
        List<DepthCharge> explodedCharges = new List<DepthCharge>();
        foreach (var dc in depthCharges) {
            dc.Update(Time.deltaTime);
            if (dc.secondsSinceDropped >= dc.timeUntilExplode) {
                explodedCharges.Add(dc);
            }
        }

        // Clear exploded depth charges
        // TODO: create an explosion object in its place
        foreach (var dc in explodedCharges) {
            explosions.Add(new ExplosionEffect(new Vector2(
                dc.depthCharge.transform.position.x,
                dc.depthCharge.transform.position.y)));

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
            ec.Update(Time.deltaTime);
        }

        //loop that checks if any items in explosions list is touching a submarine collider or destroyer collider
        //if so, destroy submarine and/or if destroyer, game over

        //update: it may be easier to try the method OnCollisionEnter2D
        //if we are able to get both box colliders somehow
        foreach (var ec in explosions)
        {
            foreach (var sub in submarines)
            {
                //this isn't working for some reason? not even the debug message is showing up.
                if (ec.explodeCharge.GetComponent<BoxCollider2D>().IsTouching(sub.submarine.GetComponent<BoxCollider2D>()) == true)
                {
                    Debug.Log("This works!");
                    Destroy(sub.submarine);
                    submarines.Remove(sub);
                }
            }
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
        Texture2D tex = Resources.Load<Texture2D>("blank_circle");
        Sprite sprite;
        SpriteRenderer renderer;

        depthCharge = new GameObject("Depth Charge");
        depthCharge.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        depthCharge.transform.position = destroyerPosition;

        renderer = depthCharge.AddComponent<SpriteRenderer>();
        renderer.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), (float)tex.width);
        renderer.sprite = sprite;
    }

    public void Update(float dt) {
        secondsSinceDropped += dt;
        Vector2 pos = depthCharge.transform.position;
        pos.y += dt * velocity;
        depthCharge.transform.position = pos;
    }
}

public class ExplosionEffect
{
    public GameObject explodeCharge;

    public float secondsSinceDropped = 0.0f;
    public float timeUntilExplode = 3.0f;

    public ExplosionEffect(Vector2 explosionPosition)
    {
        explodeCharge = Utilities.newSpriteGameObject(
            "Explosion",
            new Vector3(0.5f, 0.5f, 1),
            explosionPosition,
            new Color(1.0f, 0.5f, 0.0f, 1.0f)
        );
    }

    //maybe add another variable to the method that will grab the current explodeCharge's collider component?
    public void Update(float explosionDuration)
    {
        secondsSinceDropped += explosionDuration;
        explodeCharge.transform.localScale += new Vector3 (explosionDuration, explosionDuration, 0);
    }
}

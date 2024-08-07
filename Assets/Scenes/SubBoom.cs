using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        return go;
    }
}

public class Submarine {
    float velocity;
    GameObject go;

    public Submarine() {
        float depth = Random.Range(-4.5f, 2);

        velocity = Random.Range(0.5f, 1.5f);

        if (Random.Range(0.0f, 1.0f) >= 0.5f) {
            velocity *= -1;
        }

        Vector3 initialPosition;
        if (velocity < 0) {
            initialPosition = new Vector3(12, depth, 0);
        } else {
            initialPosition = new Vector3(-12, depth, 0);
        }

        go = Utilities.newSpriteGameObject(
            "Submarine",
            new Vector3(2, 0.5f, 1),
            initialPosition,
            new Color(1.0f, 0.0f, 0.0f, 1.0f)
        );
    }

    public void UpdatePosition(float dt) {
        Vector3 pos = go.transform.position;
        pos.x += dt * velocity;

        // Wrap the subs around if they've wandered off screen
        if (pos.x < -12) {
            pos.x = 12;
        }

        if (pos.x > 12) {
            pos.x = -12;
        }

        go.transform.position = pos;
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

    GameObject currentDepthCharge;

    float timeSinceSubAdded = 0.0f;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        ocean = Utilities.newSpriteGameObject(
            "Ocean",
            new Vector3(22, 8, 1),
            new Vector3(0, -1, 0),
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
    }

    // Update is called once per frame
    void Update()
    {
        //Nikki
        //changed vector3 to vector2 since we will only be messing with 2 axes
        Vector2 pos = destroyer.transform.position;

        // Handle user input
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            //Nikki
            //get destroyer pos
            //create charge from destroyer pos with user input
            //have charge move in a linear direction downwards
            //user input then blows up charge to destroy submarine
            depthCharges.Add(new DepthCharge(pos));
            //need to figure out how to grab the index of the depthCharges List
            //currentDepthCharge = depthCharges[0];
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
            Destroy(dc.depthCharge);
            depthCharges.Remove(dc);
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

    public void ExplodeCharge()
    {
        //first check for user input when space key is released
        //if a charge's collision makes contact with a submarine's collision
        //create explosion effect, deactivate/delete charge, and deactivate/delete submarine
        Debug.Log("Charge Exploded!");
    }

    public void Update(float dt) {
        secondsSinceDropped += dt;
        Vector2 pos = depthCharge.transform.position;
        pos.y += dt * velocity;
        depthCharge.transform.position = pos;
    }
}

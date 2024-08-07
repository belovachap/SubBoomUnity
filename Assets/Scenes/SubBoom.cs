using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    GameObject ocean;
    GameObject destroyer;
    GameObject depthCharge;
    List<Submarine> submarines;
    float timeSinceSubAdded = 0.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = destroyer.transform.position;

        // Handle user input
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            //Nikki
            //get destroyer pos
            //create charge from destroyer pos with user input
            //have charge move in a linear direction downwards
            //user input then blows up charge to destroy submarine
            CreateCharge(pos);
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
    }

    void CreateCharge(Vector3 destroyerPosition)
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

        //Nikki
        //neither of these inputs work since the if statement to make this method happen in the first place only checks if the space key was pressed
        //not if the space key was held or released
        if (Input.GetKey("space"))
            depthCharge.transform.Translate(Vector2.down * Time.deltaTime * 1.1f);

        if (Input.GetKeyUp("space"))
        {
            Debug.Log("Space Key was released!");
        }
    }
}

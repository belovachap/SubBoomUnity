using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine {
    float depth;
    float velocity;
    GameObject go;

    public Submarine() {
        depth = Random.Range(-4.5f, 2);
        velocity = Random.Range(-1, 1);
        go = new GameObject("Submarine");
        go.transform.localScale = new Vector3(2, 0.5f, 1);
        go.transform.position = new Vector3(0, depth, 0);
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        Texture2D tex = Resources.Load<Texture2D>("blank_square");
        Sprite sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f, 0.5f), (float) tex.width);
        renderer.sprite = sprite;
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
    List<Submarine> submarines;

    // Start is called before the first frame update
    void Start()
    {
        Texture2D tex = Resources.Load<Texture2D>("blank_square");
        Sprite sprite;
        SpriteRenderer renderer;

        // Setup the ocean background
        ocean = new GameObject("Ocean");
        ocean.transform.localScale = new Vector3(22, 8, 1);
        ocean.transform.position = new Vector3(0, -1, 0);
        renderer = ocean.AddComponent<SpriteRenderer>();
        renderer.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f, 0.5f), (float) tex.width);
        renderer.sprite = sprite;
    
        // Setup the destroyer
        destroyer = new GameObject("Destroyer");
        destroyer.transform.localScale = new Vector3(3, 0.5f, 1);
        destroyer.transform.position = new Vector3(0, 3.1f, 0);
        renderer = destroyer.AddComponent<SpriteRenderer>();
        renderer.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f, 0.5f), (float) tex.width);
        renderer.sprite = sprite;

        // Setup empty submarines list
        submarines = new List<Submarine>();
        submarines.Add(new Submarine());
    }

    // Update is called once per frame
    void Update()
    {
        // Handle user input
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            destroyer.transform.position = new Vector3(0, 3.1f, 0);
        }

        if (Input.GetKey("left"))
        {
            Vector3 pos = destroyer.transform.position;
            pos.x -= Time.deltaTime * 3;
            pos.x = Mathf.Max(pos.x, -9.25f);
            destroyer.transform.position = pos;
        }

        if (Input.GetKey("right"))
        {
            Vector3 pos = destroyer.transform.position;
            pos.x += Time.deltaTime * 3;
            pos.x = Mathf.Min(pos.x, 9.25f);
            destroyer.transform.position = pos;
        }

        // Update submarine positions
        foreach (var sub in submarines) {
            sub.UpdatePosition(Time.deltaTime);
        }
    }
}

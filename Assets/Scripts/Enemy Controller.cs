using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using static UnityEditor.PlayerSettings;

public class EnemyController : MonoBehaviour
{
    public GameObject bubbleParticles;

    BoxCollider2D enemyCollider;
    SpriteRenderer spriteRenderer;

    float velocity;
    float depth;

    // Start is called before the first frame update
    void Start()
    {
        enemyCollider = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    void Setup()
    {
        depth = UnityEngine.Random.Range(-4.5f, 2);
        velocity = UnityEngine.Random.Range(0.5f, 1.5f);

        if (UnityEngine.Random.Range(0.0f, 1.0f) >= 0.5f)
        {
            velocity *= -1;
            bubbleParticles.transform.position = -bubbleParticles.transform.position;
            bubbleParticles.transform.localScale = -bubbleParticles.transform.localScale;
        }

        Vector3 initialPosition;
        if (velocity < 0)
        {
            initialPosition = new Vector3(10, depth, 0);
        }
        else
        {
            initialPosition = new Vector3(-10, depth, 0);
        }

        gameObject.transform.position = initialPosition;
    }

    void UpdateMovement()
    {
        Vector3 pos = gameObject.transform.position;
        pos.x += velocity * Time.deltaTime;

        // wrap the submarines around if they've wandered off screen
        if (pos.x < -10)
        {
            pos.x = 10;
        }

        if (pos.x > 10)
        {
            pos.x = -10;
        }

        gameObject.transform.position = pos;
    }
}

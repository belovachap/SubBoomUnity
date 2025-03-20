using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using static UnityEditor.PlayerSettings;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject bubbleParticles;

    BoxCollider2D enemyCollider;
    SpriteRenderer spriteRenderer;

    protected float timeSinceLastTorpedo = 0f;
    protected float timeUntilNextTorpedo;

    private Vector3 currentPos;
    private float speed, depth;

    // plays sonar sound to introduce new submarine
    private AudioSource source;
    [SerializeField] private AudioClip sonarClip;

    void Start()
    {
        // instantiates the collider and sprite renderer
        enemyCollider = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // instantiates the depth (y spawn level) and how fast the sub travels
        depth = Random.Range(-4.5f, 2);
        speed = Random.Range(0.5f, 1.5f);

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        currentPos.x += speed * Time.deltaTime;

        timeSinceLastTorpedo += Time.deltaTime;

        // wrap the enemies around if they've wandered off screen
        if (currentPos.x < -13)
        {
            currentPos.x = 13;
        }

        if (currentPos.x > 13)
        {
            currentPos.x = -13;
        }

        gameObject.transform.position = currentPos;

        if (timeSinceLastTorpedo >= timeUntilNextTorpedo)
        {
            timeSinceLastTorpedo = 0f;
            timeUntilNextTorpedo = Random.Range(5.0f, 10.0f);
        }
    }

    private void Setup()
    {
        // sets up the audio source and audio clip components
        // so that when the enemy spawns, a sonar sound will play
        source = gameObject.GetComponent<AudioSource>();
        source.clip = sonarClip;
        source.Play();

        timeUntilNextTorpedo = Random.Range(5.0f, 10.0f);

        if (Random.Range(0.0f, 1.0f) >= 0.5f)
        {
            speed *= -1;

            // flips the bubbleParticles positions to be behind the submarine
            var flipPos = bubbleParticles.transform.position;
            flipPos.x *= -1f;
            bubbleParticles.transform.position = flipPos;

            // flips the bubbleParticles localScale to be behind the submarine
            var flipScale = bubbleParticles.transform.localScale;
            flipScale.x *= -1f;
            flipScale.y *= -1f;
            bubbleParticles.transform.localScale = flipScale;
        }

        if (speed < 0)
        {
            currentPos = new Vector3(13, depth, 0);
        }
        else
        {
            currentPos = new Vector3(-13, depth, 0);
        }

        gameObject.transform.position = currentPos;
    }
}

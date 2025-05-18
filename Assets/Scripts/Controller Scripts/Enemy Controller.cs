using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using static UnityEditor.PlayerSettings;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject bubbleParticles;
    private GameObject torpManager;

    protected float timeSinceLastTorpedo = 0f;
    protected float timeUntilNextTorpedo;

    private Vector3 spawnPos, currentPos;
    private float speed, depth;

    // plays sonar sound to introduce new submarine
    private AudioSource source;
    [SerializeField] private AudioClip sonarClip;

    void Awake()
    {
        // instantiates the depth (y spawn level) and how fast the sub travels
        depth = Random.Range(-4.5f, 1);
        speed = Random.Range(0.5f, 1.5f);

        torpManager = GameObject.Find("Torpedo Manager");

        Setup();
    }

    private void Setup()
    {
        // sets up the audio source and audio clip components
        // so that when the enemy spawns, a sonar sound will play
        source = gameObject.GetComponent<AudioSource>();
        source.clip = sonarClip;
        source.Play();

        timeUntilNextTorpedo = Random.Range(5.0f, 10.0f);

        // will randomly generate either 0, or 1.
        // if the integer generated is 0, the submarine will move the other way instead
        if (Random.Range(0, 2) == 0)
        {
            speed *= -1;

            // flips the submarine localScale to move in the opposite direction
            Vector3 flipSubScale = new Vector3(
                                          gameObject.transform.localScale.x * -1f,
                                          gameObject.transform.localScale.y,
                                          gameObject.transform.localScale.z
                                          );
            gameObject.transform.localScale = flipSubScale;

            // flips the bubbleParticles localScale to be behind the submarine
            Vector3 flipBubbleScale = new Vector3(
                                             bubbleParticles.transform.localScale.x * -1f,
                                             bubbleParticles.transform.localScale.y,
                                             bubbleParticles.transform.localScale.z
                                             );
            bubbleParticles.transform.localScale = flipBubbleScale;
            
        }

        if (speed < 0)
        {
            spawnPos = new Vector3(13, depth, 0);
        }
        else
        {
            spawnPos = new Vector3(-13, depth, 0);
        }

        gameObject.transform.position = spawnPos;
    }

    void Update()
    {
        // updates current position variable to transform
        currentPos = gameObject.transform.position;

        // submarine movement
        currentPos.x += speed * Time.deltaTime;

        // left boundary that teleports enemy to right side
        if (gameObject.transform.position.x < -13f)
        {
            currentPos.x = 13f;
        }

        // right boundary that teleports enemy to left side
        if (gameObject.transform.position.x > 13f)
        {
            currentPos.x = -13f;
        }

        gameObject.transform.position = currentPos;

        timeSinceLastTorpedo += Time.deltaTime;
        if (timeSinceLastTorpedo >= timeUntilNextTorpedo)
        {
            TorpedoHandler(gameObject.transform.position);
        }
    }

    void TorpedoHandler(Vector3 subPos)
    {
        GameObject torp = torpManager.GetComponent<ObjectPooler>().ObjectManager();

        if (torp != null)
        {
            torp.transform.position = subPos;
            torp.SetActive(true);
        }

        timeSinceLastTorpedo = 0f;
        timeUntilNextTorpedo = Random.Range(5.0f, 10.0f);
    }
}

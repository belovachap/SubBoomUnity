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

    void Start()
    {
        // instantiates the depth (y spawn level) and how fast the sub travels
        depth = Random.Range(-4.5f, 2);
        speed = Random.Range(0.5f, 1.5f);

        torpManager = GameObject.Find("Torpedo Manager");

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        // updates current position variable to transform
        currentPos = gameObject.transform.position;

        // submarine movement
        currentPos.x += speed * Time.deltaTime;

        // left boundary that teleports enemy to right side
        if (gameObject.transform.position.x < -13)
        {
            currentPos.x = 13;
        }

        // right boundary that teleports enemy to left side
        if (gameObject.transform.position.x > 13)
        {
            currentPos.x = -13;
        }

        gameObject.transform.position = currentPos;

        timeSinceLastTorpedo += Time.deltaTime;
        if (timeSinceLastTorpedo >= timeUntilNextTorpedo)
        {
            TorpedoHandler(gameObject.transform.position);
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
            spawnPos = new Vector3(13, depth, 0);
        }
        else
        {
            spawnPos = new Vector3(-13, depth, 0);
        }

        gameObject.transform.position = spawnPos;
    }

    void TorpedoHandler(Vector3 subPos)
    {
        // GameObject torp = ObjectPooler.SharedInstance.TorpedoManager();

        // TODO:
        // finish object pooler stuff here
        GameObject torp = torpManager.GetComponent<ObjectPooler>().ObjectManager();

        if (torp != null)
        {
            torp.transform.position = subPos;
            torp.SetActive(true);

            Debug.Log("Torpedo launched!");
        }

        timeSinceLastTorpedo = 0f;
        timeUntilNextTorpedo = Random.Range(5.0f, 10.0f);
    }
}

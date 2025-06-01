using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using static UnityEditor.PlayerSettings;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject bubbleParticles;
    private GameObject torpManager;

    private PlayerController playerController;

    protected float timeSinceLastTorpedo = 0f;
    protected float timeUntilNextTorpedo;

    private Vector3 spawnPos, currentPos;

    private float speed = 0, depth;

    public bool statsChanged = false;
    private bool facingRight = true;

    // plays sonar sound to introduce new submarine
    private AudioSource source;
    [SerializeField] private AudioClip sonarClip;

    void Awake()
    {
        torpManager = GameObject.Find("Torpedo Manager");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        source = gameObject.GetComponent<AudioSource>();
        source.clip = sonarClip;

        if (!statsChanged)
            Setup();
    }

    private void Setup()
    {
        // instantiates the depth (y spawn level) and how fast the sub travels
        depth = Random.Range(-4.5f, 1);

        // sets the speed
        speed = Random.Range(0.5f, 1.5f);

        // rolls between 0 or 1, if 0, reverse submarine
        if (Random.Range(0, 2) == 0)
        {
            speed *= -1f;
            Flip();
        }

        timeUntilNextTorpedo = Random.Range(5.0f, 10.0f);

        if (speed < 0)
        {
            spawnPos = new Vector3(13, depth, 0);
        }
        else
        {
            spawnPos = new Vector3(-13, depth, 0);
        }

        gameObject.transform.position = spawnPos;

        statsChanged = true;
    }

    void Flip()
    {
        // flips the submarine localScale to move in the opposite direction
        transform.localScale = new Vector3(
            gameObject.transform.localScale.x * -1f,
            gameObject.transform.localScale.y,
            gameObject.transform.localScale.z);

        // flips the bubbleParticles localScale to be behind the submarine
        bubbleParticles.transform.localScale = new Vector3(
            bubbleParticles.transform.localScale.x * -1f,
            bubbleParticles.transform.localScale.y * -1f,
            bubbleParticles.transform.localScale.z);

        if (facingRight)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

    void Update()
    {
        if (playerController.isGameActive && gameObject.activeSelf)
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

            // if horizontal movement is right AND sub is facing left
            // OR if horizontal movement is left AND sub is facing right
            // flip sub sprite
            if ((speed > 0 && !facingRight) || (speed < 0 && facingRight))
            {
                Flip();
            }

            gameObject.transform.position = currentPos;

            timeSinceLastTorpedo += Time.deltaTime;
            if (timeSinceLastTorpedo >= timeUntilNextTorpedo)
            {
                TorpedoHandler(gameObject.transform.position);
            }
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

    public void WaitToRespawn()
    {
        if (!gameObject.activeSelf)
        {
            if (!statsChanged)
                Setup();

            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        }
    }

    public void PlaySpawnSound()
    {
        // sets up the audio source and audio clip components
        // so that when the enemy spawns, a sonar sound will play
        if (playerController.isGameActive)
        {
            source.Play();
        }
    }
}

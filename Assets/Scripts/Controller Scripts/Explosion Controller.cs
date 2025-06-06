using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private float timer = 0.0f;
    private readonly float duration = 1.5f;
    private readonly float scale = 0.75f;

    private AudioSource source;
    [SerializeField] private AudioClip[] explosionClips = new AudioClip[5];

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        source = gameObject.GetComponent<AudioSource>();
        PlaySpawnSound();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                gameObject.transform.localScale += new Vector3(Time.deltaTime * scale, Time.deltaTime * scale, 1);
            }

            if (timer >= duration)
            {
                timer = 0.0f;

                gameObject.SetActive(false);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void CreateExplosion(Vector3 spawnPosition)
    {
        gameObject.transform.position = spawnPosition;
        // PlaySpawnSound();
    }

    private void PlaySpawnSound()
    {
        source.clip = explosionClips[UnityEngine.Random.Range(0, 5)];
        source.Play();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.activeInHierarchy)
        {
            // TODO:
            // figure out how to set a timer to delay the respawn of the submarine
            other.gameObject.SetActive(false);

            other.GetComponent<EnemyController>().WaitToRespawn();

            gameManager.UpdateScore(10);
        }
        else if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            gameManager.GameOverScreen();
        }
    }
}

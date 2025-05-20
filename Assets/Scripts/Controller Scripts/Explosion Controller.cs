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

    private SubmarinePooler subPooler;

    private void Start()
    {
        subPooler = GameObject.Find("Submarine Manager").GetComponent<SubmarinePooler>();

        source = gameObject.GetComponent<AudioSource>();
        source.clip = explosionClips[UnityEngine.Random.Range(0, 5)];

        source.Play();
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
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Submarine(Clone)")
        {
            other.gameObject.SetActive(false);

            // TODO:
            // figure out how to move submarine to outside the map
            // once deactivated and stay there for a certain amount of time
            // before reactivating
            other.gameObject.GetComponent<EnemyController>().statsChanged = false;
            other.gameObject.GetComponent<EnemyController>().WaitToRespawn();
        }

        /*
        if (other.gameObject.name == "Player")
        {
            other.gameObject.SetActive(false);
        }
        */
    }
}

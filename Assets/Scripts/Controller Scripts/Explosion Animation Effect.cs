using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimationEffect : MonoBehaviour
{
    private float timer = 0.0f;
    private float duration = 2.0f;

    private AudioSource source;

    // creates a serialized list of all the explosion sounds
    [SerializeField]
    private AudioClip[] explosionClips = new AudioClip[5];

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.clip = explosionClips[UnityEngine.Random.Range(0, 5)];

        source.Play();
    }

    // maybe add another variable to the method that will grab the current explodeCharge's collider component?
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                gameObject.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 1);
            }

            if (timer >= duration)
            {
                timer = 0.0f;

                gameObject.SetActive(false);

                gameObject.transform.localScale = new Vector3(1, 1, 1);
                // source.clip = explosionClips[UnityEngine.Random.Range(0, 5)];
            }
        }
    }

    public void CreateExplosion(Vector3 spawnPosition)
    {
        gameObject.transform.position = spawnPosition;
    }
}

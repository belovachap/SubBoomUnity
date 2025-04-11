using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimationEffect : MonoBehaviour
{
    private float secondsSinceDropped = 0.0f;
    private float timeUntilExplode = 3.0f;

    private AudioSource source;
    // creates a serialized list of all the explosion sounds we have
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
        secondsSinceDropped += Time.deltaTime;
        gameObject.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
    }

    public void CreateExplosion(Vector2 spawnPosition)
    {
        gameObject.transform.position = spawnPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimationEffect : MonoBehaviour
{
    private float timer = 0.0f;
    private readonly float duration = 1.5f;
    private readonly float scale = 0.75f;

    private AudioSource source;

    [SerializeField] private AudioClip[] explosionClips = new AudioClip[5];

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
}

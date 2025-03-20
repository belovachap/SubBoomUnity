using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargeController : PlayerController
{
    private float timeExisted;
    // private float timeToExist;

    public float spawnDuration;

    private AudioSource source;
    [SerializeField] private AudioClip dropClip;

    private Vector2 currentPos;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.clip = dropClip;
        source.Play();

        // timeToExist = spawnDuration;
        Debug.Log("The spawnDuration is: " + spawnDuration);
    }
}

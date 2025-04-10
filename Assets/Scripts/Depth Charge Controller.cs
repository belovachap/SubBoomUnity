using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargeController : MonoBehaviour
{
    private readonly float speed = 0.6f;

    private AudioSource source;
    [SerializeField] private AudioClip dropClip;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.clip = dropClip;
        source.Play();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.down);
        }
    }
}
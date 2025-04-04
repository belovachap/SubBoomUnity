using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargeController : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip dropClip;

    private GameObject player;

    private float speed = 0.6f;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.clip = dropClip;
        source.Play();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.down);
        }
    }
}

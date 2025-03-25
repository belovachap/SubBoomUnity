using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargeController : PlayerController
{
    public float timeActive = 0;
    public float spawnDuration;

    private AudioSource source;
    [SerializeField] private AudioClip dropClip;

    private Vector3 currentPos;
    private float speed = -2f;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.clip = dropClip;
        source.Play();

        Debug.Log("The spawnDuration is: " + spawnDuration);
    }

    private void Update()
    {

    }

    public void UpdateMovement(Vector3 playerCoords)
    {
        currentPos = playerCoords;
        gameObject.transform.position = currentPos;

        while (timeActive < spawnDuration)
        {
            /*
            gameObject.transform.position = playerCoords;
            gameObject.transform.position += new Vector3(0, -0.65f * Time.deltaTime, 0);
            */

            currentPos.y += speed * Time.deltaTime;
            gameObject.transform.position = currentPos;

            timeActive += Time.deltaTime;
        }
    }
}

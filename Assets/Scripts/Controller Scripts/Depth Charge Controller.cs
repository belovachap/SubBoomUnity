using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargeController : MonoBehaviour
{
    private float timer = 0;
    private float spawnDuration = 0;
    private Vector3 distance;

    private AudioSource source;
    // [SerializeField] private AudioClip dropClip;

    private GameObject expManager;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        // source.clip = dropClip;
        source.Play();

        expManager = GameObject.Find("Explosion Manager");
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            // vertical movement of depth charge
            transform.Translate(distance);

            if (timer < spawnDuration)
            {
                timer += Time.deltaTime;
            }

            if (timer >= spawnDuration)
            {
                timer = 0;
                GameObject exp = expManager.GetComponent<ObjectPooler>().ObjectManager();
                exp.SetActive(true);
                exp.GetComponent<ExplosionController>().CreateExplosion(gameObject.transform.position);

                gameObject.SetActive(false);
            }
        }
    }

    public void SetSpawnDuration(float incTimeHeld)
    {
        spawnDuration = incTimeHeld;
    }

    public void SetDistance(Vector3 incDistance)
    {
        distance = incDistance;
    }
}
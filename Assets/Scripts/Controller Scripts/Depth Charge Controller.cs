using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DepthChargeController : MonoBehaviour
{
    private Rigidbody rb;

    private const float speed = 0.6f;
    private float timer = 0;
    public float spawnDuration = 0;

    private AudioSource source;
    [SerializeField] private AudioClip dropClip;

    private GameObject expManager;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        source = gameObject.GetComponent<AudioSource>();
        source.clip = dropClip;
        source.Play();

        expManager = GameObject.Find("Explosion Manager");
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            // vertical movement of depth charge
            transform.Translate(speed * Time.deltaTime * Vector3.down, Space.World);

            if (timer < spawnDuration)
            {
                timer += Time.deltaTime;
            }

            if (timer >= spawnDuration)
            {
                timer = 0;

                // TODO:
                // figure out why explosion animations are only playing SOMETIMES

                for (int i = 0; i < expManager.GetComponent<ObjectPooler>().objList.Count; i++)
                {
                    GameObject exp = expManager.GetComponent<ObjectPooler>().ObjectManager();

                    // if object is NOT active, that means we can use it
                    if (exp != null)
                    {
                        // sets available explosion effect to active
                        exp.SetActive(true);

                        //creates explosion at the depth charge position before it deactivates
                        exp.GetComponent<ExplosionController>().CreateExplosion(gameObject.transform.position);
                    }
                }

                gameObject.SetActive(false);
            }
        }
    }
}
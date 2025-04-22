using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TorpedoController : MonoBehaviour
{
    public const float speed = 0.5f;

    private GameObject player;
    private Vector3 playerPos;
    private bool check = false;

    private float distance;

    private AudioSource source;
    [SerializeField] private AudioClip dropClip;

    private GameObject expManager;

    // Start is called before the first frame update
    void Start()
    {
        /*
        source = gameObject.GetComponent<AudioSource>();
        source.clip = dropClip;
        source.Play();
        */

        Debug.Log("TODO: Play Torpedo Sound!");

        player = GameObject.FindGameObjectWithTag("Player");

        expManager = GameObject.Find("Explosion Manager");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            // distance = CalculateDistance(gameObject.transform.position, player.GetComponent<PlayerController>().GetPlayerPosition());

            if (!check)
            {
                playerPos = player.GetComponent<PlayerController>().GetPlayerPosition();
                check = true;
            }

            transform.Translate(player.transform.position.x * Time.deltaTime, player.transform.position.y * Time.deltaTime, 0);

            if (gameObject.transform.position.y >= playerPos.y)
            {
                Debug.Log("Torpedo Exploded!");
                check = false;

                // TODO:
                // create explosion animation here

                GameObject exp = expManager.GetComponent<ObjectPooler>().ObjectManager();

                // if object is NOT active, that means we can use it
                if (exp != null)
                {
                    // sets available explosion effect to active
                    exp.SetActive(true);

                    //creates explosion at the depth charge position before it deactivates
                    exp.GetComponent<ExplosionAnimationEffect>().CreateExplosion(gameObject.transform.position);
                }

                gameObject.SetActive(false);
            }
        }
    }

    private float CalculateDistance(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, end);
    }
}

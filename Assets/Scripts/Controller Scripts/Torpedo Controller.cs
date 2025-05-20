using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TorpedoController : MonoBehaviour
{
    public const float speed = 1.25f;
    private float angle;

    private GameObject expManager, player;

    private Vector3 torpPos, playerPos, direction;

    private bool check = false;

    private AudioSource source;
    [SerializeField] private AudioClip dropClip;

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
            torpPos = transform.position;

            if (!check)
            {
                playerPos = player.transform.position;

                // gets transform of torpedo and transforms it with the local player position
                direction = transform.InverseTransformPoint(playerPos);

                // gets the angle of where the torpedo must face towards the player
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // rotates torpedo to the direction of the player
                transform.Rotate(0, 0, angle);

                check = true;
            }

            // moves the torpedo to the player location, which is stored in playerPos
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerPos, step);

            // if the torpPos Y is the same as the player's, then the torpedo will explode
            if (torpPos.y >= playerPos.y)
            {
                check = false;

                GameObject exp = expManager.GetComponent<ObjectPooler>().ObjectManager();

                // if object is NOT active, that means we can use it
                if (exp != null)
                {
                    // sets available explosion effect to active
                    exp.SetActive(true);

                    //creates explosion at the depth charge position before it deactivates
                    exp.GetComponent<ExplosionController>().CreateExplosion(torpPos);
                }

                gameObject.SetActive(false);
            }
        }
    }
}

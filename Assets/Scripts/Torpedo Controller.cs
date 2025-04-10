using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoController : MonoBehaviour
{
    public float speed = 0.5f;

    private GameObject player;
    private GameObject submarine;

    private float distance;
    public float duration = 3f;

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
        submarine = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            distance = CalculateDistance(submarine.transform.position, player.transform.position);

            gameObject.transform.position = Vector3.MoveTowards(
                                                    submarine.transform.position,
                                                    player.transform.position,
                                                    (distance/duration) * Time.deltaTime
                                                    );

            if (gameObject.transform.position.y >= 0)
            {
                Debug.Log("Torpedo Exploded!");
                // gameObject.SetActive(false);
            }
        }
    }

    private float CalculateDistance(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, end);
    }
}

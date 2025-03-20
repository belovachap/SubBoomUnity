using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject depthCharge;
    // [SerializeField] private List<GameObject> depthChargeList = new();

    private ulong depth = 0;

    private bool isGameActive = false;

    public float timePlayed = 0.0f;
    public float timeHeldSpace = 0f;

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        timePlayed += Time.deltaTime;

        Vector3 pos = gameObject.transform.position;

        // if the game is active, the player can interact using the destroyer
        if (isGameActive == true)
        {
            // handle user input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // once get key has been pressed, start a timer for how long it is held down
                // once get key up is true, timer stops and we use that time to add to the posy of depth charge
                timeHeldSpace = 0;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                timeHeldSpace += Time.deltaTime;
                depth = (ulong)(timeHeldSpace * 0.5 * 100);
                // depthSlider.value = depth;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                // TODO:
                // change this to object pooling
                /*
                GameObject dc = Instantiate(depthCharge);
                dc.GetComponent<DepthChargeController>().spawnDuration = timeHeldSpace;
                dc.transform.position = pos;
                depthChargeList.Add(dc);
                */

                // instantiates new gameobject as one of the gameobjects from the object pooler
                GameObject dc = ObjectPooler.SharedInstance.DepthChargeManager();

                // if the gameobject is not null, set it to active
                if (dc != null)
                {
                    // TODO:
                    // continue doing this next
                    // 3/18/25
                    dc.SetActive(true);

                    dc.GetComponent<DepthChargeController>().spawnDuration = timeHeldSpace;

                    float timer = 0;

                    while (timer < timeHeldSpace)
                    {
                        dc.transform.position = pos;
                        dc.transform.position += new Vector3 (0, -0.65f * Time.deltaTime, 0);

                        timer += Time.deltaTime;
                    }
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pos.x -= Time.deltaTime * 3;
                pos.x = Mathf.Max(pos.x, -9.25f);
                gameObject.transform.position = pos;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                pos.x += Time.deltaTime * 3;
                pos.x = Mathf.Min(pos.x, 9.25f);
                gameObject.transform.position = pos;
            }
        }
    }
}

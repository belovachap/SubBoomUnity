using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    public GameObject destroyer;
    public GameObject depthCharge;

    List<GameObject> depthChargeList = new();

    ulong depth = 0;

    public bool isGameActive = false;

    float timePlayed = 0.0f;
    float timeHeldSpace = 0f;

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
                GameObject dc = Instantiate(depthCharge);
                dc.GetComponent<DepthChargeController>().spawnDuration = timeHeldSpace;
                dc.transform.position = pos;
                depthChargeList.Add(dc);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pos.x -= Time.deltaTime * 3;
                pos.x = Mathf.Max(pos.x, -9.25f);
                destroyer.transform.position = pos;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                pos.x += Time.deltaTime * 3;
                pos.x = Mathf.Min(pos.x, 9.25f);
                destroyer.transform.position = pos;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject depthCharge;
    [SerializeField] private GameObject dcManager;

    private ulong depth = 0;

    private bool isGameActive = false;

    public float timePlayed = 0.0f;
    public float timeHeldSpace = 0f;

    private const float speed = 3f;

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

        float movement = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;

        // if the game is active, the player can interact using the destroyer
        if (isGameActive == true)
        {
            // horizontal movement
            transform.Translate(movement, 0, 0);

            // right boundary
            if (pos.x > 9)
            {
                pos.x = 9;
                gameObject.transform.position = pos;
            }

            // left boundary
            if (pos.x < -9)
            {
                pos.x = -9;
                gameObject.transform.position = pos;
            }

            DepthChargeInputs(pos);
        }
    }

    private void DepthChargeInputs(Vector3 playerPos)
    {
        // handle user input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // reset timer
            timeHeldSpace = 0;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            // once get key has been pressed, start a timer for how long it is held down
            timeHeldSpace += (Time.deltaTime * 5);
            depth = (ulong)(timeHeldSpace * 0.5 * 100);
            // depthSlider.value = depth;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // once get key up is true, timer stops and we use that time to add to the y-pos of depth charge
            DepthChargeHandler(playerPos);
        }
    }

    private void DepthChargeHandler(Vector3 playerPos)
    {
        // instantiates new gameobject as one of the gameobjects from the object pooler
        GameObject dc = dcManager.GetComponent<ObjectPooler>().ObjectManager();

        // if the gameobject is not null, set it to active
        if (dc != null)
        {
            dc.transform.position = playerPos + new Vector3(0, -0.25f, 0);
            dc.GetComponent<DepthChargeController>().spawnDuration = timeHeldSpace;

            dc.SetActive(true);

            Debug.Log("Time Held Space amount is: " + dc.GetComponent<DepthChargeController>().spawnDuration);
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return gameObject.transform.position;
    }
}

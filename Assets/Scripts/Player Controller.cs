using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject depthCharge;

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
                // reset timer
                timeHeldSpace = 0;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                // once get key has been pressed, start a timer for how long it is held down
                timeHeldSpace += Time.deltaTime;
                depth = (ulong)(timeHeldSpace * 0.5 * 100);
                // depthSlider.value = depth;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                // once get key up is true, timer stops and we use that time to add to the y-pos of depth charge
                DepthChargeHandler(pos);
            }

            // TODO:
            // change left and right inputs to horizontal Axis inputs
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

    private void DepthChargeHandler(Vector3 destroyerPos)
    {
        // instantiates new gameobject as one of the gameobjects from the object pooler
        GameObject dc = ObjectPooler.SharedInstance.DepthChargeManager();

        // if the gameobject is not null, set it to active
        if (dc != null)
        {
            dc.transform.position = destroyerPos;
            dc.SetActive(true);

            Debug.Log("Time Held Space amount is: " + timeHeldSpace);

            float timer = 0;

            if (timer < timeHeldSpace)
            {
                timer += Time.deltaTime;
            }

            // TODO:
            // figure out why depth charge isn't deactivating once timer is reached
            if (timer >= timeHeldSpace)
            {
                dc.SetActive(false);
            }

            Debug.Log("Depth Charge Movement is Completed!");
            // once timer is done, depthCharge is meant to explode
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return gameObject.transform.position;
    }
}

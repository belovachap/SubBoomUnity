using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject dcDisplay;
    [SerializeField] private GameObject dcManager;
    [SerializeField] private GameManager gameManager;

    private bool facingRight = true;
    private float timeHeldSpace = 0f;

    private const float speed = 3f;

    private const float dcSpeed = 0.8f;
    private Vector3 dcDistance;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if the game is active, the player can interact using the destroyer
        if (gameManager.isGameActive == true)
        {
            Vector3 pos = gameObject.transform.position;
            float horInput = Input.GetAxisRaw("Horizontal");

            float movement = horInput * speed * Time.deltaTime;

            // horizontal movement
            transform.Translate(movement, 0, 0);

            // if horizontal movement is right AND player is facing left
            // OR if horizontal movement is left AND player is facing right
            // flip player sprite
            if ((horInput > 0 && !facingRight) || (horInput < 0 && facingRight))
            {
                Flip();
            }

            // right boundary
            if (pos.x > 9.5f)
            {
                pos.x = 9.5f;
                gameObject.transform.position = pos;
            }

            // left boundary
            if (pos.x < -9.5f)
            {
                pos.x = -9.5f;
                gameObject.transform.position = pos;
            }

            DepthChargeInputs(pos);
        }

        /*
        if (!gameObject.activeInHierarchy && gameManager.isGameActive)
        {
            gameManager.isGameActive = false;
        }
        */
    }

    private void DepthChargeInputs(Vector3 playerPos)
    {
        // handle user input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // reset timer
            timeHeldSpace = 0;

            // activates depth charge to be visible
            dcDisplay.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            // once get key has been pressed, start a timer for how long it is held down
            timeHeldSpace += Time.deltaTime;

            // create formula for the depth charge distance travelled to send to the actual depth charge
            dcDistance = dcSpeed * Time.deltaTime * Vector3.down;

            // moves depth charge vertically downwards
            dcDisplay.transform.Translate(dcDistance);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // once get key up is true, timer stops and we use that time to add to the y-pos of depth charge
            DepthChargeHandler(playerPos);

            // deactivate depth charge
            dcDisplay.SetActive(false);
            dcDisplay.transform.position = transform.position;
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
            dc.GetComponent<DepthChargeController>().SetSpawnDuration(timeHeldSpace);
            dc.GetComponent<DepthChargeController>().SetDistance(dcDistance);

            dc.SetActive(true);
        }
    }

    void Flip()
    {
        gameObject.transform.localScale = new Vector3(
            gameObject.transform.localScale.x * -1f,
            gameObject.transform.localScale.y,
            gameObject.transform.localScale.z);

        if (facingRight)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject depthCharge;
    [SerializeField] private GameObject dcManager;

    private ulong depth = 0;

    private bool isGameActive = false;
    private bool facingRight = true;

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
        // if the game is active, the player can interact using the destroyer
        if (isGameActive == true)
        {
            timePlayed += Time.deltaTime;

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

        if (gameObject.activeInHierarchy == false && isGameActive)
        {
            isGameActive = false;
            // TODO:
            // call game over screen here
        }
    }

    private void DepthChargeInputs(Vector3 playerPos)
    {
        // handle user input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // reset timer
            timeHeldSpace = 0;

            // TO DO:
            // set depth charge movement to player movement
        }

        if (Input.GetKey(KeyCode.Space))
        {
            // once get key has been pressed, start a timer for how long it is held down
            timeHeldSpace += (Time.deltaTime * 5);
            depth = (ulong)(timeHeldSpace * 0.5 * 100);
            // depthSlider.value = depth;

            // TODO:
            // move depth charge downwards
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // once get key up is true, timer stops and we use that time to add to the y-pos of depth charge
            DepthChargeHandler(playerPos);

            // TODO:
            // deactivate depth charge
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
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return gameObject.transform.position;
    }

    void Flip()
    {
        Vector3 flipScale = new Vector3(
                                       gameObject.transform.localScale.x * -1f,
                                       gameObject.transform.localScale.y,
                                       gameObject.transform.localScale.z
                                       );
        gameObject.transform.localScale = flipScale;

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

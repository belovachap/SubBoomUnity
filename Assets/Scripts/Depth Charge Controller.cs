using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargeController : MonoBehaviour
{

    public GameObject depthCharge;

    public float timeExisted;
    public float timeToExist;

    public float spawnDuration;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audio = depthCharge.AddComponent<AudioSource>();
        AudioClip sound = Resources.Load<AudioClip>("depth_charge_drop");
        audio.PlayOneShot(sound);

        timeToExist = spawnDuration;
        Debug.Log("The spawnDuration is: " + spawnDuration);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    public void UpdateMovement()
    {
        Vector2 pos = depthCharge.transform.position;
        float speed = -0.65f;

        timeExisted += Time.deltaTime;

        //insert math equation here
        pos.y += speed * Time.deltaTime;
        depthCharge.transform.position = pos;
    }
}

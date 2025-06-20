using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SubmarinePooler : ObjectPooler
{
    private float timeSinceSubAdded = 3.0f;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public override GameObject ObjectManager()
    {
        // adds time to timeSinceSubAdded
        timeSinceSubAdded += Time.deltaTime;

        // add a new submarine if it's been at least 5 seconds
        if (timeSinceSubAdded >= 5)
        {
            // for as many objects as are in the submarineList
            for (int i = 0; i < objList.Count; i++)
            {
                // if the pooled objects is NOT active, activate that object to be visible 
                if (!objList[i].activeInHierarchy)
                {
                    objList[i].SetActive(true);
                    audioManager.PlaySFX(audioManager.submarineSFX);

                    break;
                }
            }
            timeSinceSubAdded = 0.0f;
        }
        // otherwise, return null
        return null;
    }
}

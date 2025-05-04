using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SubmarinePooler : ObjectPooler
{
    private float timeSinceSubAdded = 0.0f;
    public override GameObject ObjectManager()
    {
        // add a new submarine if it's been at least 5 seconds
        timeSinceSubAdded += Time.deltaTime;

        if (timeSinceSubAdded >= 5)
        {
            // for as many objects as are in the submarineList
            for (int i = 0; i < objList.Count; i++)
            {
                // if the pooled objects is NOT active, activate that object to be visible 
                if (!objList[i].activeInHierarchy)
                {
                    objList[i].SetActive(true);
                    break;
                }
            }
            timeSinceSubAdded = 0.0f;
        }
        // otherwise, return null
        return null;
    }
}

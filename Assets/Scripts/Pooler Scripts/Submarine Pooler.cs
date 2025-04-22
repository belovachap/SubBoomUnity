using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SubmarinePooler : ObjectPooler
{
    private float timeSinceSubAdded = 0.0f;

    [SerializeField] GameObject subPoolerObj;

    public SubmarinePooler()
    {
        numObjs = 10;
        poolerObj = subPoolerObj;
        objList = new(numObjs);

        new ObjectPooler(numObjs, poolerObj, objList);
    }

    public override GameObject ObjectManager()
    {
        // add a new submarine if it's been at least 5 seconds
        timeSinceSubAdded += Time.deltaTime;

        if (timeSinceSubAdded >= 5)
        {
            timeSinceSubAdded = 0.0f;

            // for as many objects as are in the submarineList
            for (int i = 0; i < objList.Count; i++)
            {
                // if the pooled objects is NOT active, activate that object to be visible 
                if (!objList[i].activeInHierarchy)
                {
                    // objList[i].SetActive(true);
                    return objList[i];
                    // break;
                }
            }
            // return null;
        }
        // otherwise, return null
        return null;
    }
}

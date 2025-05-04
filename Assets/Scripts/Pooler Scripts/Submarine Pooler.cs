using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SubmarinePooler : ObjectPooler
{
    private float timeSinceSubAdded = 0.0f;

    private void Awake()
    {
        poolerObj = base.poolerObj;
        numObjs = base.numObjs;
    }

    public override GameObject ObjectManager()
    {
        objList = base.objList;

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

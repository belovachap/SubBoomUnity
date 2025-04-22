using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChargePooler : ObjectPooler
{
    [SerializeField] GameObject dcPoolerObj;

    public DepthChargePooler()
    {
        numObjs = 5;
        poolerObj = dcPoolerObj;
        objList = new(numObjs);

        new ObjectPooler(numObjs, poolerObj, objList);
    }
}

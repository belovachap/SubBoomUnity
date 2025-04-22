using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoPooler : ObjectPooler
{
    [SerializeField] GameObject torpPoolerObj;

    public TorpedoPooler()
    {
        numObjs = 10;
        poolerObj = torpPoolerObj;
        objList = new(numObjs);

        new ObjectPooler(numObjs, poolerObj, objList);
    }
}

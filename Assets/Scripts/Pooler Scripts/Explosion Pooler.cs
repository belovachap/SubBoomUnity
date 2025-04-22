using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPooler : ObjectPooler
{
    [SerializeField] GameObject expPoolerObj;

    public ExplosionPooler()
    {
        numObjs = 25;
        poolerObj = expPoolerObj;
        objList = new(numObjs);

        new ObjectPooler(numObjs, poolerObj, objList);
    }
}

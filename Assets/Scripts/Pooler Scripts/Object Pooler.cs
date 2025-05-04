using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    protected GameObject managerObj;
    [SerializeField] protected GameObject poolerObj;

    [SerializeField] protected int numObjs;

    public List<GameObject> objList;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        SpawnObjects();
    }

    private void Update()
    {
        ObjectManager();
    }

    protected void SpawnObjects()
    {
        managerObj = gameObject;

        for (int i = 0; i < numObjs; i++)
        {
            // creates a new object
            GameObject obj = (GameObject)Instantiate(poolerObj);

            // hides the game object
            obj.SetActive(false);

            // adds obj to the object list
            objList.Add(obj);

            // adds object to the manager object
            obj.transform.SetParent(managerObj.transform);
        }
    }

    public virtual GameObject ObjectManager()
    {
        // for as many objects as are in the pooledObjects list
        for (int i = 0; i < objList.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!objList[i].activeInHierarchy)
            {
                return objList[i];
            }
        }
        // otherwise, return null   
        return null;
    }
}

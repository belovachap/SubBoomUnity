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

    /*
    [SerializeField] private GameObject subManager;
    [SerializeField] private GameObject submarine;
    private List<GameObject> submarineList = new();

    [SerializeField] private GameObject depthManager;
    [SerializeField] private GameObject depthCharge;
    private List<GameObject> depthChargeList = new();

    [SerializeField] private GameObject torpManager;
    [SerializeField] private GameObject torpedo;
    private List<GameObject> torpedoList = new();

    [SerializeField] private GameObject expManager;
    [SerializeField] private GameObject explosion;
    private List<GameObject> explosionList = new();

    private const int subCount = 10;
    private const int depthCount = 5;
    private const int explosionCount = subCount * depthCount;

    private float timeSinceSubAdded = 0.0f;


    // TODO: (4/13)
    // finish implementing explosionCount into object pooler
    // then, make sure the explosionAnimationEffect script works properly
    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        SpawnSubmarines();
        SpawnDepthCharges();
    }

    private void Update()
    {
        SubmarineManager();
        DepthChargeManager();
    }

    private void SpawnSubmarines()
    {
        for (int i = 0; i < subCount; i++)
        {
            // pooling submarines
            GameObject sub = (GameObject)Instantiate(submarine);

            // hides the game object so that not all 10 spawn when the game starts
            sub.SetActive(false);
            submarineList.Add(sub);

            // adds the submarines as a child object of the Sub Boom game object
            sub.transform.SetParent(GameObject.Find("Submarine Manager").transform);
        }

        SpawnTorpedos();
    }

    public void SubmarineManager()
    {
        // add a new submarine if it's been at least 5 seconds
        timeSinceSubAdded += Time.deltaTime;

        if (timeSinceSubAdded >= 5)
        {
            timeSinceSubAdded = 0.0f;

            // for as many objects as are in the submarineList
            for (int i = 0; i < submarineList.Count; i++)
            {
                // if the pooled objects is NOT active, activate that object to be visible 
                if (!submarineList[i].activeInHierarchy)
                {
                    submarineList[i].SetActive(true);
                    break;
                }
            }
        }
    }

    private void SpawnTorpedos()
    {
        for (int i = 0; i < subCount; i++)
        {
            // creates a new torpedo object
            GameObject torp = (GameObject)Instantiate(torpedo);

            // hides the game object
            torp.SetActive(false);

            // adds torpedo to torpedo list
            torpedoList.Add(torp);

            // adds torpedo to each submarine
            torp.transform.SetParent(submarineList[i].transform);
        }

        SpawnExplosions(torpedoList);
    }

    public GameObject TorpedoManager()
    {
        // for as many objects as are in the pooledObjects list
        for (int i = 0; i < torpedoList.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!torpedoList[i].activeInHierarchy)
            {
                return torpedoList[i];
            }
        }
        // otherwise, return null   
        return null;
    }

    private void SpawnDepthCharges()
    {
        for (int i = 0; i < depthCount; i++)
        {
            // pooling depthCharges (for player)
            GameObject dc = (GameObject)Instantiate(depthCharge);

            // hides depthCharges when game starts
            dc.SetActive(false);
            
            // adds depthCharge to list
            depthChargeList.Add(dc);

            // sets the depthCharges as a child of the Player (Destroyer) object
            dc.transform.SetParent(GameObject.Find("Depth Charge Manager").transform);
        }

        SpawnExplosions(depthChargeList);
    }

    public GameObject DepthChargeManager()
    {
        // for as many objects as are in the pooledObjects list
        for (int i = 0; i < depthChargeList.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!depthChargeList[i].activeInHierarchy)
            {
                return depthChargeList[i];
            }
        }
        // otherwise, return null   
        return null;
    }

    private void SpawnExplosions(List<GameObject> incList)
    {
        for (int i = 0; i < incList.Count; i++)
        {
            // instantiate explosion object
            GameObject exp = (GameObject)Instantiate(explosion);

            // set explosion object to false
            exp.SetActive(false);

            // add explosion object to list
            explosionList.Add(exp);

            // sets explosion object as child of every object index in list
            exp.transform.SetParent(incList[i].transform);
        }
    }

    public GameObject ExplosionManager()
    {
        for (int i = 0; i < explosionList.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!explosionList[i].activeInHierarchy)
            {
                return explosionList[i];
            }
        }
        // otherwise, return null   
        return null;
    }
    */
}

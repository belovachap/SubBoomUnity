using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance; 

    [SerializeField] private GameObject submarine;
    private List<GameObject> submarineList = new();

    [SerializeField] private GameObject depthCharge;
    private List<GameObject> depthChargeList = new();

    private static int subCount = 10;
    private static int depthCount = 10;

    private float timeSinceSubAdded = 0.0f;

    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        SpawnSubmarines();
        SpawnDepthCharges();
    }

    private void Update()
    {
        SubmarineManager();
        DepthChargeManager();
    }

    public void SpawnSubmarines()
    {
        for (int i = 0; i < subCount; i++)
        {
            // pooling submarines
            GameObject sub = (GameObject)Instantiate(submarine);

            // hides the game object so that not all 10 spawn when the game starts
            sub.SetActive(false);
            submarineList.Add(sub);

            // adds the submarines as a child object of the Sub Boom game object
            sub.transform.SetParent(this.transform);
        }
    }

    public void SubmarineManager()
    {
        // add a new submarine if it's been at least 10 seconds
        timeSinceSubAdded += Time.deltaTime;

        if (timeSinceSubAdded >= 10)
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

    public void SpawnDepthCharges()
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
            dc.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

    public GameObject DepthChargeManager()
    {
        // For as many objects as are in the pooledObjects list
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
}

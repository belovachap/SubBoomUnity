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

    float timeSinceSubAdded = 0.0f;

    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        // list will be 10 indexes long
        for (int i = 0; i < 10; i++)
        {
            // pooling submarines
            GameObject sub = (GameObject) Instantiate(submarine);

            // hides the game object so that not all 10 spawn when the game starts
            sub.SetActive(false);
            submarineList.Add(sub);

            // adds the submarines as a child object of the Sub Boom game object
            sub.transform.SetParent(this.transform);


            // pooling depthCharges (for player)
            GameObject dc = (GameObject) Instantiate(depthCharge);
            dc.SetActive(false);
            depthChargeList.Add(dc);

            dc.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

    private void Update()
    {
        SubmarineManager();
        DepthChargeManager();
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

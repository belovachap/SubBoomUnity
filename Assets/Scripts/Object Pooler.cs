using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance; 
    public GameObject submarine;
    public List<GameObject> submarineList = new();

    float timeSinceSubAdded = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        SharedInstance = this;

        // list will be 10 indexes long
        for (int i = 0; i < 10; i++)
        {
            GameObject go = (GameObject) Instantiate(submarine);

            // hides the game object so that not all 10 spawn when the game starts
            go.SetActive(false);
            submarineList.Add(go);

            // adds the submarines as a child object of the Sub Boom game object
            go.transform.SetParent(this.transform);
        }
    }

    private void Update()
    {
        ObjectManager();
    }

    public void ObjectManager()
    {
        // Add a new submarine if it's been at least 10 seconds
        timeSinceSubAdded += Time.deltaTime;

        if (timeSinceSubAdded > 10)
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
}

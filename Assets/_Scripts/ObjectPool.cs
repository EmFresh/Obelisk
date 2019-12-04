using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reference: https://www.youtube.com/watch?v=tdSmKaJvCoA
public class ObjectPool : MonoBehaviour
{
    //Create class for simple pool
    //Dive into the class pool which inside class Objectpool
    [System.Serializable]
    public class Pool
    {
        //Name of the pool
        public string tag;
        //Type of 3D model in this pool
        public GameObject prefab;
        //Number of models contain in this pool
        public int size;
    }

    //Create Object Pool as a Singleton
    #region Singleton
    
    public static ObjectPool Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    #endregion

    //Create the list of pools which could manipulate inside Unity and assign information for pool dictionary
    public List<Pool> pools;

    //Create a dictionary to help save name of pool and the objects inside the pool
    public Dictionary<string, Queue<GameObject>> poolDict;

    // Start is called before the first frame update
    void Start()
    {
        //assign poolDict to a dictionaty that contain a name tag and a queue of game object
        poolDict = new Dictionary<string, Queue<GameObject>>();

        //call all pools in the list of pool
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //Use the information inside a pool to create a queue of objects in certain size and set them unactive at start
            for (int i = 0; i < pool.size; i++)
            {
                //Create a gameobject inside the game world
                GameObject ob = Instantiate(pool.prefab);
                //Set this gameobject unactive
                ob.SetActive(false);
                //Put new object at the end of queue
                objectPool.Enqueue(ob);
            }

            //Save the name tag of the pool and the queue of objects into dictionary 
            poolDict.Add(pool.tag, objectPool);
        }
    }   

    //using certain pool's name inside the pool dictionary to spawn object in certain position and rotation
    public GameObject SpawnObject(string tag, Vector3 pos, Quaternion rot, bool keepInQueue)
    {
        //Check if the tag exist in the dictionary
        if (!poolDict.ContainsKey(tag))
        {
            //If not, return and print error message
            Debug.LogWarning("Pool with tag " + tag + " does not exist!");
            return null;
        }

        //Check if the pool have object in it
        if (poolDict[tag].Count != 0)
        {
            //Take off the first object of pool's queue
            GameObject objectGoSpawn = poolDict[tag].Dequeue();

            //Set this object active and in certain position and rotation
            objectGoSpawn.SetActive(true);
            objectGoSpawn.transform.position = pos;
            objectGoSpawn.transform.rotation = rot;

            //Check if this obeject still need in pool
            if (keepInQueue)
            {
                //Put this object to the end of pool's  queue
                poolDict[tag].Enqueue(objectGoSpawn);
            }

            //Return this game obeject
            return objectGoSpawn;
        }
        else
        {
            //If not, return and print error message
            Debug.Log("Pool with tag " + tag + " is empty!");
            return null;
        }
    }

    //Collect the object back to the certain pool
    public void CollectObject(string tag, GameObject ourObject)
    {
        ourObject.SetActive(false);

        //Put this object to the end of pool's  queue
        poolDict[tag].Enqueue(ourObject);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject area;
    public GameObject spawnpoint;


   //trigger
   private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == area)
        {
            transform.position = spawnpoint.transform.position;
            transform.rotation = spawnpoint.transform.rotation;
        }
    }
}

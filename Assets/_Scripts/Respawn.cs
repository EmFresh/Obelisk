using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject area;
    public GameObject spawnpoint;
    [HideInInspector] public bool respawn;

    void Update()
    {
        if (respawn)
        {
            respawn = false;
            transform.position = spawnpoint.transform.position;
            transform.rotation = spawnpoint.transform.rotation;

            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    //trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == area)
        {
            transform.position = spawnpoint.transform.position;
            transform.rotation = spawnpoint.transform.rotation;

            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
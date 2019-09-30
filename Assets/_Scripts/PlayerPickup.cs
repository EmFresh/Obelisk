using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public static int WoodAmount = 0;
    public static int StoneAmount = 0;
    public static int CrystalAmount = 0;
    float timer = 0;
    float pickupDuration = 1f;

    bool resourceCollected = false;
    bool woodCollision = false;
    bool stoneCollision = false;
    bool crystalCollision = false;

    public KeyCode key;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            timer = Time.time;
        }
        //Picks up resource after 1 second of holding the key down (Key is public, it is et in Unity)
        //Also sends a bool to onTriggerStay
        if (Input.GetKey(key))
        {
            if (woodCollision == true)
            {

                if (Time.time - timer >= pickupDuration)
                {
                    Debug.Log("hi");

                    WoodAmount += 1;
                    resourceCollected = true;
                    woodCollision = false;
                    timer = 0;
                }
            }
            else if (stoneCollision == true)
            {

                if (Time.time - timer >= pickupDuration)
                {
                    Debug.Log("hi");

                    StoneAmount += 1;
                    resourceCollected = true;
                    stoneCollision = false;
                    timer = 0;
                }
            }
            else if (crystalCollision == true)
            {

                if (Time.time - timer >= pickupDuration)
                {
                    Debug.Log("hi");

                    CrystalAmount += 1;
                    resourceCollected = true;
                    crystalCollision = false;
                    timer = 0;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Sends collision bool to be used in update depending on resource
        if (other.gameObject.CompareTag("Resource(Wood)"))
        {
            //Debug.Log("hello");
            woodCollision = true;
        }
        if (other.gameObject.CompareTag("Resource(Stone)"))
        {
            //Debug.Log("hello");
            stoneCollision = true;
        }
        if (other.gameObject.CompareTag("Resource(Crystal)"))
        {
            //Debug.Log("hello");
            crystalCollision = true;
        }
        //else if(Input.GetKeyDown(key))
        // {
        //    if (other.gameObject.CompareTag ("Resource(Wood)"))
        //    {
        //        if (Time.time - timer > pickupDuration)
        //        {
        //             other.gameObject.SetActive (false);
        //             WoodAmount += 1;
        //        }
        //   }
        //}
        //if (other.gameObject.CompareTag ("Resource(Stone)"))
        // {
        //other.gameObject.SetActive (false);
        //StoneAmount += 1;
        //}
        //if (other.gameObject.CompareTag ("Resource(Crystal)"))
        //{
        //other.gameObject.SetActive (false);
        //CrystalAmount += 1;
        //}
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BuildZone"))
        {
            
        }
        //deletes resource object
        else if (resourceCollected == true && !other.gameObject.CompareTag("BuildZone"))
        {
            other.gameObject.SetActive(false);
            resourceCollected = false;
        }
    }
}

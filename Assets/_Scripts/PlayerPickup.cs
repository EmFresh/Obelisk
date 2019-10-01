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

    int resourceCollected = 0;
    bool woodCollision = false;
    bool stoneCollision = false;
    bool crystalCollision = false;
    bool woodSmallCollision = false;
    bool stoneSmallCollision = false;
    bool crystalSmallCollision = false;
    bool keyPressed = false;
    bool deletThis = false;

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
            keyPressed = true;
            timer = Time.time;
        }
        else if (Input.GetKeyUp(key))
        {
            keyPressed = false;
            resourceCollected = 0;
        }

        //Picks up resource after 1 second of holding the key down (Key is public, it is et in Unity)
        //Also sends a bool to onTriggerStay
        if (keyPressed)
        {
            if (woodCollision == true)

            {

                if (Time.time - timer >= pickupDuration)
                {
                    Debug.Log("hi");

                    WoodAmount += 3;
                    resourceCollected += 1;
                    woodCollision = false;
                    timer = 0;
                    deletThis = true;
                }
            }
            else if (stoneCollision == true)
            {

                if (Time.time - timer >= pickupDuration)
                {
                    Debug.Log("hi");

                    StoneAmount += 3;
                    resourceCollected += 1;
                    stoneCollision = false;
                    timer = 0;
                    deletThis = true;
                }
            }
            else if (crystalCollision == true)
            {

                if (Time.time - timer >= pickupDuration)
                {
                    Debug.Log("hi");

                    CrystalAmount += 3;
                    resourceCollected += 1;
                    crystalCollision = false;
                    timer = 0;
                    deletThis = true;
                }
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        //Sends collision bool to be used in update depending on resource

        if (!keyPressed)
        {
            Debug.Log("Y do we play gaem");
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
            if (other.gameObject.CompareTag("Resource(WoodSmall)"))
            {
                //Debug.Log("hello");
                //woodSmallCollision = true;
                other.gameObject.SetActive (false); 
                WoodAmount += 1;
            }
            if (other.gameObject.CompareTag("Resource(StoneSmall)"))
            {
                //Debug.Log("hello");
                //stoneSmallCollision = true;
                other.gameObject.SetActive (false); 
                StoneAmount += 1;
            }
            if (other.gameObject.CompareTag("Resource(CrystalSmall)"))
            {
                //Debug.Log("hello");
                //crystalSmallCollision = true;
                other.gameObject.SetActive (false); 
                CrystalAmount += 1; 
            }
           

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (deletThis)
        {
            //if()
            //{
            if (other.gameObject.CompareTag("BuildZone"))
            {

            }
            //deletes resource object
            else if (!other.gameObject.CompareTag("BuildZone"))
            {
                other.gameObject.SetActive(false);
                Debug.Log("Y do we play gaem2");
                deletThis = false;
            }
            //}
        }
    }
}

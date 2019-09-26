using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MaxSpeed = 1;
    public float JumpHeight = 7;
    public static int WoodAmount = 0;
    public static int StoneAmount = 0;
    public static int CrystalAmount = 0;
    public bool isGrounded;
    float timer = 0;
    float pickupDuration = 1f;

    bool resourceCollected = false;
    bool woodCollision = false;
    bool stoneCollision = false;
    bool crystalCollision = false;

    public KeyCode key;

    Rigidbody rb;
    BoxCollider col_size;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col_size = GetComponent<BoxCollider>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        transform.position += (Vector3.forward * MaxSpeed) * y * Time.deltaTime;
        transform.position += (Vector3.right * MaxSpeed) * x * Time.deltaTime;

        if(Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(0, JumpHeight, 0);
            isGrounded = false;
        }

        if(Input.GetKeyDown(key))
        {
            timer = Time.time;
        }
        //Picks up resource after 1 second of holding the key down (Key is public, it is et in Unity)
        //Also sends a bool to onTriggerStay
        if(Input.GetKey(key))
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

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        
    }
    void OnTriggerEnter(Collider other) 
    {
        //Sends collision bool to be used in update depending on resource
        if (other.gameObject.CompareTag ("Resource(Wood)"))
        {
            //Debug.Log("hello");
            woodCollision = true;
        }
        if (other.gameObject.CompareTag ("Resource(Stone)"))
        {
            //Debug.Log("hello");
            stoneCollision = true;
        }
        if (other.gameObject.CompareTag ("Resource(Crystal)"))
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
        //deletes resource object
        if(resourceCollected == true)
            {
                other.gameObject.SetActive (false);
                resourceCollected = false;
            }
    }
}
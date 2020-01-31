using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPickup : MonoBehaviour
{
    // Start is called before the first frame update
    //Rigidbody rb;
    bool speedBoost = false;
    float speedtimer = 5;
    float timer = 0;
    float speedbuff=4;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float maxtmp = 0;
        if (Time.time - timer > speedtimer)
        {
            speedBoost = false;
        }
        if (speedBoost)
        {

            GetComponent<PlayerMovement>().speed *= speedbuff;
        }
        else
        {
            GetComponent<PlayerMovement>().speed = GetComponent<PlayerMovement>().MaxSpeed;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBuff"))
        {
            other.gameObject.SetActive(false);
            speedBoost = true;
            timer = Time.time;
        }
    }
}

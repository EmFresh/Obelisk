using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDeposit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag ("BuildZone"))
        {
            //Debug.Log("hello");
             PlayerPickup.WoodAmount = 0;
             PlayerPickup.StoneAmount = 0;
             PlayerPickup.CrystalAmount = 0;
        }
    }
}

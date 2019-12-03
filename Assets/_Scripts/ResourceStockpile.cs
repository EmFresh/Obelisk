using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStockpile : MonoBehaviour
{
    // Start is called before the first frame update

    int woodStock = 0;
    int stoneStock = 0;
    int crystalStock = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider obj)
    {
        if ((obj.gameObject.CompareTag ("Player 2"))) //TODO: Fix this
        {
            woodStock += obj.gameObject.GetComponent<PlayerPickup>().WoodAmount;
            stoneStock += obj.gameObject.GetComponent<PlayerPickup>().StoneAmount;
            crystalStock += obj.gameObject.GetComponent<PlayerPickup>().CrystalAmount;

            obj.gameObject.GetComponent<PlayerPickup>().WoodAmount = 0;
            obj.gameObject.GetComponent<PlayerPickup>().StoneAmount = 0;
            obj.gameObject.GetComponent<PlayerPickup>().CrystalAmount = 0;

            Debug.Log(woodStock);
            Debug.Log(stoneStock);
            Debug.Log(crystalStock);
        }

        if (obj.gameObject.CompareTag ("Player 2"))
        {
            woodStock += obj.gameObject.GetComponent<PlayerPickup>().WoodAmount;
            stoneStock += obj.gameObject.GetComponent<PlayerPickup>().StoneAmount;
            crystalStock += obj.gameObject.GetComponent<PlayerPickup>().CrystalAmount;

            obj.gameObject.GetComponent<PlayerPickup>().WoodAmount = 0;
            obj.gameObject.GetComponent<PlayerPickup>().StoneAmount = 0;
            obj.gameObject.GetComponent<PlayerPickup>().CrystalAmount = 0;

            Debug.Log(woodStock);
            Debug.Log(stoneStock);
            Debug.Log(crystalStock);
        }
    }
}

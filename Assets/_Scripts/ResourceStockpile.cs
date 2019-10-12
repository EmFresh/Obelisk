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
        if (obj.gameObject.CompareTag ("Player"))
        {
            woodStock += PlayerPickup.WoodAmount;
            stoneStock += PlayerPickup.StoneAmount;
            crystalStock += PlayerPickup.CrystalAmount;

            PlayerPickup.WoodAmount = 0;
            PlayerPickup.StoneAmount = 0;
            PlayerPickup.CrystalAmount = 0;

            Debug.Log(woodStock);
            Debug.Log(stoneStock);
            Debug.Log(crystalStock);
        }
    }
}

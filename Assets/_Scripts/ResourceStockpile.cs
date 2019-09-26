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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            woodStock = PlayerMovement.WoodAmount;
            stoneStock = PlayerMovement.StoneAmount;
            crystalStock = PlayerMovement.CrystalAmount;
            Debug.Log(woodStock);
            Debug.Log(stoneStock);
            Debug.Log(crystalStock);
        }
    }
}

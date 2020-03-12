using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestResources : MonoBehaviour
{
    // Start is called before the first frame update

    public int woodStock = 0;
    public int stoneStock = 0;
    public int crystalStock = 0;

    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(player.tag))
        {
            woodStock += player.GetComponent<PlayerPickup>().WoodAmount;
            stoneStock += player.GetComponent<PlayerPickup>().StoneAmount;
            crystalStock += player.GetComponent<PlayerPickup>().CrystalAmount;

            player.GetComponent<PlayerPickup>().WoodAmount = 0;
            player.GetComponent<PlayerPickup>().StoneAmount = 0;
            player.GetComponent<PlayerPickup>().CrystalAmount = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour
{
    private Text woodText;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        woodText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
       
        woodText.text = "Wood: " + player.GetComponent<PlayerPickup>().WoodAmount + "\nStone: " + player.GetComponent<PlayerPickup>().StoneAmount + "\nCrystal: " + player.GetComponent<PlayerPickup>().CrystalAmount;
    }
}

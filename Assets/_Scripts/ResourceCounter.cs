using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour
{
    private Text woodText;
    // Start is called before the first frame update
    void Start()
    {
        woodText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
       
        woodText.text = "Wood: " + PlayerPickup.WoodAmount + "\nStone: " + PlayerPickup.StoneAmount + "\nCrystal: " + PlayerPickup.CrystalAmount;
    }
}

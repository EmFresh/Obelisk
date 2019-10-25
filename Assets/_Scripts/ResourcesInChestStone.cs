using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesInChestStone : MonoBehaviour
{
    // Start is called before the first frame update
    
    int woodStock = 0;
    int stoneStock = 0;
    int crystalStock = 0;

    private Text stoneText;
    void Start()
    {
         stoneText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        stoneText.text = "Stone: " + PlayerPickup.stoneStock;
    }


}

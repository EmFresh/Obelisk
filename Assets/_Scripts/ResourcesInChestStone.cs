using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesInChestStone : MonoBehaviour
{
    // Start is called before the first frame update


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

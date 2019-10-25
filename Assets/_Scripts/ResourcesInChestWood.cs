using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ResourcesInChestWood : MonoBehaviour
{
    // Start is called before the first frame update
    
    int woodStock = 0;
    int stoneStock = 0;
    int crystalStock = 0;

    private Text woodText;
    void Start()
    {
         woodText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        woodText.text = "Wood: " + PlayerPickup.woodStock;
    }
}

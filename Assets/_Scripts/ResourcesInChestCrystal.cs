using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesInChestCrystal : MonoBehaviour
{
    // Start is called before the first frame update
int woodStock = 0;
    int stoneStock = 0;
    int crystalStock = 0;

    private Text crystalText;
    void Start()
    {
         crystalText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        crystalText.text = "Crystal: " + PlayerPickup.crystalStock;
    }

}

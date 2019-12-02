using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesInChestCrystal : MonoBehaviour
{
    // Start is called before the first frame update
 
    public GameObject player;
    private Text crystalText;
    void Start()
    {
        crystalText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        crystalText.text = "Crystal: " + player.GetComponent<PlayerPickup>().crystalStock;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ResourcesInChestWood : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject player;
    private Text woodText;
    void Start()
    {
         woodText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        woodText.text = "Wood: " + player.GetComponent<PlayerPickup>().woodStock;
    }
}

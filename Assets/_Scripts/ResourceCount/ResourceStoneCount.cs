using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStoneCount : MonoBehaviour
{
    private Text stoneText;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        stoneText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        stoneText.text = "" + player.GetComponent<PlayerPickup>().StoneAmount;
    }
}

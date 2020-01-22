using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceWoodCount : MonoBehaviour
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
        woodText.text = "" + player.GetComponent<PlayerPickup>().WoodAmount;
    }
}

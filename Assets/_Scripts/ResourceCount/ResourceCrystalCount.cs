using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCrystalCount : MonoBehaviour
{
    private Text crystalText;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        crystalText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        crystalText.text = "" + player.GetComponent<PlayerPickup>().CrystalAmount;
    }
}

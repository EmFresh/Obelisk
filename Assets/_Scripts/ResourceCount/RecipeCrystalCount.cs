using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCrystalCount : MonoBehaviour
{
    public GameObject player;
    private Text crystalText;
    public GameObject buildSite;
    // Start is called before the first frame update
    void Start()
    {
        crystalText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int crystalInstock = player.GetComponent<PlayerPickup>().crystalStock;
        int crystalNeed = buildSite.GetComponent<TowerBuild>().crystalNeeded;
        crystalText.text = crystalInstock + "/" + crystalNeed;
    }
}

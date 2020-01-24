using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCrystalCount : MonoBehaviour
{
    public GameObject player;
    private Text crystalText;
    public GameObject buildSite;
    public GameObject chest;
    // Start is called before the first frame update
    void Start()
    {
        crystalText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int crystalInstock = chest.GetComponent<ChestResources>().crystalStock;
        int crystalNeed = buildSite.GetComponent<TowerBuild>().crystalNeeded;
        crystalText.text = crystalInstock + "/" + crystalNeed;
    }
}

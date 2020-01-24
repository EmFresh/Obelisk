using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeStoneCount : MonoBehaviour
{
    public GameObject player;
    private Text stoneText;
    public GameObject buildSite;
    public GameObject chest;
    // Start is called before the first frame update
    void Start()
    {
        stoneText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int stoneInstock = chest.GetComponent<ChestResources>().stoneStock;
        int stoneNeed = buildSite.GetComponent<TowerBuild>().stoneNeeded;
        stoneText.text = stoneInstock + "/" + stoneNeed;
    }
}

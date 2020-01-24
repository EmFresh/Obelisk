using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeWoodCount : MonoBehaviour
{
    public GameObject player;
    public GameObject chest;
    private Text woodText;
    public GameObject buildSite;
    // Start is called before the first frame update
    void Start()
    {
        woodText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int woodInstock = chest.GetComponent<ChestResources>().woodStock;
        int woodNeed = buildSite.GetComponent<TowerBuild>().woodNeeded;
        woodText.text = woodInstock + "/" + woodNeed;
    }
}

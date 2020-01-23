using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeStageCount : MonoBehaviour
{
    public GameObject buildSite;
    private Text stageText;
    // Start is called before the first frame update
    void Start()
    {
        stageText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int stageNum = buildSite.GetComponent<TowerBuild>().stage + 1;
        stageText.text = "Stage\n" + stageNum;
    }
}

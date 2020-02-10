using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeStageImage : MonoBehaviour
{
    public GameObject buildSite;
    public Image mask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int stageNum = buildSite.GetComponent<TowerBuild>().stage;
        if (stageNum == 0)
        {
            mask.fillAmount = 0.2f;
        }
        if (stageNum == 1)
        {
            mask.fillAmount = 0.4f;
        }
        if (stageNum == 2)
        {
            mask.fillAmount = 0.56f;
        }
        if (stageNum == 3)
        {
            mask.fillAmount = 0.8f;
        }
    }
}

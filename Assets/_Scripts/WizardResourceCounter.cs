using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardResourceCounter : MonoBehaviour
{
    // Start is called before the first frame update
    private Text resourceText;
    // Start is called before the first frame update
    void Start()
    {
        resourceText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
       
        resourceText.text = "Wood: " + WizardResourceManager.wizardWoodAmount + "\nStone: " + WizardResourceManager.wizardStoneAmount + "\nCrystal: " + WizardResourceManager.wizardCrystalAmount;
    }
}

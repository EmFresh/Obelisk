﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControllerInput;
using static ControllerInput.CONTROLLER_BUTTON;

public class TowerBuild : MonoBehaviour
{

    public List<GameObject> towerParts = new List<GameObject>();
    public KeyCode buildKey = KeyCode.Tab;
    public CONTROLLER_BUTTON buildJoy = Y;

    public static int stage;

    public int woodNeeded;
    public int stoneNeeded;
    public int crystalNeeded;


    private ushort playerIndex;
    private Transform theParent;
    private const int maxStages = 3;

    void OnTriggerStay(Collider obj)
    {
        playerIndex = obj.gameObject.GetComponent<PlayerMovement>().playerIndex;
        
        if ((obj.gameObject.CompareTag("Player 1") && this.gameObject.tag.Contains("Build Zone")) )
            if (Input.GetKeyDown(buildKey) || isButtonDown(playerIndex, (int)buildJoy))
            {
                if (stage < maxStages)
                {
                    if (WizardResourceManager.wizardWoodAmount >= woodNeeded && WizardResourceManager.wizardStoneAmount >= stoneNeeded && WizardResourceManager.wizardCrystalAmount >= crystalNeeded)
                    {
                        //get the next Parent
                        theParent = gameObject.transform;
                        if(theParent.childCount != 3)
                        while (theParent.childCount > 0) theParent = theParent.GetChild(theParent.childCount-1);


                        GameObject towerPart = Instantiate(towerParts[stage % towerParts.Count]);

                        //sets the tower part as the child of the previous object
                        towerPart.transform.parent = theParent;
                        towerPart.transform.position = theParent.position;

                        //get the size of the two objects
                        Bounds obj1 = theParent.gameObject.GetComponent<MeshRenderer>().bounds;
                        Bounds obj2 = towerPart.GetComponent<MeshRenderer>().bounds;
                        Vector3 size1 = obj1.max - obj2.min, size2 = obj2.max - obj2.min;

                        //place the new object on top of the old one
                        towerPart.transform.position += new Vector3(0, (size1.y + size2.y) * 0.5f, 0);

                        //increase the build stage
                        WizardResourceManager.wizardWoodAmount -= woodNeeded;
                        WizardResourceManager.wizardStoneAmount -= stoneNeeded;
                        WizardResourceManager.wizardCrystalAmount -= crystalNeeded;

                        stage++;
                    }
                }
            }

    }

}

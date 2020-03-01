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
    public float spacing = 3;
    [HideInInspector] public int stage;

    public int woodNeeded;
    public int stoneNeeded;
    public int crystalNeeded;

    public Animator _animator;

    private ushort playerIndex;
    private Transform theParent;
    private const int maxStages = 3;

    public GameObject chest;

    //private bool initBuild = true;
    void OnTriggerStay(Collider obj)
    {
        if (obj.gameObject.tag.ToLower().Contains("player"))
            playerIndex = obj.gameObject.GetComponent<PlayerMovement>().playerIndex;

        _animator.SetBool("isBuild", false);

        if ((obj.gameObject.CompareTag("Player 1") && this.gameObject.tag.Contains("Build Zone 1")) || (obj.gameObject.CompareTag("Player 2") && this.gameObject.tag.Contains("Build Zone 2")))
            if (Input.GetKeyDown(buildKey) || isButtonDown(playerIndex, (int)buildJoy))
            {
                if (stage < maxStages)
                {
                    if (chest.GetComponent<ChestResources>().woodStock >= woodNeeded && chest.GetComponent<ChestResources>().stoneStock >= stoneNeeded && chest.GetComponent<ChestResources>().crystalStock >= crystalNeeded)
                    {
                        //play animation
                        _animator.SetBool("isBuild", true);

                        //get the next Parent
                        theParent = gameObject.transform.GetChild(0);

                        //while (theParent.childCount > 0) theParent = theParent.GetChild(0);
                        GameObject towerPart = Instantiate(towerParts[stage % towerParts.Count], theParent);
                        towerPart.transform.localScale = new Vector3(1, 1, 1);
                        towerPart.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
                        //sets the tower part as the child of the previous object
                        //towerPart.transform.parent = theParent;
                        towerPart.transform.position = theParent.position;
                        //if (theParent.childCount > 0)
                        //    theParent = theParent.GetChild(theParent.childCount - 1);
                        //get the size of the two objects
                        Bounds obj1 = theParent.gameObject.GetComponent<MeshRenderer>().bounds;
                        Bounds obj2 = towerPart.GetComponent<MeshRenderer>().bounds;
                        Vector3 size1 = obj1.max - obj2.min, size2 = obj2.max - obj2.min;


                        //place the new object on top of the old one
                        towerPart.transform.position += new Vector3(0, obj1.size.y * 1.5f, 0);

                        for (int a = 0; a < theParent.childCount - 1; a++)
                        {
                            obj1 = theParent.GetChild(a).gameObject.GetComponent<MeshRenderer>().bounds;
                            //place the new object on top of the old one
                            towerPart.transform.position += new Vector3(0, obj1.size.y + spacing, 0);
                        }


                        //Rob / Lilian's solution IDK
                        //towerParts2[stage % towerParts.Count].SetActive(true);

                        //initBuild = false;
                        //increase the build stage
                        chest.GetComponent<ChestResources>().woodStock -= woodNeeded;
                        chest.GetComponent<ChestResources>().stoneStock -= stoneNeeded;
                        chest.GetComponent<ChestResources>().crystalStock -= crystalNeeded;
                        var tmp = towerPart.AddComponent<ObeliskRotation>();

                        tmp.right = (stage % 2) != 0;
                        tmp.speed = stage * 5 + 10;

                        stage++;
                    }
                }
            }

    }

}

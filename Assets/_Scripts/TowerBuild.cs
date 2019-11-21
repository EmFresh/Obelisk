using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerBuild : MonoBehaviour
{

    public List<GameObject> towerParts = new List<GameObject>();
    public KeyCode buildKey;

    private Transform theParent;

    int maxStages = 3;
    public static int stage;

    public int woodNeeded;
    public int stoneNeeded;
    public int crystalNeeded;
    // Start is called before the first frame update
    void Start()
    {
        print("init started");
        AudioPlayer.init();
        print("init finished");
        //  theParent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        AudioPlayer.update();
    }

    void onExit()
    {
        AudioPlayer.stopAll();
        AudioPlayer.disable();
    }

    void OnTriggerStay(Collider obj)
    {
        if ((obj.gameObject.CompareTag("Build Zone 1") && this.gameObject.CompareTag("Player 1")) || (obj.gameObject.CompareTag("Build Zone 2") && this.gameObject.CompareTag("Player 3")))
            if (Input.GetKeyDown(buildKey))
            {
                if (stage < maxStages)
                {
                    if (WizardResourceManager.wizardWoodAmount >= woodNeeded && WizardResourceManager.wizardStoneAmount >= stoneNeeded && WizardResourceManager.wizardCrystalAmount >= crystalNeeded)
                    {
                        //get the next Parent
                        theParent = obj.gameObject.transform;
                        while (theParent.childCount > 0) theParent = theParent.GetChild(0);


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

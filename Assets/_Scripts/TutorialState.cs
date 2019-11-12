using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TutorialState : MonoBehaviour
{
    const string DLL_NAME = "MetricsLogger";

    [DllImport(DLL_NAME)]
    private static extern void WriteStateToText(float time, int num, bool hasNotWritten);

    // Start is called before the first frame update
    public enum tutorialState
    {
        learnMovementStage1,
        learnMovementStage2,
        learnPickupSmall,
        learnPickupLarge,
        learnSorting,
        //Switches to the spellcaster here
        learnPickupFromChest,
        learnTowerBuild
    }

    public GameObject player1;
    public GameObject player2;

    public GameObject UI1;
    public GameObject UI2;
    public GameObject UI3;
    public GameObject UI4;
    public GameObject UI5;
    public GameObject UI6;
    public GameObject UI7;
    public GameObject UI8;

    public tutorialState state;
    private static ObjectPool myPool;

    GameObject[] fob;
    ///var fob : GameObject[];
    void Start()
    {
        state = tutorialState.learnMovementStage1;
        myPool = ObjectPool.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == tutorialState.learnMovementStage1)
        {
            WriteStateToText(Time.time,1, true);

            //UI: This is a tower building and movement tutorial
            //UI: Use the mouse to look around, use WASD to move, try moving over to that big beam of light!
            if (Waypoint.touchedWaypoint)
            {
                state = tutorialState.learnMovementStage2;
                //UI: Great, now walk to the next one!
            }

        }
        if (state == tutorialState.learnMovementStage2)
        {
             WriteStateToText(Time.time,2,true);
            if (Waypoint.touchedWaypoint2)
            {

                //start pawaning small resources
                myPool.SpawnObject("Resource(WoodSmall)", new Vector3(-3, 0.5f, -60), transform.rotation);
                myPool.SpawnObject("Resource(StoneSmall)", new Vector3(0, 0.5f, -60), transform.rotation);
                myPool.SpawnObject("Resource(CrystalSmall)", new Vector3(3, 0.5f, -60), transform.rotation);
                UI1.SetActive(false);
                UI2.SetActive(true);
                //Debug.Log("Spawn Wood");
                state = tutorialState.learnPickupSmall;
            }
        }
        if (state == tutorialState.learnPickupSmall)
        {
            WriteStateToText(Time.time,3,true);
            if (PlayerPickup.StoneAmount >= 1 && PlayerPickup.WoodAmount >= 1 && PlayerPickup.CrystalAmount >= 1)
            {
                //delete small resources and start spawning large resources
                myPool.SpawnObject("Resource(Wood)", new Vector3(-3, 0.5f, -70), transform.rotation);
                myPool.SpawnObject("Resource(Stone)", new Vector3(0, 0.5f, -70), transform.rotation);
                myPool.SpawnObject("Resource(Crystal)", new Vector3(3, 0.5f, -70), transform.rotation);
                UI2.SetActive(false);
                UI3.SetActive(true);
                state = tutorialState.learnPickupLarge;
            }
        }
        if (state == tutorialState.learnPickupLarge)
        {
            WriteStateToText(Time.time,4,true);
            if (PlayerPickup.StoneAmount >= 3 && PlayerPickup.WoodAmount >= 3 && PlayerPickup.CrystalAmount >= 3)
            {
                //delete small resources and start spawning large resources
                UI3.SetActive(false);
                UI4.SetActive(true);
                state = tutorialState.learnSorting;
            }
        }
        if (state == tutorialState.learnSorting)
        {
            WriteStateToText(Time.time,5,true);
            if (PlayerPickup.stoneStock >= 1 && PlayerPickup.woodStock >= 1 && PlayerPickup.crystalStock >= 1)
            {
                //[SerializeField]
                //player2.setactive(false);
                //player1.setactive(true);
                //Test.Equals(this).player1.setactive(false);
                player1.SetActive(true);
                player2.SetActive(false);
                state = tutorialState.learnPickupFromChest;
                UI5.SetActive(true);
               

                MinimapScript._pivot.transform.position = player1.transform.position;
                MinimapScript._pivot.transform.rotation = player1.transform.rotation;
                MinimapScript._pivot.transform.parent = player1.transform;
            }
        }
        if (state == tutorialState.learnPickupFromChest)
        {
            WriteStateToText(Time.time,6,true);

            if (WizardResourceManager.wizardWoodAmount >= 1 && WizardResourceManager.wizardStoneAmount >= 1 && WizardResourceManager.wizardCrystalAmount >= 1)
            {
                UI5.SetActive(false);
                UI6.SetActive(true);
                state = tutorialState.learnTowerBuild;
            }
        }
        if (state == tutorialState.learnTowerBuild)
        {
            WriteStateToText(Time.time,7,true);
            if (TowerBuild.stage >= 1)
            {
                
                //end tutorial
            }
        }
    }
}

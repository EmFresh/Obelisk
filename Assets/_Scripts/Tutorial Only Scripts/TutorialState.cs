using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

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
        learnShooting,
        learnPickupFromChest,
        learnTowerBuild,
        endGame
    }

    bool endEnabled = false;
    public GameObject player1Wizard;
    public GameObject player2Wizard;
    public GameObject player1Rogue;
    public GameObject player2Rogue;

    public GameObject UI1;
    public GameObject UI2;
    public GameObject UI3;
    public GameObject UI4;
    public GameObject UI5;
    public GameObject UI6;
    public GameObject UI7;
    public GameObject UI8;
    public GameObject UI12;
    public GameObject UI22;
    public GameObject UI32;
    public GameObject UI42;
    public GameObject UI52;
    public GameObject UI62;
    public GameObject UI72;
    public GameObject UI82;
    public GameObject OverUI;
    public GameObject OverUI2;

    public tutorialState state;
    public tutorialState stateP2;
    private static ObjectPool myPool;

    public KeyCode key;

    public GameObject buildSite;

    public GameObject site2;
    GameObject[] fob;
    ///var fob : GameObject[];
    void Start()
    {
        state = tutorialState.learnMovementStage1;
        stateP2 = tutorialState.learnMovementStage1;
        WriteStateToText(Time.time, 1, true);
        myPool = ObjectPool.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == tutorialState.learnMovementStage1)
        {
            //UI: This is a tower building and movement tutorial
            //UI: Use the mouse to look around, use WASD to move, try moving over to that big beam of light!
            if (Waypoint.touchedWaypoint)
            {
                UI1.SetActive(false);
                UI2.SetActive(true);
                state = tutorialState.learnMovementStage2;
                WriteStateToText(Time.time, 2, true);
                //UI: Great, now walk to the next one!
            }

        }
        if (state == tutorialState.learnMovementStage2)
        {

            if (Waypoint.touchedWaypoint2)
            {

                //start pawaning small resources
                myPool.SpawnObject("Resource(WoodSmall)", new Vector3(-263, 0.5f, -14), transform.rotation);
                myPool.SpawnObject("Resource(StoneSmall)", new Vector3(-260, 0.5f, -14), transform.rotation);
                myPool.SpawnObject("Resource(CrystalSmall)", new Vector3(-257, 0.5f, -14), transform.rotation);
                UI2.SetActive(false);
                UI3.SetActive(true);
                WriteStateToText(Time.time, 3, true);

                //Debug.Log("Spawn Wood");

                state = tutorialState.learnPickupSmall;
            }
        }
        if (state == tutorialState.learnPickupSmall)
        {

            if (player1Rogue.GetComponent<PlayerPickup>().StoneAmount >= 1 && player1Rogue.GetComponent<PlayerPickup>().WoodAmount >= 1 && player1Rogue.GetComponent<PlayerPickup>().CrystalAmount >= 1)
            {
                //delete small resources and start spawning large resources
                myPool.SpawnObject("Resource(Wood)", new Vector3(-264, 0.5f, -23), transform.rotation);
                myPool.SpawnObject("Resource(Stone)", new Vector3(-260, 0.5f, -23), transform.rotation);
                myPool.SpawnObject("Resource(Crystal)", new Vector3(-256, 0.5f, -23), transform.rotation);
                UI3.SetActive(false);
                UI4.SetActive(true);
                WriteStateToText(Time.time, 4, true);
                state = tutorialState.learnPickupLarge;
            }
        }
        if (state == tutorialState.learnPickupLarge)
        {

            if (player1Rogue.GetComponent<PlayerPickup>().StoneAmount >= 3 && player1Rogue.GetComponent<PlayerPickup>().WoodAmount >= 3 && player1Rogue.GetComponent<PlayerPickup>().CrystalAmount >= 3)
            {
                UI4.SetActive(false);
                UI5.SetActive(true);
                WriteStateToText(Time.time, 5, true);
                state = tutorialState.learnSorting;
            }
        }
        if (state == tutorialState.learnSorting)
        {

            if (player1Rogue.GetComponent<PlayerPickup>().stoneStock >= 1 && player1Rogue.GetComponent<PlayerPickup>().woodStock >= 1 && player1Rogue.GetComponent<PlayerPickup>().crystalStock >= 1)
            {
                //[SerializeField]
                //player2.setactive(false);
                //player1.setactive(true);
                //Test.Equals(this).player1.setactive(false);
                player1Wizard.SetActive(true);
                player1Rogue.SetActive(false);

                UI5.SetActive(false);
                UI6.SetActive(true);

                //MinimapScript._pivot.transform.position = player1Wizard.transform.position;
                //MinimapScript._pivot.transform.rotation = player1Wizard.transform.rotation;
                //MinimapScript._pivot.transform.parent = player1Wizard.transform;

                myPool.SpawnObject("ScaryBois", new Vector3(-256, 0.6f, -23), transform.rotation);
                myPool.SpawnObject("ScaryBois", new Vector3(-260, 0.6f, -23), transform.rotation);
                myPool.SpawnObject("ScaryBois", new Vector3(-264, 0.6f, -23), transform.rotation);
                WriteStateToText(Time.time, 6, true);

                state = tutorialState.learnShooting;



            }
        }
        if (state == tutorialState.learnShooting)
        {
            Debug.Log(FireballCollision.scarecrowsDown);
            if (FireballCollision.scarecrowsDown >= 3)
            {
                UI6.SetActive(false);
                UI7.SetActive(true);
                WriteStateToText(Time.time, 7, true);

                state = tutorialState.learnPickupFromChest;
            }
        }
        if (state == tutorialState.learnPickupFromChest)
        {


            if (player1Wizard.GetComponent<WizardResourceManager>().wizardWoodAmount >= 1 && player1Wizard.GetComponent<WizardResourceManager>().wizardStoneAmount >= 1 && player1Wizard.GetComponent<WizardResourceManager>().wizardCrystalAmount >= 1)
            {
                UI7.SetActive(false);
                UI8.SetActive(true);
                WriteStateToText(Time.time, 8, true);
                state = tutorialState.learnTowerBuild;
            }
        }
        if (state == tutorialState.learnTowerBuild)
        {

            if (buildSite.GetComponent<TowerBuild>().stage >= 1)
            {
                UI8.SetActive(false);
                OverUI.SetActive(true);
                if (!endEnabled)
                {
                    WriteStateToText(Time.time, 9, true);
                    //endEnabled = true;
                }

                state = tutorialState.endGame;
            }
        }


        if (stateP2 == tutorialState.learnMovementStage1)
        {

            if (Waypoint.P2touchedWaypoint)
            {
                UI12.SetActive(false);
                UI22.SetActive(true);
                stateP2 = tutorialState.learnMovementStage2;
                WriteStateToText(Time.time, 2, true);

            }

        }
        if (stateP2 == tutorialState.learnMovementStage2)
        {

            if (Waypoint.P2touchedWaypoint2)
            {

                //start pawaning small resources
                myPool.SpawnObject("Resource(WoodSmall)", new Vector3(-88, 0.5f, -14), transform.rotation);
                myPool.SpawnObject("Resource(StoneSmall)", new Vector3(-84, 0.5f, -14), transform.rotation);
                myPool.SpawnObject("Resource(CrystalSmall)", new Vector3(-80, 0.5f, -14), transform.rotation);
                UI22.SetActive(false);
                UI32.SetActive(true);
                WriteStateToText(Time.time, 3, true);

                //Debug.Log("Spawn Wood");

                stateP2 = tutorialState.learnPickupSmall;
            }
        }
        if (stateP2 == tutorialState.learnPickupSmall)
        {

            if (player2Rogue.GetComponent<PlayerPickup>().StoneAmount >= 1 && player2Rogue.GetComponent<PlayerPickup>().WoodAmount >= 1 && player2Rogue.GetComponent<PlayerPickup>().CrystalAmount >= 1)
            {
                //delete small resources and start spawning large resources
                myPool.SpawnObject("Resource(Wood)", new Vector3(-88, 0.5f, -23), transform.rotation);
                myPool.SpawnObject("Resource(Stone)", new Vector3(-84, 0.5f, -23), transform.rotation);
                myPool.SpawnObject("Resource(Crystal)", new Vector3(-80, 0.5f, -23), transform.rotation);
                UI32.SetActive(false);
                UI42.SetActive(true);
                WriteStateToText(Time.time, 4, true);
                stateP2 = tutorialState.learnPickupLarge;
            }
        }
        if (stateP2 == tutorialState.learnPickupLarge)
        {

            if (player2Rogue.GetComponent<PlayerPickup>().StoneAmount >= 3 && player2Rogue.GetComponent<PlayerPickup>().WoodAmount >= 3 && player2Rogue.GetComponent<PlayerPickup>().CrystalAmount >= 3)
            {
                UI42.SetActive(false);
                UI52.SetActive(true);
                WriteStateToText(Time.time, 5, true);
                Debug.Log("printAThingyYe");
                stateP2 = tutorialState.learnSorting;
            }
        }
        if (stateP2 == tutorialState.learnSorting)
        {

            if (player2Rogue.GetComponent<PlayerPickup>().stoneStock >= 1 && player2Rogue.GetComponent<PlayerPickup>().woodStock >= 1 && player2Rogue.GetComponent<PlayerPickup>().crystalStock >= 1)
            {
                //[SerializeField]
                //player2.setactive(false);
                //player1.setactive(true);
                //Test.Equals(this).player1.setactive(false);
                player2Wizard.SetActive(true);
                player2Rogue.SetActive(false);

                UI52.SetActive(false);
                UI62.SetActive(true);

                //MinimapScript._pivot.transform.position = player1Wizard.transform.position;
                //MinimapScript._pivot.transform.rotation = player1Wizard.transform.rotation;
                //MinimapScript._pivot.transform.parent = player1Wizard.transform;

                myPool.SpawnObject("ScaryBois", new Vector3(-89, 0.6f, -23), transform.rotation);
                myPool.SpawnObject("ScaryBois", new Vector3(-85, 0.6f, -23), transform.rotation);
                myPool.SpawnObject("ScaryBois", new Vector3(-81, 0.6f, -23), transform.rotation);
                WriteStateToText(Time.time, 6, true);

                stateP2 = tutorialState.learnShooting;



            }
        }
        if (stateP2 == tutorialState.learnShooting)
        {
            Debug.Log(FireballCollision.scarecrowsDown);
            Debug.Log(stateP2);
            if (FireballCollision.scarecrowsDown >= 3)
            {
                UI62.SetActive(false);
                UI72.SetActive(true);
                WriteStateToText(Time.time, 7, true);

                stateP2 = tutorialState.learnPickupFromChest;
            }
        }
        if (stateP2 == tutorialState.learnPickupFromChest)
        {


            if (player2Wizard.GetComponent<WizardResourceManager>().wizardWoodAmount >= 1 && player2Wizard.GetComponent<WizardResourceManager>().wizardStoneAmount >= 1 && player2Wizard.GetComponent<WizardResourceManager>().wizardCrystalAmount >= 1)
            {
                UI72.SetActive(false);
                UI82.SetActive(true);
                WriteStateToText(Time.time, 8, true);
                stateP2 = tutorialState.learnTowerBuild;
            }
        }
        if (stateP2 == tutorialState.learnTowerBuild)
        {

            if (site2.GetComponent<TowerBuild>().stage >= 1)
            {
                UI82.SetActive(false);
                OverUI2.SetActive(true);
                if (!endEnabled)
                {
                    WriteStateToText(Time.time, 9, true);
                    //endEnabled = true;
                }

                stateP2 = tutorialState.endGame;
            }
        }
        if (state == tutorialState.endGame && stateP2 == tutorialState.endGame)
        {
            if (Input.GetKeyDown(key))
            {
                SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            }
        }
    }
}


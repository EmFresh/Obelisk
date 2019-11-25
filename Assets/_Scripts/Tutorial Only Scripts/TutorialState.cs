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
    public GameObject OverUI;

    public tutorialState state;
    private static ObjectPool myPool;

    public KeyCode key;

    GameObject[] fob;
    ///var fob : GameObject[];
    void Start()
    {
        state = tutorialState.learnMovementStage1;
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
                UI7.SetActive(true);
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
                UI7.SetActive(false);
                UI2.SetActive(true);
                WriteStateToText(Time.time, 3, true);

                //Debug.Log("Spawn Wood");

                state = tutorialState.learnPickupSmall;
            }
        }
        if (state == tutorialState.learnPickupSmall)
        {

            if (PlayerPickup.StoneAmount >= 1 && PlayerPickup.WoodAmount >= 1 && PlayerPickup.CrystalAmount >= 1)
            {
                //delete small resources and start spawning large resources
                myPool.SpawnObject("Resource(Wood)", new Vector3(-264, 0.5f, -23), transform.rotation);
                myPool.SpawnObject("Resource(Stone)", new Vector3(-260, 0.5f, -23), transform.rotation);
                myPool.SpawnObject("Resource(Crystal)", new Vector3(-256, 0.5f, -23), transform.rotation);
                UI2.SetActive(false);
                UI3.SetActive(true);
                WriteStateToText(Time.time, 4, true);
                state = tutorialState.learnPickupLarge;
            }
        }
        if (state == tutorialState.learnPickupLarge)
        {

            if (PlayerPickup.StoneAmount >= 3 && PlayerPickup.WoodAmount >= 3 && PlayerPickup.CrystalAmount >= 3)
            {
                UI3.SetActive(false);
                UI4.SetActive(true);
                WriteStateToText(Time.time, 5, true);
                state = tutorialState.learnSorting;
            }
        }
        if (state == tutorialState.learnSorting)
        {

            if (PlayerPickup.stoneStock >= 1 && PlayerPickup.woodStock >= 1 && PlayerPickup.crystalStock >= 1)
            {
                //[SerializeField]
                //player2.setactive(false);
                //player1.setactive(true);
                //Test.Equals(this).player1.setactive(false);
                player1.SetActive(true);
                player2.SetActive(false);

                UI4.SetActive(false);
                UI8.SetActive(true);

                MinimapScript._pivot.transform.position = player1.transform.position;
                MinimapScript._pivot.transform.rotation = player1.transform.rotation;
                MinimapScript._pivot.transform.parent = player1.transform;

                myPool.SpawnObject("ScaryBois", new Vector3(-256, 0.5f, -23), transform.rotation);
                myPool.SpawnObject("ScaryBois", new Vector3(-260, 0.5f, -23), transform.rotation);
                myPool.SpawnObject("ScaryBois", new Vector3(-264, 0.5f, -23), transform.rotation);
                WriteStateToText(Time.time, 6, true);

                state = tutorialState.learnShooting;



            }
        }
        if (state == tutorialState.learnShooting)
        {
            if (FireballCollision.scarecrowsDown >= 3)
            {
                UI8.SetActive(false);
                UI5.SetActive(true);
                WriteStateToText(Time.time, 7, true);

                state = tutorialState.learnPickupFromChest;
            }
        }
        if (state == tutorialState.learnPickupFromChest)
        {


            if (WizardResourceManager.wizardWoodAmount >= 1 && WizardResourceManager.wizardStoneAmount >= 1 && WizardResourceManager.wizardCrystalAmount >= 1)
            {
                UI5.SetActive(false);
                UI6.SetActive(true);
                WriteStateToText(Time.time, 8, true);
                state = tutorialState.learnTowerBuild;
            }
        }
        if (state == tutorialState.learnTowerBuild)
        {

            if (TowerBuild.stage >= 1)
            {
                UI6.SetActive(false);
                OverUI.SetActive(true);
                if(!endEnabled)
                {
                    WriteStateToText(Time.time, 9, true);
                    endEnabled = true;
                }
                
                state = tutorialState.endGame;
            }
        }
        if (state == tutorialState.endGame)
        {
            if (Input.GetKeyDown(key))
            {
                SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            }
        }
    }
}

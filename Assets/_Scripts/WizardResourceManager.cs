using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControllerInput;
using static ControllerInput.CONTROLLER_BUTTON;

public class WizardResourceManager : MonoBehaviour
{

    //int woodAmount = 0;
    public static int wizardWoodAmount = 0;

    //int stoneAmount = 0;
    public static int wizardStoneAmount = 0;
    //int crystalAmount = 0;
    public static int wizardCrystalAmount = 0;
    bool woodChestCollision = false;
    bool stoneChestCollision = false;
    bool crystalChestCollision = false;

    public KeyCode pickupKey = KeyCode.E;
    public CONTROLLER_BUTTON pickupJoy = X;

    private ushort playerIndex;


    // Update is called once per frame
    void Update()
    {
        playerIndex = GetComponent<PlayerMovement>().playerIndex;
        //if E pressed while colliding with chests then take resources from chest
        if (Input.GetKeyDown(pickupKey) || isButtonDown(playerIndex, (int)pickupJoy))
        {
            if (woodChestCollision)
            {
                if (PlayerPickup.woodStock > 0)
                {
                    PlayerPickup.woodStock -= 1;
                    wizardWoodAmount += 1;
                }
                else
                {
                    Debug.Log("No Wood In Chest");
                }
            }
            if (stoneChestCollision)
            {
                if (PlayerPickup.stoneStock > 0)
                {
                    PlayerPickup.stoneStock -= 1;
                    wizardStoneAmount += 1;
                }
                else
                {
                    Debug.Log("No Stone In Chest");
                }
            }
            if (crystalChestCollision)
            {
                if (PlayerPickup.crystalStock > 0)
                {
                    PlayerPickup.crystalStock -= 1;
                    wizardCrystalAmount += 1;
                }
                else
                {
                    Debug.Log("No Crystal In Chest");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //If collides with chests
        if (other.gameObject.CompareTag("Chest(Wood)"))
        {
            woodChestCollision = true;
        }
        if (other.gameObject.CompareTag("Chest(Stone)"))
        {
            stoneChestCollision = true;
        }
        if (other.gameObject.CompareTag("Chest(Crystal)"))
        {
            crystalChestCollision = true;
        }

    }
}

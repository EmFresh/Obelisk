using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    private bool exitPlayer = false;
    void OnTriggerEnter(Collider ent)
    {
        if (exitPlayer)
        {
            Destroy(this.gameObject);
            print("deleat this garbage");
        }
    }

    void OnTriggerExit(Collider ent)
    {
        exitPlayer = true;
        print("Exited the player!!!");
    }
}

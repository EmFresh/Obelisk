using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    private bool exitPlayer = false;
    public static int scarecrowsDown = 0;
    void OnTriggerEnter(Collider ent)
    {
        if(ent.gameObject.tag.Contains("crow"))
        {
            scarecrowsDown += 1;
            //Debug.Log(scarecrowsDown);
            ent.gameObject.SetActive(false);
        }
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

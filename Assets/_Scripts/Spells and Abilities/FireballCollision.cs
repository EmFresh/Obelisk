using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    private bool exitPlayer = true;
    private GameObject explosion;
    public static int scarecrowsDown = 0;

    private void Awake()
    {

    }
    void OnTriggerEnter(Collider ent)
    {
        if (ent.gameObject.tag.Contains("crow"))
        {
            scarecrowsDown += 1;
            //Debug.Log(scarecrowsDown);
            ent.gameObject.SetActive(false);
        }
        if (exitPlayer)
        {

            explosion = (GameObject)Resources.Load("_Prefabs/fireballImpact");
            explosion.transform.position = this.transform.position;

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

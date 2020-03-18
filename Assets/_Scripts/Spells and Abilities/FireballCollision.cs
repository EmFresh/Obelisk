using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    [HideInInspector] public GameObject caster;
    public static int scarecrowsDown = 0;

    void OnTriggerEnter(Collider ent)
    {
        if (ent.isTrigger || ent.gameObject == caster)return;

        if (ent.gameObject.tag.Contains("crow"))
        {
            scarecrowsDown += 1;
            //Debug.Log(scarecrowsDown);
            ent.gameObject.SetActive(false);
        }
       

            //   explosion = (GameObject)Resources.Load("_Prefabs/fireballImpact");
            //   explosion.transform.position = this.transform.position;

            Destroy(this.gameObject);
            print("deleat this garbage");
        
    }
}
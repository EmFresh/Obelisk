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

        if (ent.tag.ToLower().Contains("player"))
        {
            var player = ent.GetComponent<PlayerMovement>();
            player.currentHealth -= player.currentHealth > 0 ? 1 : 0;

            if ( player.currentHealth == 0)
            {
                ent.GetComponent<Respawn>().respawn = true;
                player.currentHealth = ent.GetComponent<PlayerMovement>().healthAmount;
            }
        }

        //   explosion = (GameObject)Resources.Load("_Prefabs/fireballImpact");
        //   explosion.transform.position = this.transform.position;

        Destroy(this.gameObject);
        print("delete this garbage");

    }
}
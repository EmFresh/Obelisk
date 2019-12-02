using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//referenced https://www.youtube.com/watch?v=28JTTXqMvOU
public class MinimapScript : MonoBehaviour
{
    public Transform player;
    public Transform pivot;

    void Start()
    {
        pivot.transform.position = player.transform.position;
        pivot.transform.parent = player.transform;
    }

    void LateUpdate()
    {
        Vector3 newPos = pivot.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(90f, pivot.eulerAngles.y, 0f);
    }


}

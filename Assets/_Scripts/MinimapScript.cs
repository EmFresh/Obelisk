using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//referenced https://www.youtube.com/watch?v=28JTTXqMvOU
public class MinimapScript : MonoBehaviour
{
    public Transform player;
    public Transform pivot;
    public static Transform _pivot;

    void Start()
    {
        _pivot = pivot;
        _pivot.transform.position = player.transform.position;
        _pivot.transform.parent = player.transform;
    }

    void LateUpdate()
    {
        Vector3 newPos = _pivot.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(90f, _pivot.eulerAngles.y, 0f);
    }


}

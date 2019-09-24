using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{

    public float speed;
    Vector3 rot = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxis("HorizontalL"), 0, Input.GetAxis("VerticalL"));
         rot += new Vector3(Input.GetAxis("HorizontalR"), 0, Input.GetAxis("VerticalR"));
        transform.position += dir * speed;
        transform.rotation = Quaternion.Euler(-rot.z, rot.x, 0);

    }
}

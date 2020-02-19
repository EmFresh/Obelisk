using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskRotation : MonoBehaviour
{
    public bool right = false;
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), (right ? 1 : -1) * speed * Time.deltaTime);
    }
}

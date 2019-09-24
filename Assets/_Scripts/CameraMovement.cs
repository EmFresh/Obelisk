using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    public float MaxSpeed = 1;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        move(x, y);

        Vector3 newPos = PlayerTransform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    }
    private void move(float x, float y)
    {
        transform.position += (Vector3.forward * MaxSpeed) * y * Time.deltaTime;
        transform.position += (Vector3.right * MaxSpeed) * x * Time.deltaTime;
    }
}

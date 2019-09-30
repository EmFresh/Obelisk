using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    public Transform pivot;
    public float RotateSpeed = 1;

    public float _cameraLROffset = 0.5f;
    public float _cameraUDOffset = 0.6f;
    private Vector3 _cameraBFOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraBFOffset = transform.position - PlayerTransform.position;

        pivot.transform.position = PlayerTransform.transform.position;
        pivot.transform.parent = PlayerTransform.transform;

        Cursor.lockState = CursorLockMode.Locked;

        transform.position += transform.right * _cameraLROffset;
        transform.position += transform.up * _cameraUDOffset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position -= transform.right * _cameraLROffset;
        transform.position -= transform.up * _cameraUDOffset;

        float horizontal = Input.GetAxis("Mouse X") * RotateSpeed;
        PlayerTransform.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Mouse Y") * RotateSpeed;
        pivot.Rotate(-vertical, 0, 0);

        float desiredYAngle = PlayerTransform.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = Vector3.Slerp(transform.position, PlayerTransform.position + (rotation * _cameraBFOffset), SmoothFactor);
        

        if(transform.position.y < PlayerTransform.position.y)
        {
            transform.position = new Vector3(transform.position.x, PlayerTransform.position.y, transform.position.z);
        }

        transform.LookAt(PlayerTransform);

        transform.position += transform.right * _cameraLROffset;
        transform.position += transform.up * _cameraUDOffset;
    }
}

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
        //Assign Camera distence offset
        _cameraBFOffset = transform.position - PlayerTransform.position;

        //Make pivot as a child to player and move to player's position
        pivot.transform.position = PlayerTransform.transform.position;
        pivot.transform.parent = PlayerTransform.transform;

        //Make you cursor dispear when play game
        Cursor.lockState = CursorLockMode.Locked;

        //Assign moveup and move right offset to camera
        transform.position += transform.right * _cameraLROffset;
        transform.position += transform.up * _cameraUDOffset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Remove moveup and move right offset to help calculation
        transform.position -= transform.right * _cameraLROffset;
        transform.position -= transform.up * _cameraUDOffset;

        //Get X position of the mouse and rotate the player
        float horizontal = Input.GetAxis("Mouse X") * RotateSpeed;
        PlayerTransform.Rotate(0, horizontal, 0);

        //Get X position of the mouse and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * RotateSpeed;
        pivot.Rotate(-vertical, 0, 0);

        //Move the camera based on current ratation of player and the sidtance offset, slerp help movement smoother
        float desiredYAngle = PlayerTransform.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = Vector3.Slerp(transform.position, PlayerTransform.position + (rotation * _cameraBFOffset), SmoothFactor);
        
        //Make sure camera not going below ground
        if(transform.position.y < PlayerTransform.position.y)
        {
            transform.position = new Vector3(transform.position.x, PlayerTransform.position.y, transform.position.z);
        }

        //Rotate camera to player's location
        transform.LookAt(PlayerTransform);

        //Assign moveup and move right offset to camera
        transform.position += transform.right * _cameraLROffset;
        transform.position += transform.up * _cameraUDOffset;
    }
}

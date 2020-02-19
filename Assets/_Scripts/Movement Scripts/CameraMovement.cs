using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControllerInput;

public class CameraMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    public Transform pivot;

    public bool invertYJoy = false;
    public bool invertYKeyboard = true;
    [Range(0, 5.0f)]
    public float RotateSpeed = 1;

    public float _cameraLROffset = 0.5f;
    public float _cameraUDOffset = 0.6f;

    ushort playerIndex;

    float minYAngle = -45.0f;
    float maxYAngle = 45.0f;

    float yRot = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
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
        playerIndex = transform.parent.GetComponent<PlayerMovement>().playerIndex;

        //Remove moveup and move right offset to help calculation
        transform.position -= transform.right * _cameraLROffset;
        transform.position -= transform.up * _cameraUDOffset;

        var stick = getSticks(playerIndex)[RS];
        var keyEnabled = GetComponentInParent<PlayerMovement>().enableKeyboard;
        //Get X position of the mouse and rotate the player
        float horizontal = keyEnabled ? (Input.GetAxisRaw("Mouse X") + Mathf.Clamp(stick.x, -1, 1)) * RotateSpeed : 0;
        PlayerTransform.Rotate(0, horizontal, 0);

        //Assign camera to be a child of pivot to help it rotate with pivot
        transform.parent = pivot.transform;

        //Get X position of the mouse and rotate the pivot
        float vertical = keyEnabled ? ((invertYKeyboard ? -1 : 1) * Input.GetAxisRaw("Mouse Y") + (invertYJoy ? -1 : 1) * Mathf.Clamp(stick.y, -1, 1)) * RotateSpeed : 0;
        yRot += vertical;

        //Make sure camera not going below ground or above head
        yRot = Mathf.Clamp(yRot, minYAngle, maxYAngle);
        pivot.eulerAngles = new Vector3(yRot, pivot.eulerAngles.y, 0.0f);

        //Assign camera to be a child of player
        transform.parent = PlayerTransform.transform;

        //Rotate camera to player's location
        transform.LookAt(PlayerTransform);

        //Assign moveup and move right offset to camera
        transform.position += transform.right * _cameraLROffset;
        transform.position += transform.up * _cameraUDOffset;
    }
}

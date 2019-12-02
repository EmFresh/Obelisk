using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControllerInput;
using static ControllerInput.CONTROLLER_BUTTON;


public class PlayerMovement : MonoBehaviour
{
    public ushort playerIndex;

    public CONTROLLER_BUTTON jumpJoy = A;
    public Animator _animator;
    public static float MaxSpeed = 5;
    public float JumpHeight = 7;
    public bool isGrounded;

    private Vector3 moveDirection;

    Rigidbody rb;
    BoxCollider col_size;

    // Start is called before the first frame update
    void Start()
    {

        //Assign player's phisics body and collider
        rb = GetComponent<Rigidbody>();
        col_size = GetComponent<BoxCollider>();

        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the X and Y position of any input (laptop or controller)
        float x = 0;
        float y = 0;

        Stick stick = getSticks(playerIndex)[LS];

        x = Mathf.Clamp(stick.x, -1, 1);
        y = Mathf.Clamp(stick.y, -1, 1);

        print(getStickDeadZone(playerIndex));

        moveDirection = transform.forward * y + transform.right * x;
        moveDirection = moveDirection.normalized;
        transform.position += moveDirection * MaxSpeed * Time.deltaTime;

        //Set running animation base on input runnning direction
        if (_animator)
        {
            _animator.SetFloat("VelX", x);
            _animator.SetFloat("VelY", y);
        }

        //If player is on the ground make player jump
        if ((Input.GetKey(KeyCode.Space) ||
        isButtonDown(playerIndex, (int)jumpJoy)) &&
        isGrounded == true)
        {
            rb.AddForce(0, JumpHeight, 0);
            isGrounded = false;

            //Set jump animation to true
            if(_animator)
            _animator.SetBool("isJump", true);
        }

    }

    // Check player on the ground or not (Unity build in function)
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;

        //Set jump animation to false
        if (_animator)
        _animator.SetBool("isJump", false);
    }


}
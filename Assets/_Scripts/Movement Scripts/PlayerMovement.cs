﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControllerInput;
using static ControllerInput.CONTROLLER_BUTTON;
using static Networking;

public class PlayerMovement : MonoBehaviour
{
    public Animator _animator;

    public ushort playerIndex;

    public CONTROLLER_BUTTON jumpJoy = A;

    public ParticleSystem jumpParticles;
    public ParticleSystem landParticles;

    bool hasJumped = false;

    public bool enableKeyboard = false;

 
    public int healthAmount = 5;
    [HideInInspector] public float currentHealth = 0;

    [Tooltip("MUST be set before you run the editor")] public float MaxSpeed = 15;

    public float speed = 0;

    [Tooltip("Value added to speed per update")] public float speedTime = 0.5f; 

    public float JumpHeight = 7;

    [HideInInspector] public bool isGrounded;

    private Vector3 moveDirection;

    private Rigidbody rb;
    private BoxCollider col_size;
    [HideInInspector] public Vector3 lastPosition;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public float dt;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        currentHealth = healthAmount;
        //Assign player's phisics body and collider
        rb = GetComponent<Rigidbody>();
        col_size = GetComponent<BoxCollider>();
        isGrounded = true;
        //print("The Error:"+getLastError());
    }

    // Update is called once per frame
    void Update()
    {
        Stick stick = getSticks(playerIndex)[LS];

        //Get the X and Y position of any input (laptop or controller)
        float x = (enableKeyboard ? Input.GetAxis("Horizontal") : 0) + Mathf.Clamp(stick.x, -1, 1);
        float y = (enableKeyboard ? Input.GetAxis("Vertical") : 0) + Mathf.Clamp(stick.y, -1, 1);

        moveDirection = transform.forward * y + transform.right * x;
        moveDirection = moveDirection.normalized;
        lastPosition = transform.position;
        transform.position += moveDirection * speed * Time.deltaTime;
        velocity = transform.position - lastPosition;
        dt = Time.deltaTime;

        // maxSpeed per character. Slower max speed for spellcaster, higher for Rogue

        //Rogue movement envelope is snappy and responsive.
        if (speed > MaxSpeed)
            speed = MaxSpeed;

        if (moveDirection.magnitude == 0)
        {
            if (speed > 0)
                speed -= 0.25f;
        }

        speed += moveDirection.magnitude * speedTime;

        transform.position += 1 * moveDirection * speed * Time.deltaTime;


        //Spellcaster isn't.



        //Set running animation base on input runnning direction
        if (_animator)
        {
            _animator.SetFloat("VelX", x);
            _animator.SetFloat("VelY", y);
        }

        //If player is on the ground make player jump
        if ((isButtonDown(playerIndex, (int)jumpJoy) || Input.GetKey(KeyCode.Space)) &&
            isGrounded == true)
        {
            rb.AddForce(0, JumpHeight, 0);
            CreateJumpParticles(gameObject.transform.position);
            hasJumped = true;
            isGrounded = false;

            //Set jump animation to true
            if (_animator)
                _animator.SetBool("isJump", true);
        }

    }

    // Check player on the ground or not (Unity build in function)
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        if (hasJumped)
        {
            CreateLandParticles(gameObject.transform.position);
            hasJumped = false;
        }


        //Set jump animation to false
        if (_animator)
            _animator.SetBool("isJump", false);
    }


    void CreateJumpParticles(Vector3 playerPos)
    {
       
        Instantiate(jumpParticles,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
        jumpParticles.Play();
    }


    void CreateLandParticles(Vector3 playerPos)
    {
      
        Instantiate(landParticles,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
        landParticles.Play();
    }

}
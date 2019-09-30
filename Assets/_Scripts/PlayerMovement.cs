using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MaxSpeed = 1;
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
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        //Move player to that direction, forward is allways where player look at
        moveDirection = transform.forward * y + transform.right * x;
        moveDirection = moveDirection.normalized;
        transform.position += moveDirection * MaxSpeed * Time.deltaTime;
       
        //If player is on the ground make player jump
        if(Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(0, JumpHeight, 0);
            isGrounded = false;
        }
    }

    // Check player on the ground or not (Unity build in function)
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        
    }
}
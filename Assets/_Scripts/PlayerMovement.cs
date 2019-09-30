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
        rb = GetComponent<Rigidbody>();
        col_size = GetComponent<BoxCollider>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        moveDirection = transform.forward * y + transform.right * x;
        moveDirection = moveDirection.normalized;
        transform.position += moveDirection * MaxSpeed * Time.deltaTime;
       
        if(Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(0, JumpHeight, 0);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        
    }
}
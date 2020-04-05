using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControllerInput;
using static ControllerInput.CONTROLLER_BUTTON;
using static Networking;
using static NetworkControl;
using System.Runtime.InteropServices;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    public bool isNetworkedPlayer = false;
    public ushort networkID;
    public ushort controllerIndex;

    public CONTROLLER_BUTTON jumpJoy = A;

    public bool enableKeyboard;

    [Tooltip("MUST be set before you run the editor")] public float MaxSpeed = 15;

    public float JumpHeight = 7;
    public float minMovement = 0.1f;

    [HideInInspector] public float speed = 0;

    [HideInInspector] public bool isGrounded;

    private Vector3 moveDirection;

    private Rigidbody rb;
    private BoxCollider col_size;
    [HideInInspector] public Vector3 lastPosition;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float dt;
    static float startTime;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        startTime = Time.realtimeSinceStartup;
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = MaxSpeed;

        //Assign player's phisics body and collider
        rb = GetComponent<Rigidbody>();
        col_size = GetComponent<BoxCollider>();
        isGrounded = true;

        //print("The Error:"+getLastError());
    }

    // Update is called once per frame
    void Update()
    {

        if (!isNetworkedPlayer)
        {
            Stick stick = getSticks(controllerIndex)[LS];

            //Get the X and Y position of any input (laptop or controller)
            float x = (enableKeyboard ? Input.GetAxis("Horizontal") : 0) + Mathf.Clamp(stick.x, -1, 1);
            float y = (enableKeyboard ? Input.GetAxis("Vertical") : 0) + Mathf.Clamp(stick.y, -1, 1);

            moveDirection = transform.forward * y + transform.right * x;
            moveDirection = moveDirection.normalized;

            lastPosition = transform.position;
            transform.position += moveDirection * speed * Time.deltaTime;
            direction = transform.position - lastPosition;
            dt = Time.deltaTime;

            //Set running animation base on input runnning direction
            if (animator)
            {
                animator.SetFloat("VelX", x);
                animator.SetFloat("VelY", y);
            }

            //If player is on the ground make player jump
            if ((isButtonDown(controllerIndex, (int)jumpJoy) || Input.GetKey(KeyCode.Space)) &&
                isGrounded == true)
            {
                rb.AddForce(0, JumpHeight, 0);
                isGrounded = false;

                //Set jump animation to true
                if (animator)
                    animator.SetBool("isJump", true);
            }

        }
        else
        {
            //TODO: move player based on networked positions (testing)

            var move = NetworkControl.movements[networkID];
            if (move.isUpdated)
            {
                transform.position = move.pos;
                move.isUpdated = false;
            }
            else //this can
                transform.position += divideVec(1, (move.dir * move.dt)) * Time.deltaTime;
        }
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        if (!isNetworkedPlayer)
        {
            //TODO: send information tp server
            Movement movement = new Movement();
            movement.pos = transform.position;
            movement.dir = direction;
            movement.rot = transform.rotation;
            movement.dt = dt;
            movement.id = networkID;
            movement.isUpdated = false;
            sendToPacket(sock, movement, ip);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                var timeInSec = Time.realtimeSinceStartup - startTime;
                string end = "GameEnd ";
                end += timeInSec.ToString() + " " ;
                end += NetworkControl.thisUser._name;

                /// end = "EndGame 10.5 The Winner!!" 
                sendToPacket(sock, end, ip);
             
                
            }
        }
    }
    Vector3 divideVec(float scale, Vector3 vec)
    {
        float x, y, z;

        x = scale / vec.x;
        y = scale / vec.y;
        z = scale / vec.z;

        return new Vector3(x, y, z);
    }
    // Check player on the ground or not (Unity build in function)
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;

        //Set jump animation to false
        if (animator)
            animator.SetBool("isJump", false);
    }

}
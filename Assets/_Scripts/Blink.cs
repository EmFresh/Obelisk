using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{

    private PlayerMovement playerMovement;
    private MeshRenderer characterRenderer;
    private float nextBlinkTime = 0;
    private float cooldownTime = 2.0f;
    private float blinkActiveTime = 5;
    // Start is called before the first frame update

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

    }
    void Start()
    {
        characterRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Blink") && Time.time > nextBlinkTime)
        {
            nextBlinkTime = Time.time + cooldownTime;
            Debug.Log("Blinking");
            characterRenderer.enabled = !characterRenderer.enabled;
            playerMovement.MaxSpeed = 10;
        }
    }
}


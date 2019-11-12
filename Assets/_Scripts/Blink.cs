using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public Animator _animator;

    private PlayerMovement playerMovement;
    private BlinkEffect shaderScript;
    [SerializeField]
    //private MeshRenderer characterRenderer;
    private float cooldownTime = 2.0f;
    private float nextBlinkTime = 0;

    public float blinkTime; //Set here or in Inspector to modify the amount of time the player is in "Blink" mode
    private bool isBlinking;

    public KeyCode key;
    // Start is called before the first frame update

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        shaderScript = GetComponentInChildren<BlinkEffect>();

    }
    void Start()
    {
        //characterRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && Time.time > nextBlinkTime && !isBlinking)
        {

            ActivateBlink();

        }
    }

    void ActivateBlink()
    {
        _animator.SetBool("isBlink", true);
        //playerMovement.MaxSpeed = 10;
        PlayerMovement.MaxSpeed = 10;
        shaderScript.enabled = !shaderScript.enabled;
        isBlinking = true;
       // characterRenderer.enabled = !characterRenderer.enabled;
        Debug.Log("Blink Active");
        Invoke("StopBlink", blinkTime); //After blinkTime seconds, StopBlink()
    }

    void StopBlink()
    {
         _animator.SetBool("isBlink", false);
        Debug.Log("Blink Stopped");
        //playerMovement.MaxSpeed = 5;
        PlayerMovement.MaxSpeed = 5;
        shaderScript.enabled = !shaderScript.enabled;

        //characterRenderer.enabled = !characterRenderer.enabled;
        nextBlinkTime = Time.time + cooldownTime;
        isBlinking = false;
    }
}


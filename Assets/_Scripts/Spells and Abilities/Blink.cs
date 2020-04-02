using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControllerInput;
using static ControllerInput.CONTROLLER_BUTTON;


public class Blink : MonoBehaviour
{
    public Animator _animator;
    public float blinkTime; //Set here or in Inspector to modify the amount of time the player is in "Blink" mode
    public bool isBlinking;
    public KeyCode blinkKey;
    public CONTROLLER_BUTTON blinkJoy = RB;

    public float endBlinkTime = 0;
    public float nextBlinkTime = 0;
    public float cooldownTime = 2.0f;

    private ushort playerIndex;
    private PlayerMovement playerMovement;
    private BlinkEffect shaderScript;
    //[SerializeField]
    //private MeshRenderer characterRenderer;


    // Start is called before the first frame update

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        shaderScript = GetComponentInChildren<BlinkEffect>();
    }


    // Update is called once per frame
    void Update()
    {
        playerIndex = GetComponent<PlayerMovement>().controllerIndex;

        if(!GetComponent<PlayerMovement>().isNetworkedPlayer)
        if ((Input.GetKeyDown(blinkKey) || isButtonDown(playerIndex, (int)blinkJoy)) && Time.time > nextBlinkTime && !isBlinking)
        {
            ActivateBlink();
        }
    }

    void ActivateBlink()
    {
        _animator.SetBool("isBlink", true);
        //playerMovement.MaxSpeed = 10;
        GetComponent<PlayerMovement>().speed = GetComponent<PlayerMovement>().MaxSpeed*3;
        shaderScript.enabled = !shaderScript.enabled;
        isBlinking = true;
        // characterRenderer.enabled = !characterRenderer.enabled;
        Debug.Log("Blink Active");
        endBlinkTime = Time.time + blinkTime;
        Invoke("StopBlink", blinkTime); //After blinkTime seconds, StopBlink()
    }

    void StopBlink()
    {
        _animator.SetBool("isBlink", false);
        Debug.Log("Blink Stopped");
        //playerMovement.MaxSpeed = 5;
        GetComponent<PlayerMovement>().speed = GetComponent<PlayerMovement>().MaxSpeed;
        shaderScript.enabled = !shaderScript.enabled;

        //characterRenderer.enabled = !characterRenderer.enabled;
        nextBlinkTime = Time.time + cooldownTime;
        isBlinking = false;
    }
}


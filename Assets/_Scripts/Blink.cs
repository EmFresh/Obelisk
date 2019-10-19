using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{

    private MeshRenderer characterRenderer;
    // Start is called before the first frame update
    void Start()
    {
        characterRenderer = GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Blink"))
        {
            Debug.Log("Blink Detected");
            characterRenderer.enabled=!characterRenderer.enabled;
        }
        
    }
}

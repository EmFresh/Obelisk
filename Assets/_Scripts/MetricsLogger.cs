using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MetricsLogger : MonoBehaviour
{
  
    const string DLL_NAME = "MetricsLogger";

    [DllImport(DLL_NAME)]
    private static extern int TestFunction();
    
    [DllImport(DLL_NAME)]
    private static extern void WriteInputToText();


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(TestFunction());
        }

        
    }

    
}

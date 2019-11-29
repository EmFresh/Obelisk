using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkBarScript : MonoBehaviour
{
    [Range(0, 1)]
    public float percentBlink = 1f;
    private Material rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = Instantiate(GetComponent<RawImage>().material);
        GetComponent<RawImage>().material = rend;
    }

    // Update is called once per frame
    void Update()
    {
        //percent -= 0.001f;
        rend.SetFloat("blink", percentBlink = Mathf.Clamp(percentBlink, 0, 1));
    }
}
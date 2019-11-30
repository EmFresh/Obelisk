using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkBarScript : MonoBehaviour
{
    [Range(0, 1)]
    public float percentBlink = 1f;
    private Material rend;
    private Blink blink;
    // Start is called before the first frame update
    void Start()
    {
        rend = Instantiate(GetComponent<RawImage>().material);
        GetComponent<RawImage>().material = rend;

        blink = transform.parent.parent.parent.gameObject.GetComponent<Blink>();
    }

    // Update is called once per frame
    void Update()
    {
        //percent -= 0.001f;
        if (blink.isBlinking)
        {
            rend.SetFloat("blink", percentBlink = 1 - Mathf.Clamp((Time.time - blink.endBlinkTime+blink.blinkTime)/blink.blinkTime, 0, 1));
        }
        else
        {
            if (blink.blinkTime != 0)
                rend.SetFloat("blink", (percentBlink = Mathf.Clamp((Time.time-blink.nextBlinkTime+blink.cooldownTime) / blink.cooldownTime, 0, 1)));
        }
    }
}
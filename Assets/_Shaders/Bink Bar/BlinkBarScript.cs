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
        Color lightGrey = new Color(.8f, .8f, .8f, 1);
        //percent -= 0.001f;
        if (blink.isBlinking)
        {
            rend.SetFloat("blink", percentBlink = 1 - Mathf.Clamp((Time.time - blink.endBlinkTime + blink.blinkTime) / blink.blinkTime, 0, 1));
            GetComponent<RawImage>().color = lightGrey;
        }
        else
        {
            if (blink.blinkTime != 0)
                rend.SetFloat("blink", (percentBlink = Mathf.Clamp((Time.time - blink.nextBlinkTime + blink.cooldownTime) / blink.cooldownTime, 0, 1)));

            if (percentBlink == 1)
                GetComponent<RawImage>().color = LerpColour(lightGrey, Color.white, Mathf.Cos(Time.time * 5) * .5f + .5f);
        }
    }

    Color LerpColour(Color a, Color b, float c)
    {
        return (1 - c) * a + c * b;
    }
}
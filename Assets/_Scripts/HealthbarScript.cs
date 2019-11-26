using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    [Range(0, 1)]
    public float percent;
    private Material rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<RawImage>().material;
        rend.EnableKeyword("percent");
    }

    // Update is called once per frame
    void Update()
    {
        
        
        rend.SetFloat("health", percent);
        
    }
}

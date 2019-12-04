using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlinkEffect : MonoBehaviour
{
    //public float intensity; 
    public Material material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
        Graphics.Blit(source,destination,material);

    }
   
}

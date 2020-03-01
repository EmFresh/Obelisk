using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
public class DynamicAudio : MonoBehaviour
{
    public string source = "vca:/Forest";
    [Range(0, 5)] public float speed = 1.0f;
    bool onIsland = false;
    [Range(0, 1)] public float maxVol = 1;
    float vol = 0;

    void Start()
    {
        FMODUnity.RuntimeManager.GetVCA(source).setVolume(vol);
    }
    void Update()
    {

        vol += speed * maxVol * Time.deltaTime * (onIsland ? 1 : -1);

        FMODUnity.RuntimeManager.GetVCA(source).setVolume(vol = Mathf.Clamp(vol, 0, maxVol));

    }
    void OnTriggerEnter(Collider other) => onIsland = other.tag.Contains("Player") ? true : onIsland;


    void OnTriggerExit(Collider other) => onIsland = other.tag.Contains("Player") ? false : onIsland;
}

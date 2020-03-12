using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class ScaleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [FMODUnity.EventRef] public string Event;
    EventInstance instance;
    EventDescription eventDescription;

    bool mouseOver = false;
    bool mouseOverExit = false;
    Vector3 minScale;
    Vector3 currentScale;
    public Vector3 targetScale;
    public float targetScaleAfter;
    private Vector3 maxScale;

    //[FMODUnity.EventRef]
    //public string hover = "";
    //FMOD.Studio.EventInstance hoverEvent;
    //[Range(0, 1)] public float maxVol = 1;
    //float vol = 0;
    private void Start()
    {
        maxScale = targetScale * targetScaleAfter;
       eventDescription = RuntimeManager.GetEventDescription(Event);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        mouseOverExit = false;
        eventDescription.createInstance(out instance);
        instance.start();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        mouseOverExit = true;
    }

    void Update()
    {
        if (mouseOver)
        {

            minScale = transform.localScale;
            //hoverEvent.start();
            transform.localScale = Vector2.Lerp(minScale, maxScale, .25f);
        }

        if (mouseOverExit)
        {
            currentScale = transform.localScale;
            transform.localScale = Vector2.Lerp(currentScale, targetScale, .25f);
        }
    }
}
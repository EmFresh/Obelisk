using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [FMODUnity.EventRef] public string mouseOver;
    [FMODUnity.EventRef] public string mouseClick;
    EventInstance instance;
    EventDescription overDescription, clickDescription;

    bool isMouseOver = false;
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
        overDescription = RuntimeManager.GetEventDescription(mouseOver);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        instance.release();
        overDescription.createInstance(out instance);
        instance.start();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        instance.release();
        clickDescription.createInstance(out instance);
        instance.start();
    }

    void Update()
    {
        if (isMouseOver)
        {

            minScale = transform.localScale;
            transform.localScale = Vector2.Lerp(minScale, maxScale, .25f);
            return;
        }

        currentScale = transform.localScale;
        transform.localScale = Vector2.Lerp(currentScale, targetScale, .25f);

    }
}
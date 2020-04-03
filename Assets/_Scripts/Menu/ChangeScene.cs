using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class ChangeScene : MonoBehaviour
{
    //public string sceneName;
    [FMODUnity.EventRef] public string Event;
    EventInstance instance;
    EventDescription eventDescription;
    [SerializeField] private string scene;
    private void Start()
    {
        eventDescription = RuntimeManager.GetEventDescription(Event);
    }
    public void SceneChange(string sceneName)
    {
        eventDescription.createInstance(out instance);
        instance.start();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

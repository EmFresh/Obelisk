using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SplashFade : MonoBehaviour
{
    bool faded = false;
    bool done = false;

    public Image black;
    public string scene;

    public KeyCode skipKey = KeyCode.Space;

    void Start()
    {
        //StartCoroutine(FadeCanvasGroup(GetComponent<CanvasGroup>(), GetComponent<CanvasGroup>().alpha, 1, duration));
        StartCoroutine(FadeCanvasInOut());
    }

    void Update()
    {
        if (Input.GetKeyDown(skipKey))
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
    IEnumerator FadeCanvasInOut()
    {
        //FADE OUT
        if (!faded)
        {
            black.CrossFadeAlpha(1, 2, false);
            yield return new WaitForSeconds(0f);
            faded = true;
        }

        //FADE BACK IN
        if (faded)
        {
            black.CrossFadeAlpha(0, 2f, false);
            yield return new WaitForSeconds(40f);
            done = true;
        }
        if (done)
        {
             SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

}
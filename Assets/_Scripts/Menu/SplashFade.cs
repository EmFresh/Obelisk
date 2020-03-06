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

    void Start()
    {
        //StartCoroutine(FadeCanvasGroup(GetComponent<CanvasGroup>(), GetComponent<CanvasGroup>().alpha, 1, duration));
        StartCoroutine(FadeCanvasInOut());
    }

    IEnumerator FadeCanvasInOut()
    {
        //FADE OUT
        if (!faded)
        {
            black.CrossFadeAlpha(0, 2, false);
            yield return new WaitForSeconds(2f);
            faded = true;
        }

        //FADE BACK IN
        if (faded)
        {
            black.CrossFadeAlpha(1, 3f, false);
            yield return new WaitForSeconds(3.5f);
            done = true;
        }
        if (done)
        {
             SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }



    }

}
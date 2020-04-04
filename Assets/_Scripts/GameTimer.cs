using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    // Start is called before the first frame update

    public float counter = 300.0f;
    public Text countText;

    public string TieScene;

    public bool timeOut = false;
    void Start()
    {
        countText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 10)
        {
            counter -= Time.deltaTime;
            countText.text = "Time Left: " + Mathf.Round(counter);
        }
        if (counter > 0 && counter < 10)
        {
            counter -= Time.deltaTime;
            countText.text = "Time Left: " + Mathf.Round(counter * 100f) / 100f;
        }
        if (counter <= 0)
        {
            //timeOut = true;
            SceneManager.LoadScene(TieScene, LoadSceneMode.Single);
            countText.text = "Time Left: " + Mathf.Round(counter);
            Debug.Log("TimeOut = True");
        }
    }

}

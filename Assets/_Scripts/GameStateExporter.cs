using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class GameStateExporter : MonoBehaviour
{
    public GameObject BuildSite1;
    public GameObject BuildSite2;
    public GameObject TextWin1;
    public GameObject TextWin2;

    private Blink blinkScript;
    // Start is called before the first frame update
    RogueData rogueData = new RogueData();
    string path;
    string json;

    readonly string postURL = "192.168.0.100/index.php";
    
    void Start()
    {
        path = Application.dataPath + "/gameState.json";
        blinkScript = GameObject.Find("Player 2 (Runner)").GetComponent<Blink>();
        rogueData.isBlinking = blinkScript.isBlinking;

        json = JsonUtility.ToJson(rogueData);
        Debug.Log(json);


        if (!File.Exists(path))
        {
            File.WriteAllText(path, json);
        }
    }

    // Update is called once per frame
    void Update()
    {
        path = Application.dataPath + "/gameState.json";
        blinkScript = GameObject.Find("Player 2 (Runner)").GetComponent<Blink>();
        rogueData.isBlinking = blinkScript.isBlinking;

        if (rogueData.isBlinking)
        {
            rogueData.blinkTime = (blinkScript.endBlinkTime) - Time.time;
        }
        else
            rogueData.blinkTime = 0;

        json = JsonUtility.ToJson(rogueData);
        Debug.Log(json);

        File.WriteAllText(path, json);

        string jsonData = File.ReadAllText(path);
       
        if((BuildSite1.GetComponent<TowerBuild>().stage >= 3)|| (BuildSite2.GetComponent<TowerBuild>().stage >= 3))
        {
            TextWin1.SetActive(true);
            TextWin2.SetActive(true);
        }
    }

    private class RogueData
    {
        public bool isBlinking;
        public float blinkTime;
    }

 }

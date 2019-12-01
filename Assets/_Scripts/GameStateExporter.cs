using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateExporter : MonoBehaviour
{
    private Blink blinkScript;
    // Start is called before the first frame update
    RogueData rogueData  = new RogueData();
        string path; 
string json;
    void Start()
    {
        path = Application.dataPath + "/gameState.json";
        blinkScript = GameObject.Find("Player 2 (Runner)").GetComponent<Blink>();      
        rogueData.isBlinking = blinkScript.isBlinking;
        json = JsonUtility.ToJson(rogueData);
        Debug.Log(json);

        if(!File.Exists(path))
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
        json = JsonUtility.ToJson(rogueData);
        Debug.Log(json);

        File.WriteAllText(path, json);
        
    }

    private class RogueData
    {
        public bool isBlinking;
    }
}

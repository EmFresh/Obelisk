using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobyManager : MonoBehaviour
{
    public RawImage RRimage;
    public RawImage RWimage;
    public RawImage BRimage;
    public RawImage BWimage;
    public Text RRtext;
    public Text RWtext;
    public Text BRtext;
    public Text BWtext;
    public Text chatBox;

    int userCount;
    RawImage[] rawImages;
    Text[] texts;

    public void SeatSelection(int i)
    {
        NetworkControl.sendSeatSelection(i);
    }

    public void SendStart()
    {
        NetworkControl.sendStartRequest();
    }

    // Start is called before the first frame update
    void Start()
    {
        rawImages = new RawImage[] { RRimage, RWimage, BRimage, BWimage };
        texts = new Text[] { RRtext, RWtext, BRtext, BWtext };
        updateText();
        updateImage();
        chatBox.text = "Welcome " + NetworkControl.thisUser._name + ", you have successfully joined the game!";
        userCount = NetworkControl.users.Count;
        if (NetworkControl.users.Count != 0)
        {
            for (int i = 0; i < NetworkControl.users.Count; i++)
            {
                chatBox.text = NetworkControl.users[i]._name + " joined the game!\n" + chatBox.text;
            }
        }
    }

    // Update is called once per frame      
    void Update()
    {
        updateText();
        updateImage();
        if (NetworkControl.users.Count > userCount)
        {
            for (int i = 0; i < NetworkControl.users.Count - userCount; i++)
            {
                chatBox.text = NetworkControl.users[userCount + i]._name + " joined the game!\n" + chatBox.text;
            }
            userCount = NetworkControl.users.Count;
        }
    }

    void updateText()
    {
        for (int i = 0; i < 4; i++)
        {
            if (NetworkControl.seat[i] == 0)
            {
                texts[i].text = "Click to Join";
            }
            else
            {
                texts[i].text = NetworkControl.FindPlayerName(NetworkControl.seat[i]);
            }
        }
    }

    void updateImage()
    {
        for (int i = 0; i < 4; i++)
        {
            if (NetworkControl.start[i] == 0)
            {
                rawImages[i].color = Color.white;
            }
            else
            {
                rawImages[i].color = Color.grey;
            }
        }
    }
}
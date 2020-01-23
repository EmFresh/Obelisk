using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessIcon : MonoBehaviour
{
    public GameObject icon;
    public Image mask;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mask.fillAmount = player.GetComponent<PlayerPickup>().gatherPercent;
    }
}

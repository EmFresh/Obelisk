using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    [Range(0, 1)]
    public float percentHealth = 1,
        maxHealth = 1;
    private Material rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = Instantiate(GetComponent<RawImage>().material);
        GetComponent<RawImage>().material = rend;
    }

    // Update is called once per frame
    void Update()
    {
        var player = transform.parent.parent.parent.GetComponent<PlayerMovement>();
        float percent = player.currentHealth / player.healthAmount;
        percentHealth = percent;

        //percent -= 0.001f;
        rend.SetFloat("health", Mathf.Clamp(percentHealth * maxHealth, 0, 1));
    }
}
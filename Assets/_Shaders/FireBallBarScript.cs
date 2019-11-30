using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBallBarScript : MonoBehaviour
{

    [Range(0, 1)]
    public float[] percentFire = new float[3] { 1, 1, 1 };

    private Material[] fire = new Material[3];
    private PlayerSpellShot shot;

    // Start is called before the first frame update
    void Start()
    {
        const int start = 1;
        shot = transform.parent.parent.GetComponent<PlayerSpellShot>();
        //start at second child second
        for (int index = 0; index < 3; index++)
        {
            fire[index] = Instantiate(transform.GetChild(index + start).GetComponent<RawImage>().material);
            transform.GetChild(index + start).GetComponent<RawImage>().material = fire[index];
        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int index = 0; index < 3; ++index)
            fire[index].SetFloat("fire", percentFire[index] = 0);
            
        for (int index = 0; index < 3; ++index)
        {
            if (shot.shots[index])
                fire[index].SetFloat("fire", percentFire[index] = 1);
            else
            {
                fire[index].SetFloat("fire", percentFire[index] = Mathf.Clamp(shot.shotTimer / shot.shotCooldown, 0, 1));
                break;
            }
        }
    }
}

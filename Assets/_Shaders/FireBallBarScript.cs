using System;
using UnityEngine;
using UnityEngine.UI;

public class FireBallBarScript : MonoBehaviour
{

    [Range(0, 1)]
    public float[] percentFire = new float[3] { 1, 1, 1 };
    private Material[] fire = new Material[3];
    private PlayerSpellShot shot;
    private float[] speed = new float[3];

    // Start is called before the first frame update
    void Start()
    {
        shot = transform.parent.parent.GetComponent<PlayerSpellShot>();//Gets the spell shot script


        System.Random rand = new System.Random();

        //gets the three fire ball sprites
        for (int index = 0; index < 3; ++index)
        {
            speed[index] = rand.Next(2);
            speed[index] += (float)rand.Next(100) * .01f;

            fire[index] = Instantiate(transform.GetChild(index).GetComponent<RawImage>().material);
            transform.GetChild(index).GetComponent<RawImage>().material = fire[index];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int index = 0; index < 3; ++index)
        {
            transform.GetChild(index).GetComponent<RawImage>().color = Color.white;
            fire[index].SetFloat("fire", percentFire[index] = 0);
            fire[index].SetFloat("timer", Time.time * (2.25f + speed[index]));
            fire[index].SetInt("sway", shot.shots[index] ? 1 : 0);

        }
        for (int index = 0; index < 3; ++index)
        {
            Color lightGrey = new Color(.6f, .6f, .6f, 1);

            if (shot.shots[index])
                fire[index].SetFloat("fire", percentFire[index] = 1);
            else
            {
                fire[index].SetFloat("fire", percentFire[index] = Mathf.Clamp(shot.shotTimer / shot.shotCooldown, 0, 1));
                transform.GetChild(index).GetComponent<RawImage>().color = Color.grey;
                break;
            }
        }
    }
}

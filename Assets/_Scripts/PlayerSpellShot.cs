using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellShot : MonoBehaviour
{
    public GameObject projectial;
    public KeyCode key;
    public float duration;

    IList<GameObject> Projcopy = new List<GameObject>();
    IList<float> projCounter = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
    }
    float movement = 0;

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        //creates a new gameObject every time a key is pressed
        if (Input.GetKeyDown(key))
        {
            Projcopy.Add(Instantiate(projectial));
            projCounter.Add(0);
            Projcopy[Projcopy.Count - 1].transform.position = transform.position;

            Debug.Log("I need you to move");
            movement = .1f;
            Projcopy[Projcopy.Count - 1].transform.position = transform.position;
        }


        for (int i = 0; i < projCounter.Count; i++)
        {
            //Object destruction after 5 seconds
            if (projCounter[i] >= duration)
            {
               Destroy(Projcopy[i]);
                Projcopy.RemoveAt(i);
                projCounter.RemoveAt(i);
                i--;
                continue;
            }
            
            //increase the timer
            projCounter[i] += dt;

            //Move the object
            Projcopy[i].transform.position += transform.forward * movement;
        }
    }
}

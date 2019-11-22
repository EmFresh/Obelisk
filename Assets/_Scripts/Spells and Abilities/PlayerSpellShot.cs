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
    IList<Vector3> direction = new List<Vector3>();

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
            direction.Add(transform.forward);
            Projcopy[Projcopy.Count - 1].transform.position = transform.position;
            Projcopy[Projcopy.Count - 1].transform.rotation = transform.rotation;

            //            Debug.Log("I need you to move");
            movement = .1f;
            Projcopy[Projcopy.Count - 1].transform.position = transform.position;
            //  Projcopy[Projcopy.Count - 1].transform.parent = this.transform;
        }


        for (int i = 0; i < projCounter.Count; i++)
        {
            //Remove all destroyed objects
            if (Projcopy[i] == null)
            {
                Projcopy.RemoveAt(i);
                projCounter.RemoveAt(i);
                direction.RemoveAt(i);
                i--;

                continue;
            }

            //Object destruction after 5 seconds
            if (projCounter[i] >= duration)
            {
                Debug.Log(direction[i]);
                Destroy(Projcopy[i]);
                Projcopy.RemoveAt(i);
                projCounter.RemoveAt(i);
                direction.RemoveAt(i);
                i--;

                continue;

            }

            //increase the timer
            projCounter[i] += dt;

            //Move the object
            Projcopy[i].transform.position += direction[i] * movement;
        }
    }
}

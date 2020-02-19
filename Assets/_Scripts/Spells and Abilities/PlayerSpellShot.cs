using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControllerInput;
using static ControllerInput.CONTROLLER_BUTTON;

public class PlayerSpellShot : MonoBehaviour
{
    public GameObject projectial;
    public KeyCode fireKey = KeyCode.F;
    public CONTROLLER_BUTTON fireJoy = B;
    public float duration;
    public float shotCooldown;
    public bool[] shots = new bool[3] { true, true, true };
    public float shotTimer = 0;
    [Range(0,50)]public float movement = .1f;
    public Animator _animator;


    private ushort playerIndex;
    private IList<GameObject> Projcopy = new List<GameObject>();
    private IList<float> projCounter = new List<float>();
    private IList<Vector3> direction = new List<Vector3>();

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("isShoot", false);
        float dt = Time.deltaTime;
        playerIndex = GetComponent<PlayerMovement>().playerIndex;

        //creates a new gameObject every time a key is pressed
        if ((Input.GetKeyDown(fireKey) || isButtonDown(playerIndex, (int)fireJoy)) && shots[0])
        {
            _animator.SetBool("isShoot", true);
            Projcopy.Add(Instantiate(projectial));
            projCounter.Add(0);
            direction.Add(transform.forward);
            Projcopy[Projcopy.Count - 1].transform.position = transform.position + new Vector3(0.2f,1.5f,0);
            Projcopy[Projcopy.Count - 1].transform.rotation = transform.rotation;

            
           // Projcopy[Projcopy.Count - 1].transform.position = transform.position;

            for (int a = shots.Length - 1; a >= 0; --a)
                if (shots[a])
                {
                    shots[a] = false;
                    break;
                }
            //Projcopy[Projcopy.Count - 1].transform.parent = this.transform;
        }

        //checks if the player can make a shot
        if (!(shots[0] && shots[1] && shots[2]))
        {
            shotTimer += Time.deltaTime * 0.5f;
            for (int a = 0; a < shots.Length; ++a)
                if (!shots[a])
                {
                    if (shotTimer > shotCooldown)
                    {
                        shotTimer = 0;
                        shots[a] = true;
                    }
                    break;
                }
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
            Projcopy[i].transform.position += direction[i] * movement * Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ControllerInput;
using static ControllerInput.CONTROLLER_BUTTON;

public class PlayerSpellShot : MonoBehaviour
{
    public Vector3 directionOffset;
    public GameObject projectile;
    public GameObject explosion;
    public KeyCode fireKey = KeyCode.F;
    public CONTROLLER_BUTTON fireJoy = B;
    public float duration;
    public float shotCooldown;
    public bool[] shots = new bool[3] { true, true, true };
    public float shotTimer = 0;
    [Range(0, 50)] public float movement = .1f;
    public Animator _animator;

    private ushort playerIndex;
    private IList<GameObject> projcopy = new List<GameObject>();
    private IList<float> projCounter = new List<float>();
    private IList<Vector3> direction = new List<Vector3>();
    private IList<Vector3> position = new List<Vector3>();

    [HideInInspector]public bool stopRumble;
    float rumbleDur = 0.2f;
    float rumbleCount = 0;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rumbleCount = rumbleDur;
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("isShoot", false);
        float dt = Time.deltaTime;
        playerIndex = GetComponent<PlayerMovement>().playerIndex;

        //creates a new gameObject every time a key is pressed
        if ((Input.GetKeyDown(fireKey) || isButtonDown(playerIndex, (int)fireJoy)) && shots[0])
        {
            rumbleCount = 0;
            stopRumble = true;
            setVibrationR(playerIndex, 5);

            _animator.SetBool("isShoot", true);
            projcopy.Add(Instantiate(projectile));
            projcopy.Last().GetComponent<FireballCollision>().caster = gameObject;
            projCounter.Add(0);

            var forw = transform.forward;
            forw.y = GetComponentInChildren<CameraMovement>().gameObject.transform.forward.y;
            forw.x *= 1 - Mathf.Abs(forw.y);
            forw.z *= 1 - Mathf.Abs(forw.y);
            forw.Normalize();
            forw = Quaternion.Euler(directionOffset) * forw;
            direction.Add(forw);

            position.Add(transform.position + new Vector3(0.2f, 1.5f, 0));
            projcopy.Last().transform.position = position.Last();
            projcopy.Last().transform.rotation = transform.rotation;

            // Projcopy[Projcopy.Count - 1].transform.position = transform.position;

            for (int a = shots.Length - 1; a >= 0; --a)
                if (shots[a])
                {
                    shots[a] = false;
                    break;
                }
            //Projcopy[Projcopy.Count - 1].transform.parent = this.transform;
        }

        //Rumble 
        if (rumbleCount < rumbleDur)
            rumbleCount += Time.deltaTime;
        else if (stopRumble)
        {
            stopRumble = false;
            resetVibration(playerIndex);
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
            if (projcopy[i] == null)
            {
                var tmp = position[i];
                var explosioncpy = Instantiate(explosion, new Vector3(tmp.x, tmp.y, tmp.z), Quaternion.identity);

                projcopy.RemoveAt(i);
                projCounter.RemoveAt(i);
                direction.RemoveAt(i);
                position.RemoveAt(i);
                i--;

                continue;
            }

            //Object destruction after 5 seconds
            if (projCounter[i] >= duration)
            {
                var tmp = projcopy[i].transform.position;
                var explosioncpy = Instantiate(explosion, new Vector3(tmp.x, tmp.y, tmp.z), Quaternion.identity);

                // Debug.Log(direction[i]);
                Destroy(projcopy[i]);
                projcopy.RemoveAt(i);
                projCounter.RemoveAt(i);
                direction.RemoveAt(i);
                position.RemoveAt(i);
                i--;

                continue;

            }

            //increase the timer
            projCounter[i] += dt;

            //Move the object
            position[i] += direction[i] * movement * Time.deltaTime;
            projcopy[i].transform.position = position[i];
        }
    }
}
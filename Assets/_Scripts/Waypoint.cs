using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //

    public static bool touchedWaypoint = false;
    public static bool touchedWaypoint2 = false;
    float waypointTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player 2") && !touchedWaypoint)
        {
            touchedWaypoint = true;
            //Debug.Log("waypoint move??");
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 25);
            waypointTime = Time.time;
            
        }
        if (other.gameObject.CompareTag("Player 2") && !touchedWaypoint2 && touchedWaypoint && Time.time - waypointTime >= 3)
        {
            touchedWaypoint2 = true;
            //Debug.Log("waypoint move 2??");
            this.gameObject.SetActive(false);
            
        }
    }
}

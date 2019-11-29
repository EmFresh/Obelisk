using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //

    public static bool touchedWaypoint = false;
    public static bool touchedWaypoint2 = false;

    public static bool P2touchedWaypoint = false;
    public static bool P2touchedWaypoint2 = false;

    float waypointTime;
    float waypointTime2;
     public GameObject waypoint1;

    public GameObject waypoint2;

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
        if (other.gameObject.CompareTag("Player 1") && !touchedWaypoint)
        {
            touchedWaypoint = true;
            //Debug.Log("waypoint move??");
            waypoint1.SetActive(false);//this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 25);
            waypoint2.SetActive(true);
            waypointTime = Time.time;
            
        }
        if (other.gameObject.CompareTag("Player 1") && !touchedWaypoint2 && touchedWaypoint && Time.time - waypointTime >= 3 && Blink.isBlinking)
        {
            touchedWaypoint2 = true;
            //Debug.Log("waypoint move 2??");
            this.gameObject.SetActive(false);
            
        }


        if (other.gameObject.CompareTag("Player 2") && !P2touchedWaypoint)
        {
            P2touchedWaypoint = true;
            //Debug.Log("waypoint move??");
            waypoint1.SetActive(false);//this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 25);
            waypoint2.SetActive(true);
            waypointTime2 = Time.time;
            
        }
        if (other.gameObject.CompareTag("Player 2") && !P2touchedWaypoint2 && P2touchedWaypoint && Time.time - waypointTime2 >= 3 && Blink.isBlinking)
        {
            P2touchedWaypoint2 = true;
            //Debug.Log("waypoint move 2??");
            this.gameObject.SetActive(false);
            
        }
    }
}

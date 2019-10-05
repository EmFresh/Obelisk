using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerBuild : MonoBehaviour
{

    public List<GameObject> towerParts = new List<GameObject>();
    public KeyCode key;
    private Transform theParent;

    public int stage;
    // Start is called before the first frame update
    void Start()
    {
        theParent = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider obj)
    {
        Debug.Log("entered build site");
         if (obj.gameObject.tag.Contains("BuildZone"))
            if (Input.GetKeyDown(key))
            {
                 Debug.Log("build the tower");
                GameObject towerPart = Instantiate(towerParts[0]);
                towerPart.transform.parent = theParent;
                theParent = towerPart.transform;
                stage++;
            }
    }

}

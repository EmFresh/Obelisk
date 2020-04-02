using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public GameObject spellcaster;
    public GameObject rogue;
    public GameObject spawnSpellcaster;
    public GameObject spawnRogue;
    public GameObject playArea;
    /*
        Spellcaster 
        spring: 5.248541 130.0231 -55.85095
        fall: 2.49 130.23 59.27
        
        Rogue
        spring: -5.051459 130.0231 -55.75095
        fall: -3.14 130.23 59.27
    */

    // Start is called before the first frame update
    void Start()
    {
        spellcaster = Instantiate(spellcaster, transform);
        rogue = Instantiate(rogue, transform);

        spellcaster.transform.position = spawnSpellcaster.transform.position;
        rogue.transform.position = spawnRogue.transform.position;

        GameObject[] players = { rogue, spellcaster };
        GameObject[] spawns = { spawnRogue, spawnSpellcaster };

        switch (tag.ToLower())
        {
            case "team 1":
                for (ushort a = 0; a < 2; ++a)
                {
                    if (NetworkControl.users[a]._id == NetworkControl.thisUser._id)
                    {
                        players[a].GetComponent<PlayerMovement>().controllerIndex = 0;
                        players[a].GetComponent<PlayerMovement>().enableKeyboard = true;
                        players[a].GetComponent<PlayerMovement>().isNetworkedPlayer = false;
                        players[a].GetComponent<PlayerMovement>().networkID = (ushort)NetworkControl.users[a]._id;
                        players[a].transform.position = spawns[a].transform.position;
                        players[a].transform.rotation = spawns[a].transform.rotation;
                        players[a].GetComponent<Respawn>().spawnpoint = spawns[a];
                        players[a].GetComponent<Respawn>().area = playArea;
                   
                    }
                    else
                    {
                        players[a].GetComponent<PlayerMovement>().controllerIndex = 5; //un-useable
                        players[a].GetComponent<PlayerMovement>().enableKeyboard = false;
                        players[a].GetComponent<PlayerMovement>().isNetworkedPlayer = true;
                        players[a].GetComponent<PlayerMovement>().networkID =  (ushort)NetworkControl.users[a]._id;
                    }
                }

                break;
            case "team 2":
                for (int a = 2; a < 4; ++a)
                {

                    if (NetworkControl.users[a]._id == NetworkControl.thisUser._id)
                    {
                        players[a].GetComponent<PlayerMovement>().controllerIndex = 0;
                        players[a].GetComponent<PlayerMovement>().enableKeyboard = true;
                        players[a].GetComponent<PlayerMovement>().isNetworkedPlayer = false;
                        players[a].GetComponent<PlayerMovement>().networkID = (ushort)NetworkControl.users[a]._id;
                        players[a].transform.position = spawns[a].transform.position;
                        players[a].transform.rotation = spawns[a].transform.rotation;
                        players[a].GetComponent<Respawn>().spawnpoint = spawns[a];
                        players[a].GetComponent<Respawn>().area = playArea;
                    }
                    else
                    {
                        players[a].GetComponent<PlayerMovement>().controllerIndex = 5; //un-useable
                        players[a].GetComponent<PlayerMovement>().enableKeyboard = false;
                        players[a].GetComponent<PlayerMovement>().isNetworkedPlayer = true;
                        players[a].GetComponent<PlayerMovement>().networkID = (ushort)NetworkControl.users[a]._id ;
                    }
                }
                break;
        }

    }
}
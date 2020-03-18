using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Unity.Jobs;
using static Networking;
using static Networking.PResult;


public class Chat : MonoBehaviour
{

    IPEndpointData serverIP;
    // NetworkJob job;
    SocketData sock;
    //Socket socket = null;
    // Start is called before the first frame update
    void Start()
    {

    }


    private void Awake()
    {
       // Networking.initNetwork();
       // serverIP = createIPEndpointData("localhost", 8888);
       // sock = initSocketData();
       //
       // string test = "Strings are wack bro";
       // if (createSocket(ref sock, SocketType.UDP) != P_GenericError)
       // {
       //     if (sendToPacket(ref sock, ref test, test.Length, ref serverIP) == P_GenericError)
       //         print(getLastNetworkError());
       //
       // }
       // print(getLastNetworkError());


    }


    private void OnApplicationQuit()
    {
       // if (closeSocket(ref sock) == P_GenericError) print(getLastNetworkError());
       // shutdownNetwork();
    }
}

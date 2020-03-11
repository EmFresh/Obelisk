using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Jobs;
using UnityEngine;
using static Networking;
using static Networking.PResult;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkControl : MonoBehaviour
{
    public Text ipText;
    public Text nameText;
    public static List<user> users;
    public static user thisUser;
    public static int[] seat;
    public static int[] start;

    static bool goLobby = false;
    static bool goGame = false;

    static SocketData sock;
    static IPEndpointData endp;
    NetworkClientJob jobClient;
    JobHandle hnd;
    [HideInInspector] public static bool close;
    [StructLayout(LayoutKind.Sequential)]
    struct vec3
    {
        public float x, y, z;
        public vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static vec3 operator -(vec3 v1, vec3 v2) => new vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        public float length() => Mathf.Sqrt(x * x + y * y + z * z);

        public static float dist(vec3 v1, vec3 v2) => (v1 - v2).length();

        public override string ToString() => "(" + x + ", " + y + ", " + z + ")";
    }

    public struct user
    {
        public user(string n, int d)
        {
            this._name = n;
            this._id = d;
        }
        public string _name;
        public int _id;
    }

    void PrintError(object msg) => Debug.LogError(msg);

    public struct NetworkClientJob : IJob
    {
        public SocketData sock;
        public IPEndpointData endp;
        public void Execute()
        {
            int size = 512, dump = 0;
            string recv = "";
            for (;;)
            {
                bool printable = true;
                if (recvFromPacket(ref sock, ref recv, size, ref dump, ref endp) == P_GenericError)
                {
                    printable = false;
                    Debug.LogError(getLastNetworkError());
                }

                if (printable)
                {
                    if (recv[0] == '@')
                    {
                        recv = recv.Substring(1);
                        string[] strings = recv.Split(':');
                        int index = 0;

                        // update the user's id
                        thisUser._id = int.Parse(strings[index]);
                        index++;

                        print(strings[index]);
                        // update other users
                        if (strings[index] != "0")
                        {
                            int t = int.Parse(strings[index]);
                            for (int i = 0; i < t; i++)
                            {
                                index++;
                                users.Add(new user(strings[index], int.Parse(strings[index + 1])));
                                index++;
                            }
                        }
                        // update seat status
                        index++;
                        for (int i = 0; i < 4; i++)
                        {
                            print(strings[index]);
                            seat[i] = int.Parse(strings[index]);
                            index++;
                        }
                        // update start status
                        for (int i = 0; i < 4; i++)
                        {
                            start[i] = int.Parse(strings[index]);
                            index++;
                        }
                        // go next scene
                        goLobby = true;
                    }
                    else if (recv[0] == '!')
                    {
                        recv = recv.Substring(1);
                        string[] strings = recv.Split(':');
                        users.Add(new user(strings[0], int.Parse(strings[1])));
                        print(strings[0] + "join");
                    }
                    else if (recv[0] == '#')
                    {
                        recv = recv.Substring(2);
                        string[] strings = recv.Split(':');
                        int index = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            print(strings[index] + ":");
                            seat[i] = int.Parse(strings[index]);
                            index++;
                        }
                    }
                    else if (recv[0] == '%')
                    {
                        print(recv);
                        recv = recv.Substring(2);
                        string[] strings = recv.Split(':');
                        int index = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            start[i] = int.Parse(strings[index]);
                            index++;
                        }
                    }
                    else if (recv[0] == '$')
                    {
                        goGame = true;
                    }
                    else
                        print(recv);
                }

                if (NetworkControl.close)
                    break;

            }
        }
    }

    #region Singleton
    public static NetworkControl Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void connectToInternet()
    {

        string serverAddr = ipText.text;
        string userName = nameText.text;

        thisUser._name = userName;

        // Create UDP Client
        shutdownNetwork(); //just incase there was some sort of error last time
        Networking.initNetwork();
        endp = createIPEndpointData(serverAddr, 8888);
        sock = initSocketData();

        if (createSocket(ref sock, SocketType.UDP) == P_GenericError)
            Debug.LogError(getLastNetworkError());

        if (connectEndpoint(ref endp, ref sock) == P_GenericError)
            Debug.LogError(getLastNetworkError());

        jobClient = new NetworkClientJob()
        {
            sock = sock,
            endp = endp
        };
        hnd = jobClient.Schedule(); //schedules the job to start asynchronously like std::detach in c++

        string tmp = "@" + userName;

        int dump = 0;

        if (sendToPacket(ref sock, ref tmp, ref dump, ref endp) == P_GenericError)
        {
            Debug.LogError(getLastNetworkError());
        }

    }

    static public void sendSeatSelection(int idex)
    {
        if (!checkUserStart())
        {
            if (seat[idex] == 0 && !checkUserInSeat())
            {
                string tmp = "#" + idex + thisUser._id;

                int dump = 0;

                if (sendToPacket(ref sock, ref tmp, ref dump, ref endp) == P_GenericError)
                {
                    Debug.LogError(getLastNetworkError());
                }
            }
            if (seat[idex] == 0 && checkUserInSeat())
            {
                int dump = 0;

                string tmp = "$" + thisUser._id;

                if (sendToPacket(ref sock, ref tmp, ref dump, ref endp) == P_GenericError)
                {
                    Debug.LogError(getLastNetworkError());
                }

                tmp = "#" + idex + thisUser._id;

                if (sendToPacket(ref sock, ref tmp, ref dump, ref endp) == P_GenericError)
                {
                    Debug.LogError(getLastNetworkError());
                }
            }
        }
    }

    static public void sendStartRequest()
    {
        if (checkUserInSeat())
        {
            int dump = 0;

            string tmp = "%" + thisUser._id;

            if (sendToPacket(ref sock, ref tmp, ref dump, ref endp) == P_GenericError)
            {
                Debug.LogError(getLastNetworkError());
            }
        }
    }

    static bool checkUserInSeat()
    {
        bool inSeat = false;
        for(int i = 0; i < 4; i++)
        {
            if(seat[i] == thisUser._id)
            {
                inSeat = true;
                break;
            }
        }
        return inSeat;
    }

    static bool checkUserStart()
    {
        bool inSeat = false;
        for (int i = 0; i < 4; i++)
        {
            if (start[i] == thisUser._id)
            {
                inSeat = true;
                break;
            }
        }
        return inSeat;
    }

    public static string FindPlayerName(int id)
    {
        string name = "";
        if (id == thisUser._id)
            return thisUser._name;
        for(int i = 0; i<users.Count; i++)
        {
            if (users[i]._id == id)
            {
                name = users[i]._name;
                break;
            }
        }
        return name;
    }

    private void Start()
    {
        seat = new int[] { 0, 0, 0, 0 };
        start = new int[] { 0, 0, 0, 0 };
        users = new List<user> { };
    }

    private void Update()
    {
        if (goLobby)
        {
            goLobby = false;
            // Go to Lobby
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
        if (goGame)
        {
            goGame = false;
            // Go to Game
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);

        }
    }

    private void OnApplicationQuit()
    {
        close = true;

        closeSocket(ref sock);
        shutdownNetwork();
        hnd.Complete(); //should be the same as thread::join in c++
    }
}
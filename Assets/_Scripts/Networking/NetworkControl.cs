using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Jobs;
using UnityEngine;
using static Networking;
using static Networking.PResult;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkControl : MonoBehaviour
{
    #region Variables

    public Text ipText;
    public Text nameText;
    public GameObject connectionerror;

    public static List<user> users;
    public static Movement[] movements = new Movement[4];
    public static user thisUser;
    public static int[] seat;
    public static int[] start;

    static bool goLobby = false;
    static bool goGame = false;

    public static SocketData sock;
    public static IPEndpointData ip;
    static NetworkLobyRecvJob jobLobbyRecv;
    static NetworkGameRecvJob jobGameRecv;
    static JobHandle hndLobby, hndGame;
    [HideInInspector] public static bool close;
    #endregion

    public enum MessageType : int
    {
        Unknown,
        Movement,
        HealthInfo,
        Fireball,
        ResourceSpawn,
        ResourceCollected
    }

    #region classes

    [StructLayout(LayoutKind.Sequential)]
    public class Packet
    {
        public MessageType type = NetworkControl.MessageType.Unknown;
        public int size = Marshal.SizeOf<Packet>();
    }

    [StructLayout(LayoutKind.Sequential)]
    public class Unknown : Packet
    {
        public Unknown()
        {
            type = MessageType.Movement; //int
            size = Marshal.SizeOf<Unknown>(); //int
        }
        public long l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14, l15;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class Movement : Packet
    {
        public Movement()
        {
            type = MessageType.Movement; //int
            size = Marshal.SizeOf<Movement>(); //int
        }
        public uint id;
        public bool isUpdated;
        public float dt;
        public Vector3 pos, dir;
        public Quaternion rot;
    }
    #endregion

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

    #region IJobs

    public struct NetworkLobyRecvJob : IJob
    {
        public SocketData sock;
        private IPEndpointData ip;
        public void Execute()
        {
            int size = 512, dump;
            string recv = "";
            for (; ; )
            {
                bool printable = true;
                if (isNetworkInit)
                {
                    if (recvFromPacket(sock, out recv, size, out dump, out ip) == P_GenericError)
                    {
                        printable = false;
                        PrintError(getLastNetworkError());
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
                }

                if (NetworkControl.close)
                    break;

            }
        }
    }

    public struct NetworkGameRecvJob : IJob
    {
        public SocketData sock;
        private IPEndpointData ip;

        public void Execute()
        {
            if (isNetworkInit)
            {
                while (true)
                {
                    Unknown unknown;
                    unknown = new Unknown();
                    if (recvFromPacket(sock, out unknown, 512, out ip) == PResult.P_Success)
                        switch (unknown.type)
                        {
                            case MessageType.Movement:
                                //TODO: recv the movement variable from other clients
                                Movement move = (Movement)(Packet)unknown; //I cant believe this works
                                move.isUpdated = true;
                                movements[move.id] = move;
                                break;
                            case MessageType.Fireball:
                                //TODO: recv position, velocity/sec and start time
                                break;
                            case MessageType.ResourceSpawn:
                                //TODO: recv position and resource type
                                break;
                            case MessageType.ResourceCollected:
                                //TODO: recv resource type and what position in the array was deleted
                                break;
                            default:
                                break;
                        }

                    if (NetworkControl.close)
                        break;
                }
            }
        }
    }
    #endregion

    #region Singleton
    public static NetworkControl inst;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(inst);
        }
        if (!ipText)
            ipText = inst.ipText;

        if (!nameText)
            nameText = inst.nameText;

        if (ipText)
        {
            if (inst.ipText != ipText)
                Destroy(inst.ipText);
            DontDestroyOnLoad(ipText);
        }
        if (nameText)
        {
            if (inst.nameText != nameText)
                Destroy(inst.nameText);
            DontDestroyOnLoad(nameText);
        }

        inst.ipText = ipText;
        inst.nameText = nameText;
    }
    #endregion

    #region Loby
    public void connectToInternet()
    {

        string serverAddr = ipText.text;
        string userName = nameText.text;

        thisUser._name = userName;

        // Create UDP Client

        shutdownNetwork();
        close = true; //close the running job
        closeSocket(sock);
        hndLobby.Complete();

        Networking.initNetwork();

        ip = createIPEndpointData(serverAddr, 8888);
        sock = initSocketData();
        if (createSocket(sock, SocketType.UDP) == P_GenericError)
            PrintError(getLastNetworkError());

        if (connectEndpoint(ip, sock) == P_GenericError)
        {
            PrintError(getLastNetworkError());
            connectionerror.SetActive(true);
        }
        jobLobbyRecv = new NetworkLobyRecvJob()
        {
            sock = sock,
            //ip = ip
        };
        close = false;
        hndLobby = jobLobbyRecv.Schedule(); //schedules the job to start asynchronously like std::detach c++

        string tmp = "@" + userName;

        if (sendToPacket(sock, tmp, 512, ip) == P_GenericError)
        {
            PrintError(getLastNetworkError());
        }

    }

    static public void sendSeatSelection(int index)
    {
        if (!checkUserStart())
        {
            if (seat[index] == 0 && !checkUserInSeat())
            {
                string tmp = "#" + index + thisUser._id;

                if (sendToPacket(sock, tmp, ip) == P_GenericError)
                {
                    PrintError(getLastNetworkError());
                }
            }
            if (seat[index] == 0 && checkUserInSeat())
            {

                string tmp = "$" + thisUser._id;

                if (sendToPacket(sock, tmp, ip) == P_GenericError)
                {
                    PrintError(getLastNetworkError());
                }

                tmp = "#" + index + thisUser._id;

                if (sendToPacket(sock, tmp, ip) == P_GenericError)
                {
                    PrintError(getLastNetworkError());
                }
            }
        }
    }

    static public void sendStartRequest()
    {
        if (checkUserInSeat())
        {

            string tmp = "%" + thisUser._id;

            if (sendToPacket(sock, tmp, ip) == P_GenericError)
            {
                PrintError(getLastNetworkError());
            }
        }
    }

    static bool checkUserInSeat()
    {
        bool inSeat = false;
        for (int i = 0; i < 4; i++)
        {
            if (seat[i] == thisUser._id)
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
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i]._id == id)
            {
                name = users[i]._name;
                break;
            }
        }
        return name;
    }
    #endregion

    private void Start()
    {
           
            movements[0] = new Movement();
            movements[1] = new Movement();
            movements[2] = new Movement();
            movements[3] = new Movement();
        

        if (users == null)
        {
            seat = new int[] { 0, 0, 0, 0 };
            start = new int[] { 0, 0, 0, 0 };
            users = new List<user> { };
        }
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
            shutdownNetwork();
            closeSocket(sock);
            close = true;
            hndLobby.Complete(); //should be the same as thread::join c++

            close = false;
            initNetwork();
            sock = initSocketData();
            if (createSocket(sock, SocketType.UDP) == P_GenericError)
                PrintError(getLastNetworkError());
            if (connectEndpoint(ip, sock) == P_GenericError)
                   PrintError(getLastNetworkError());
        
            jobGameRecv = new NetworkGameRecvJob()
            {
                sock = sock
            };

            hndGame = jobGameRecv.Schedule();

            goGame = false;

            // Go to Game
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);

        }
    }

    private void OnApplicationQuit()
    {
        close = true;

        if (!shutdownNetwork())
            PrintError(getLastNetworkError());
        closeSocket(sock);

        hndLobby.Complete(); //should be the same as thread::join c++
        hndGame.Complete();
    }
}
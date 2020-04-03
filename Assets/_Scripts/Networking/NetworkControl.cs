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
    static NetworkLobyRecvJob jobLobyRecv;
    static JobHandle hndLoby, hndGame;
    [HideInInspector] public static bool close;

    enum MessageType : int
    {
        Movement,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class Movement
    {
        MessageType type = MessageType.Movement;
        int size = Marshal.SizeOf<Movement>();
        public float dt;
        public Vector3 pos, vel;
        public Quaternion rot;
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

    #region IJobs

    public struct NetworkLobyRecvJob : IJob
    {
        public SocketData sock;
        public IPEndpointData ip;
        public void Execute()
        {
            int size = 512, dump = 0;
            string recv = "";
            for (;;)
            {
                bool printable = true;
                if (isNetworkInit)
                {
                    if (recvFromPacket(sock, out recv, size, ip) == P_GenericError)
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
        public IPEndpointData ip;

        public Movement move;

        public void Execute()
        {
            if(isNetworkInit)
            {
                recvFromPacket(sock, out move, ip);
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
        hndLoby.Complete();

        Networking.initNetwork();

        endp = createIPEndpointData(serverAddr, 8888);
        sock = initSocketData();
        if (createSocket(sock, SocketType.UDP) == P_GenericError)
            Debug.LogError(getLastNetworkError());

        if (connectEndpoint(endp, sock) == P_GenericError)
            Debug.LogError(getLastNetworkError());

        jobLobyRecv = new NetworkLobyRecvJob()
        {
            sock = sock,
            ip = endp
        };
        close = false;
        hndLoby = jobLobyRecv.Schedule(); //schedules the job to start asynchronously like std::detach c++

        string tmp = "@" + userName;

        if (sendToPacket(sock, tmp, 512, endp) == P_GenericError)
        {
            Debug.LogError(getLastNetworkError());
        }

    }

    static public void sendSeatSelection(int index)
    {
        if (!checkUserStart())
        {
            if (seat[index] == 0 && !checkUserInSeat())
            {
                string tmp = "#" + index + thisUser._id;

                if (sendToPacket(sock, tmp, endp) == P_GenericError)
                {
                    Debug.LogError(getLastNetworkError());
                }
            }
            if (seat[index] == 0 && checkUserInSeat())
            {

                string tmp = "$" + thisUser._id;

                if (sendToPacket(sock, tmp, endp) == P_GenericError)
                {
                    Debug.LogError(getLastNetworkError());
                }

                tmp = "#" + index + thisUser._id;

                if (sendToPacket(sock, tmp, endp) == P_GenericError)
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

            string tmp = "%" + thisUser._id;

            if (sendToPacket(sock, tmp, endp) == P_GenericError)
            {
                Debug.LogError(getLastNetworkError());
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
        seat = new int[] { 0, 0, 0, 0 };
        start = new int[] { 0, 0, 0, 0 };
        users = new List<user> {};
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

        if (!shutdownNetwork())
            PrintError(getLastNetworkError());
        // closeSocket(sock);
        hndLoby.Complete(); //should be the same as thread::join c++
    }
}
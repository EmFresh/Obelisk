using System.Runtime.InteropServices;
using UnityEngine;
using Unity.Jobs;
using static Networking;
using static Networking.PResult;

public class NetworkControle : MonoBehaviour
{
    SocketData sock;
    IPEndpointData endp;
    NetworkJob job;
    JobHandle hnd;
    struct vec3
    {
        float x, y, z;
        public override string ToString() => "(" + x + ", " + y + ", " + z + ")";
    }

    public struct NetworkJob : IJob
    {
        int size, dump;
        public SocketData sock;
        public IPEndpointData endp;
        public bool gotsomething;

        public void Execute()
        {
            size = 0; dump = 0;
            vec3 dat = new vec3();
            if (recvFromPacket(ref sock, ref size, 4, ref dump, ref endp) == P_Success)
            {
                if (size > 0)
                {
                    if (recvFromPacket(ref sock, ref dat, size, ref dump, ref endp) == P_Success)
                    {
                        if (dump == Marshal.SizeOf<vec3>(dat))
                            print(dat);
                    }
                    print(getLastNetworkError());
                }
            }
            print(getLastNetworkError());
            gotsomething = true;

        }
    }

    private void Awake()
    {

        //Networking.initNetwork();
        //endp = createIPEndpointData("localhost", 22);
        //sock = initSocketData();
//
        //if (sock.m_IPVersion == IPVersion.IPv4)
        //{
        //    if (createSocket(ref sock, SocketType.UDP) == P_GenericError)
        //        print(getLastNetworkError());
//
        //    if (bindEndpointToSocket(ref endp, ref sock) == P_GenericError)
        //        print(getLastNetworkError());
//
        //    job = new NetworkJob()
        //    {
        //        sock = this.sock,
        //        endp = this.endp,
        //        gotsomething = false
        //    };
        //    hnd = job.Schedule();//schedules the job to start asynchronously 
        //}
//        Networking.shutdownNetwork();
    }

    // Update is called once per frame
    void Update()
    {
       // if (job.gotsomething)
       // {
       //     hnd.Complete();//should be the same as thread::join in c++
       //     hnd = job.Schedule();
       // }
    }

    private void OnApplicationQuit()
    {
        closeSocket(ref sock);
        shutdownNetwork();
    }
}

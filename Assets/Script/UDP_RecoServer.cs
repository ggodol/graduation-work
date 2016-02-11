// *********************************************************
// UDP SPEECH RECOGNITION
// *********************************************************
using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI; //유니티 내에 Text 변경

public class UDP_RecoServer : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int port = 26000; // DEFAULT UDP PORT !!!!! THE QUAKE ONE ;)
    string strReceiveUDP = "";
    string LocalIP = String.Empty;
    string hostname;

    public Text speechword; //사람이 말한 것을 출력할 단어
    private bool _received = false; //말을 하면 했다고 체크

    public void Start()
    {
        Application.runInBackground = true;
        init();
    }

    public void Update()
    {
        if (_received) //말이 들어오면
        {
            speechword.text = strReceiveUDP;
            _received = false;
        }
    }

    // init
    private void init()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        hostname = Dns.GetHostName();
        IPAddress[] ips = Dns.GetHostAddresses(hostname);
        if (ips.Length > 0)
        {
            LocalIP = ips[0].ToString();
            Debug.Log(" MY IP : " + LocalIP);
        }
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Broadcast, port);
                byte[] data = client.Receive(ref anyIP);
                strReceiveUDP = Encoding.UTF8.GetString(data);
                // ***********************************************************************
                // Simple Debug. Must be replaced with SendMessage for example.
                // ***********************************************************************
                _received = true; //음성이 들어왔다는 것을 체크
                Debug.Log(strReceiveUDP);
                // ***********************************************************************
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    public string UDPGetPacket()
    {
        return strReceiveUDP;
    }

    void OnDisable()
    {
        if (receiveThread != null) receiveThread.Abort();
        client.Close();
    }
}
// *********************************************************
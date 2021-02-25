using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Network Add-ons
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

/// <summary>
/// Class made for online multiplayer testing purposes, not being used in the actual gameplay
/// </summary>
public class UDPMessages : MonoBehaviour
{
    public static UDPMessages Instance; //singelton

    LobbyListController lobbyList;
    InfoSender infoSender;

    public string userName;

    int port;
    string myIPAddress;
    IPAddress privateIPAddress;
    UdpClient server = null;
    UdpClient client = null;
    Socket s = null;
    IPEndPoint serverEndPoint;
    IPEndPoint clientEndPoint;

    Thread listenerThread;
    public bool isServer;

    public Transform clientSpawnPoint;
    public Transform serverSpawnPoint;
    public GameObject CharacterPrefab;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        isServer = false;
        port = 11000;
        lobbyList = GetComponent<LobbyListController>();
        infoSender = GetComponent<InfoSender>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartServer()
    {
        ConvertIpAddress();
        serverEndPoint = new IPEndPoint(privateIPAddress, 11010);
        server = new UdpClient(11010);
        listenerThread = new Thread(new ThreadStart(ListeningServer));
        listenerThread.IsBackground = true;
        listenerThread.Start();
        lobbyList.AddPlayer(userName);
        lobbyList.SetPlayersText();
        isServer = true;
    }

    public void SetIPAddress(string _IPAddress)
    {
        myIPAddress = _IPAddress;
    }
    public void SetUserName(string _userName)
    {
        userName = _userName;
    }
    private void ConvertIpAddress()
    {
        if (myIPAddress != "")
            privateIPAddress = IPAddress.Parse(myIPAddress);
    }

    public void StartClient()
    {
        ConvertIpAddress();
        clientEndPoint = new IPEndPoint(privateIPAddress, 11010);
        client = new UdpClient(port);
        client.Connect(clientEndPoint);
        SendMessageToServer("player:" + userName);
        listenerThread = new Thread(new ThreadStart(ListeningClient));
        listenerThread.IsBackground = true;
        listenerThread.Start();
    }

    //public void StartSocket()
    //{
    //    ConvertIpAddress();
    //    clientEndPoint = new IPEndPoint(privateIPAddress, port);
    //    s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    //    s.Connect(clientEndPoint);
    //    SendSocketMessage(userName);
    //}

    //public void SendSocketMessage(string message)
    //{
    //    byte[] sendbuf = Encoding.ASCII.GetBytes(message);
    //    s.SendTo(sendbuf, clientEndPoint);
    //    Console.WriteLine("Message sent to the broadcast address");
    //}

    public void SendMessageToServer(string message)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes(message);
        client.Send(sendBytes, sendBytes.Length);
    }

    public void SendMessageToClient(string message)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes(message);
        server.Send(sendBytes, sendBytes.Length, serverEndPoint);
    }

    private void ListeningServer()
    {
        try
        {
            while (true)
            {
                //Debug.Log("Waiting for broadcast");
                byte[] bytes = server.Receive(ref serverEndPoint);

                //Debug.Log($"Received broadcast from {serverEndPoint} :");
                string recieved = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                Debug.Log(recieved);
              
                if (recieved == "StartGame")
                {

                }
                else
                {
                    switch (GetInfoType(recieved))
                    {
                        case "player":
                            lobbyList.AddPlayer(GetInfoContent(recieved));
                            UnityMainThreadDispatcher.Instance().Enqueue(new Action(SetPlayersUI));
                            SendMessageToClient("players:" +lobbyList.GetPlayers());
                            UnityMainThreadDispatcher.Instance().Enqueue(new Action(SetPlayersUI));
                            break;
                        case "myPosition":
                            infoSender.notMyCharacterPos = JsonUtility.FromJson<Vector3>(GetInfoContent(recieved));
                            break;
                        case "myRotation":
                            infoSender.notMyCharacterRot = JsonUtility.FromJson<Vector3> (GetInfoContent (recieved));
                            break;
                        case "bullet":
                            string info = recieved.Substring (recieved.IndexOf (":") + 1); 
                            string pos = info.Substring (0, info.IndexOf ("_"));
                            string rot = info.Substring (info.IndexOf ("_") + 1);
                            CreatedObjectsManager.Instance.CreateBullet (pos,rot);
                            break;
                        case "hp":
                            InfoManager.Instance.ChangeNotMyHp (float.Parse (GetInfoContent (recieved)));
                            break;
                    }

                }
            }
        }
        catch (SocketException e)
        {
            Debug.Log(e);
        }
    }
    private void ListeningClient()
    {
        try
        {
            while (true)
            {
                //Debug.Log("Waiting for Server Messages");
                byte[] bytes = client.Receive(ref clientEndPoint);

                //Debug.Log($"Received broadcast from {clientEndPoint} :");
                string recieved = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                //Debug.Log(recieved);

                if(recieved == "StartGame")
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(new Action(HideCanvas));
                    UnityMainThreadDispatcher.Instance().Enqueue(new Action(SpawnCharacterClient));
                }
                else
                {
                    switch (GetInfoType(recieved))
                    {
                        case "players":
                            lobbyList.SetPlayers(GetInfoContent(recieved));
                            UnityMainThreadDispatcher.Instance().Enqueue(new Action(SetPlayersUI));
                            break;
                        case "myPosition":
                            infoSender.notMyCharacterPos = JsonUtility.FromJson<Vector3>(GetInfoContent(recieved));
                            break;
                        case "myRotation":
                            infoSender.notMyCharacterRot = JsonUtility.FromJson<Vector3> (GetInfoContent (recieved));
                            break;
                        case "bullet":
                            string info = recieved.Substring (recieved.IndexOf (":") + 1);
                            string pos = info.Substring (0, info.IndexOf ("_"));
                            string rot = info.Substring (info.IndexOf ("_") + 1);
                            CreatedObjectsManager.Instance.CreateBullet (pos, rot);
                            break;
                        case "hp":
                            InfoManager.Instance.ChangeNotMyHp (float.Parse (GetInfoContent (recieved)));
                            break;

                    }
                    
                }

            }
        }
        catch (SocketException e)
        {
            Debug.Log(e);
        }
    }
    public void HideCanvas()
    {
        lobbyList.HideCanvas();
    }
    public void SetPlayersUI()
    {
        lobbyList.SetPlayersText();
    }

    public void StartGame()
    {
        if (isServer)
        {
            lobbyList.HideCanvas();
            SendMessageToClient("StartGame");
            infoSender.SpawnCharacter(true);
        }
    }

    public void SpawnCharacterClient()
    {
        infoSender.SpawnCharacter(false);
    }

    public string GetInfoType(string s)
    {
        string stringBeforeChar = s.Substring(0, s.IndexOf(":"));
        //Debug.Log(stringBeforeChar);
        return stringBeforeChar;
    }
    public string GetInfoContent(string s)
    {
        string stringAfterChar = s.Substring(s.IndexOf(":") + 1);
        //Debug.Log(stringAfterChar);
        return stringAfterChar;
    }
    private void OnApplicationQuit()
    {
        if(server != null)
            server.Close();
        if(client != null)
            client.Close();
        if(s != null)
            s.Close();
    }
}

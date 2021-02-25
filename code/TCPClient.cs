using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

/// <summary>
/// Class made for online multiplayer testing purposes, not being used in the actual gameplay
/// </summary>
public class TCPClient : MonoBehaviour {

    TCPClient Instance; //singelton

    TcpClient client = null;
    Thread clientReceiveThread;
    public string customIpAddress;

    // Use this for initialization
    void Start () {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ConnectToTcpServer();
        }
	}
    public void SetCustomIp(string _customIp)
    {
        customIpAddress = _customIp;
    }
    public void StartClient() {
        ConnectToTcpServer();
    }
    private void ConnectToTcpServer() {
        try {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e) {
            Debug.Log("On client connect exception " + e);
        }
    }
    private void ListenForData() {
        try {
            Debug.Log("tryingtoConnect");
            client = new TcpClient(customIpAddress, 8888);
            //Byte[] bytes = new Byte[1024];
            //while (true) {
            //    // Get a stream object for reading 				
            //    using (NetworkStream stream = client.GetStream()) {
            //        int length;
            //        // Read incomming stream into byte arrary. 					
            //        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
            //            var incommingData = new byte[length];
            //            Array.Copy(bytes, 0, incommingData, 0, length);
            //            // Convert byte array to string message. 						
            //            string serverMessage = Encoding.ASCII.GetString(incommingData);
            //            Debug.Log("server message received as: " + serverMessage);
            //        }
            //    }
            //}
        }
        catch (SocketException socketException) {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    private void OnApplicationQuit() {
        if (client != null)
            client.Close();
    }
}

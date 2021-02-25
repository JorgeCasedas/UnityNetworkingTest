using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

/// <summary>
/// Class made for online multiplayer testing purposes, not being used in the actual gameplay
/// </summary>
public class TcpServer : MonoBehaviour {

    TcpListener server = null;
    List<TcpClient> clientsList = new List<TcpClient>();
    public int numberOfClients;
    Thread listener;
    public Text ipToText;
	// Use this for initialization
	void Start () {
        // StartCoroutine(StartServer());

    }
	
	// Update is called once per frame
	void Update () {

	}
    public void StartServer() {
        StartCoroutine(ServerInitiation());
    }
    IEnumerator ServerInitiation() {
        #region GetIP
        // ---------------- GET IP
        WWW myExtIPWWW = new WWW("http://checkip.dyndns.org");
        if (myExtIPWWW == null) {
            Debug.Log("webPageNotAvaliable");
            yield break;
        }
        yield return myExtIPWWW;
        string myExtIP = myExtIPWWW.text;
        myExtIP = myExtIP.Substring(myExtIP.IndexOf(":") + 1);
        myExtIP = myExtIP.Substring(0, myExtIP.IndexOf("<"));
        Debug.Log(myExtIP);
        //ipToText.text = myExtIP;
        #endregion
        #region StartServer with the previous ip

        // ---------------- START SERVER
        server = new TcpListener(IPAddress.Parse("25.31.75.159"), 8888);
        server.Start();
        listener = new Thread(new ThreadStart(ListeningForClients));
        listener.IsBackground = true;
        listener.Start();
        #endregion
    }
    private void ListeningForClients() {
        while (true) {
            Debug.Log("waiting for a client");
            TcpClient tempClient = server.AcceptTcpClient();  //if a connection exists, the server will accept it
            Debug.Log("I get a client");

            if (clientsList.Count == 0) {
                AddClientToList(tempClient);
            }      
            else {
                bool sameClient =true; //Default true, if any error occurs no client will be added

                foreach (TcpClient client in clientsList) {
                    Debug.Log("InForeach");
                    if (tempClient == client) {
                        sameClient = true;
                        break;
                    }
                    else {
                        sameClient = false;
                    }
                }

                Debug.Log("foreach over");
                if (!sameClient) 
                    AddClientToList(tempClient); 
            }   
        }
    }
    public void AddClientToList(TcpClient tempClient) {
        Debug.Log("GoingToAddClient");
        clientsList.Add(tempClient);
        numberOfClients++;
        Debug.Log("ClientAdded");
    }
    private void OnApplicationQuit() {
        if(server != null)
            server.Stop();
    }
}  


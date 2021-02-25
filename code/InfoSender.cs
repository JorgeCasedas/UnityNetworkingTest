using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class made for online multiplayer testing purposes, not being used in the actual gameplay
/// </summary>
public class InfoSender : MonoBehaviour
{
    public static InfoSender Instance;

    public float sendDelay = 0.1f;

    public Transform clientSpawnPoint;
    public Transform serverSpawnPoint;
    public GameObject characterPrefab;
    GameObject myCharacter;
    GameObject notMyCharacter;

    Vector3 myCharacterPos;
    Vector3 myCharacterRot;
    public Vector3 notMyCharacterPos;
    public Vector3 notMyCharacterRot;

    float notMyCharacterHp;

    bool matchStarted;
    UDPMessages _UDPMessages;
    private void Awake ()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy (this);
    }
    // Start is called before the first frame update
    void Start()
    {
        matchStarted = false;
        _UDPMessages = UDPMessages.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(matchStarted && notMyCharacter && myCharacter)
        {
            notMyCharacter.transform.position = notMyCharacterPos;
            notMyCharacter.transform.rotation = Quaternion.Euler(notMyCharacterRot);
            myCharacterPos = myCharacter.transform.position;
            myCharacterRot = myCharacter.transform.rotation.eulerAngles;
        } 
    }

    public void SpawnCharacter(bool isServer)
    {
        myCharacter = Instantiate(characterPrefab);
        notMyCharacter = Instantiate(characterPrefab);
        matchStarted = true;
        if (isServer)
        {
            myCharacter.transform.position = serverSpawnPoint.position;
            myCharacter.transform.rotation = serverSpawnPoint.rotation;
            myCharacter.name = "Server: Mine";

            notMyCharacter.transform.position = clientSpawnPoint.position;
            notMyCharacter.transform.rotation = clientSpawnPoint.rotation;
            notMyCharacter.name = "Server: NotMine";
            
            Destroy (notMyCharacter.GetComponent<ProgrammerMovement> ());
            Destroy (notMyCharacter.GetComponent<Skills> ());
            Destroy (notMyCharacter.transform.Find ("CamPivot").gameObject); 

            StartCoroutine (SendTransformToClient());
        }
        else
        {
            notMyCharacter.transform.position = serverSpawnPoint.position;
            notMyCharacter.transform.rotation = serverSpawnPoint.rotation;
            notMyCharacter.name = "Client: NotMine";

            Destroy (notMyCharacter.GetComponent<ProgrammerMovement> ());
            Destroy (notMyCharacter.GetComponent<Skills> ());
            Destroy (notMyCharacter.transform.Find ("CamPivot").gameObject);

            myCharacter.transform.position = clientSpawnPoint.position;
            myCharacter.transform.rotation = clientSpawnPoint.rotation;
            myCharacter.name = "Client: Mine";

            StartCoroutine (SendTransformToServer());
        }  
    }

    IEnumerator SendTransformToClient()
    {
        while (matchStarted)
        {
            string json = JsonUtility.ToJson(myCharacterPos);
            UDPMessages.Instance.SendMessageToClient("myPosition:" + json);
            json = JsonUtility.ToJson (myCharacterRot);
            UDPMessages.Instance.SendMessageToClient ("myRotation:" + json);
            yield return null;
        }
    }

    IEnumerator SendTransformToServer()
    {
        while (matchStarted)
        {
            string json = JsonUtility.ToJson(myCharacterPos);
            UDPMessages.Instance.SendMessageToServer("myPosition:" + json);
            json = JsonUtility.ToJson (myCharacterRot);
            UDPMessages.Instance.SendMessageToServer ("myRotation:" + json);
            yield return null;
        }
    }

    public void SpawnBullet (Vector3 pos, Vector3 rot)
    {
        string json = JsonUtility.ToJson (pos) + "_" + JsonUtility.ToJson (rot);
        if (_UDPMessages.isServer)
        { 
            UDPMessages.Instance.SendMessageToClient ("bullet:" + json);
        }
        else
        {
            UDPMessages.Instance.SendMessageToServer ("bullet:" + json);
        }
    }

    public void SendMyHp (float hp)
    {
        string json = hp.ToString ();
        if (_UDPMessages.isServer)
        {
            UDPMessages.Instance.SendMessageToClient ("hp:" + json);
        }
        else
        {
            UDPMessages.Instance.SendMessageToServer ("hp:" + json);
        }
    }

    ////////// FOR THE FUTURE //////////////
   /* 
    public void SendMessage (string tag, string message)
    {
        string json = tag + ":" + message;
        if (_UDPMessages.isServer)
        {
            UDPMessages.Instance.SendMessageToClient (json);
        }
        else
        {
            UDPMessages.Instance.SendMessageToServer (json);
        }
    }
    */
    public void SetEnemysPos(Vector3 pos)
    {
        notMyCharacterPos = pos;
    }

    public void SetEnemysRot(Vector3 rot)
    {
        notMyCharacterRot = rot;
    }
}

using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

/// <summary>
/// Class made for testing purposes, not being used in the actual gameplay
/// </summary>
public class ProgrammerMovement : MonoBehaviour {

    public GameObject camPivot;
    //public GameObject body;
    Rigidbody rb;
    public float speed;
    public float rotSpeed;

    float yRot;
    float xRot;

    bool w;
    bool s;
    bool d;
    bool a;

    private void Awake ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Use this for initialization
    void Start () 
    {
        rb = GetComponent<Rigidbody>();
        xRot = transform.rotation.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 charDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) {
            charDir += transform.forward;
            //if ((a && !d) || (d && !a)) {
            //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y,  speed * Mathf.Sqrt(0.5f));
            //}
            //else
            //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
            //w = true;
        }
        if (Input.GetKey(KeyCode.S)) {
            charDir -= transform.forward;
            //if ((a && !d) || (d && !a)) {
            //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -speed * Mathf.Sqrt(0.5f));
            //}
            //else
            //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -speed);
            //s = true;
        }   
        if (Input.GetKey(KeyCode.A)){
            charDir -= transform.right;
            //if ((s && !w) || (w && !s)) {
            //    rb.velocity = new Vector3(-speed * Mathf.Sqrt(0.5f), rb.velocity.y, rb.velocity.z);
            //}
            //else
            //    rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z);
            //a = true;
        }
        if (Input.GetKey(KeyCode.D)) {
            charDir += transform.right;
            //if ((s && !w) || (w && !s)) {
            //    rb.velocity = new Vector3(speed * Mathf.Sqrt(0.5f), rb.velocity.y, rb.velocity.z);
            //}
            //else
            //    rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
            //d = true;
        }

        //if (Input.GetKeyUp(KeyCode.S))
        //{
        //    s = false;
        //}
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    w = false;
        //}
        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    a = false;
        //}
        //if (Input.GetKeyUp(KeyCode.D))
        //{
        //    d = false;
        //}

        charDir = charDir.normalized* speed;
        rb.velocity = charDir;
        yRot += Input.GetAxis ("Mouse Y") * rotSpeed;
        yRot = Mathf.Clamp (yRot, -80, 50);
        xRot += Input.GetAxis ("Mouse X") * rotSpeed;
        
        transform.rotation = Quaternion.Euler (new Vector3 (0, xRot , 0));
        if(camPivot)
            camPivot.transform.localRotation = Quaternion.Euler (new Vector3 (-yRot, 0, 0));
        //Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //    body.transform.LookAt(new Vector3(hit.point.x, body.transform.position.y, hit.point.z));

    }


}

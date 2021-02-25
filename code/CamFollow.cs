using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class made for testing purposes, not being used in the actual gameplay
/// </summary>
public class CamFollow : MonoBehaviour {

    public GameObject tarjet;
    Vector3 initialPos;
    Vector3 tarjetInitialPos;

	// Use this for initialization
	void Start () {
        initialPos = transform.position;
        tarjetInitialPos = tarjet.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = initialPos + (tarjet.transform.position - tarjetInitialPos);
	}
}

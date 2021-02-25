using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class made for testing purposes, not being used in the actual gameplay
/// </summary>
public class Bullet : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

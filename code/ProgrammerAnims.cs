using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class made for testing purposes, not being used in the actual gameplay
/// </summary>
public class ProgrammerAnims : MonoBehaviour {

    public GameObject handPc;
    public GameObject backPc;
    Animator anim;
    Rigidbody rb;
    public bool running;
    public bool pcOn;
    public float transitionSpeed;
    bool transitioning;
    float transition;
    public float magnitude;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        magnitude = rb.velocity.magnitude;

        AnimWeight();

        if (transitioning) {
            transition -= Time.deltaTime * transitionSpeed;
            anim.SetLayerWeight(1, transition);
            if (transition <= 0) {
                transitioning = false;
            }
        }

        if (rb.velocity.magnitude > 10f) {
            running = true;
        }
        else {
            running = false;
        }
        if (running) {
            anim.SetBool("run", true);
        }
        else {
            anim.SetBool("run", false);
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (pcOn) {
                anim.SetBool("pcOn", false);
                pcOn = false;
                if (!transitioning && anim.GetLayerWeight(1) > 0.5f) {
                    transitioning = true;
                    transition = 1;
                }
            }
            else {
                anim.SetBool("pcOn", true);
                pcOn = true;
                
                //anim.SetLayerWeight(1, 1);
            }
        }
	}

    public void AnimWeight() {
        //if (anim.GetCurrentAnimatorStateInfo(1).IsName("Pc0") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1.0f) {
        //    if (!transitioning && anim.GetLayerWeight(1)>0.5f) {
        //        transitioning = true;
        //        transition = 1;
        //    }

        //}
        //else 
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Pc0"))
            anim.SetLayerWeight(1, 1);

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Pc") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.6f) {
            handPc.SetActive(true);
            backPc.SetActive(false);
        }
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Pc0") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.4f) {
            handPc.SetActive(false);
            backPc.SetActive(true);
        }

    }

}

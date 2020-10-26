using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeBasedDetailButton : MonoBehaviour {

    public GameObject TextDescription;

    private bool lookedAt = false;

    private float TimeStamp;

    public float Delay = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void IsLookedAt()
    {
        // reset after gaze was not directed to the button
        // switch back mechanism for the image
        if (!this.lookedAt)
        {
            this.TimeStamp = Time.realtimeSinceStartup;
            this.lookedAt = true;
           // this.TextDescription.SetActive(false);
            // just for debugging
            Color c = this.gameObject.GetComponent<MeshRenderer>().material.color;
            c.r = 1.0f;
            c.g = 0.0f;
            c.b = 0.0f;
            this.gameObject.GetComponent<MeshRenderer>().material.color = c;
        }
        else
        {
            if (Time.realtimeSinceStartup - this.TimeStamp > this.Delay)
            {
                // this.TextDescription.SetActive(!this.TextDescription.activeSelf);
                this.TextDescription.SetActive(!this.TextDescription.activeSelf);
                this.TimeStamp = Time.realtimeSinceStartup;
                /*
                Color w = this.gameObject.GetComponent<MeshRenderer>().material.color;
                w.r = 1.0f;
                w.g = 1.0f;
                w.b = 1.0f;
                this.gameObject.GetComponent<MeshRenderer>().material.color = w;
                */
            }
        }

        //Debug.Log("hit on button...");
    }

    public void FocusLost()
    {
        this.TimeStamp = -1.0f;
        this.lookedAt = false;
        // 
        Color c = this.gameObject.GetComponent<MeshRenderer>().material.color;
        c.r = 1.0f;
        c.g = 1.0f;
        c.b = 1.0f;
        this.gameObject.GetComponent<MeshRenderer>().material.color = c;
    }
}

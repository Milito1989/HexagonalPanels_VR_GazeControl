using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRaycast : MonoBehaviour {

    public Camera RaycastCamera;
    
    public bool RaycastActive = false;

    private bool TriggerReleased;

    private GameObject lastSelectedObject;

    void Start()
    {
        ControllerPointer.TriggerEvent += this.TriggerCallback;
    }

    // Update is called once per frame
    void LateUpdate () {
		if (this.RaycastActive && this.TriggerReleased)
        {
            Ray camerRay = new Ray(this.RaycastCamera.transform.position, this.RaycastCamera.transform.forward);
            RaycastHit hitInfo;

            Debug.DrawRay(this.RaycastCamera.transform.position, this.RaycastCamera.transform.forward, Color.red, 1.0f);
           // I guess here is where i need to activate correlated text's to each image


            if (Physics.Raycast(camerRay, out hitInfo))
            {
                Debug.Log("head raycast hit on: " + hitInfo.collider.gameObject.name);

           //     GameObject details = GameObject.Instantiate(this.DetailsPrefab);

                if (hitInfo.collider.gameObject.GetComponent<CubeDescription>())
                {
                    hitInfo.collider.gameObject.GetComponent<CubeDescription>().HandleHit();
                }


                // requires check whether selection changed, if it changed and last object was a GazeBasedDetailButton FocusLost has to be called
                this.lastSelectedObject = hitInfo.collider.gameObject;
            }
            else
            {
                if (this.lastSelectedObject != null)
                {
                    if (this.lastSelectedObject.GetComponent<GazeBasedDetailButton>())
                    {
                        this.lastSelectedObject.GetComponent<GazeBasedDetailButton>().FocusLost();
                    }
                }
                this.lastSelectedObject = null;
            }

            this.TriggerReleased = false;
        }
        else if (this.RaycastActive && !this.TriggerReleased)
        {
            Ray camerRay = new Ray(this.RaycastCamera.transform.position, this.RaycastCamera.transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(camerRay, out hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<GazeBasedDetailButton>())
                {
                    hitInfo.collider.gameObject.GetComponent<GazeBasedDetailButton>().IsLookedAt();
                }
                // requires check whether selection changed, if it changed and last object was a GazeBasedDetailButton FocusLost has to be called
                this.lastSelectedObject = hitInfo.collider.gameObject;
            }
            else
            {
                if (this.lastSelectedObject != null)
                {
                    if (this.lastSelectedObject.GetComponent<GazeBasedDetailButton>())
                    {
                        this.lastSelectedObject.GetComponent<GazeBasedDetailButton>().FocusLost();
                    }
                }
                this.lastSelectedObject = null;
            }
        }

    }

    private void TriggerCallback()
    {
        Debug.Log("Trigger released...");
        if (this.RaycastActive) this.TriggerReleased = true;
    }
}

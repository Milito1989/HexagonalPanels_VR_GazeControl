using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using UnityEngine;
using Valve.VR;

public class PanelsScript : MonoBehaviour
{

    Animator animator;
    //private setter to use "Auto Property" feature to make it e.g. read-only and accessible , Here : doorOpen 
    public bool doorOpen { get; private set; }
    //private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    //private SteamVR_TrackedObject[] trackedObj;



    void Start()
    {
        doorOpen = false;
        animator = GetComponent<Animator>();
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
        //Debug.Log("trackedObj: " + trackedObj);
    }

    void Update()
    {
    //    this.trackedObj = GameObject.FindObjectsOfType<SteamVR_TrackedObject>();
        /*
        for (int i = 0; i < this.trackedObj.Length; i++)
        {
            Debug.Log(this.trackedObj[i].gameObject.name);
        }
        */
    }



    void OnTriggerEnter(Collider col)
    {


        Debug.Log(col.gameObject.tag + " , " + col.gameObject.name + " , " + (col.gameObject.GetComponent<SteamVR_TrackedObject>() ? "attached" : "missing script"));
        if (col.gameObject.GetComponent<SteamVR_TrackedObject>())
        {
            SteamVR_TrackedObject trackedObject = col.gameObject.GetComponent<SteamVR_TrackedObject>();
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)trackedObject.index);
            
            Debug.Log(controller.GetState().ulButtonPressed + " , " + controller.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger) + " , " + controller.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger));
        }

        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<SteamVR_TrackedObject>())
        {
            SteamVR_TrackedObject trackedObject = col.gameObject.GetComponent<SteamVR_TrackedObject>();

            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)trackedObject.index);

            if (controller.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger))
            {
                doorOpen = true;
                DoorsAnim("Openning");
            }
        }

        //if (col.gameObject.tag == "Player" && triggerButtonDown)
        //{
        //    doorOpen = true;
        //    DoorsAnim("Openning");
        //}
    }

    public void switchDoorState()
    {
        if (doorOpen)
        {
           // Debug.Log("call true");
            doorOpen = false;
            DoorsAnim("Closing");
            if (this.GetComponentInChildren<PanelContainer>())
            {
               this.GetComponentInChildren<PanelContainer>().SwitchColliderState(false);
            }
        }
        else
        {
            //Debug.Log("call false");
            doorOpen = true;
            DoorsAnim("Openning");
            if (this.GetComponentInChildren<PanelContainer>())
            {
                this.GetComponentInChildren<PanelContainer>().SwitchColliderState(true);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        /*
        if (doorOpen)
        {
            doorOpen = false;
            DoorsAnim("Closing");
        }
        */
    }
    void DoorsAnim(string direction)
    {
        animator.SetTrigger(direction);
    }
}




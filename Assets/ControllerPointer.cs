using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class ControllerPointer : MonoBehaviour {
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
    private GameObject lastHit;
    public GameObject LineVisualizer;

    public delegate void TriggerEventHandler();
    public static event TriggerEventHandler TriggerEvent;

    // public UnityEngine.UI.Text description;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        this.lastHit = null;
        this.LineVisualizer.GetComponent<MeshRenderer>().enabled = false;

    /*    if (this.description == null)
        {
            this.description = this.gameObject.GetComponentInChildren<Text>();
        }*/
     //   this.description.gameObject.SetActive(false);

    }


    // Update is called once per frame
    void Update () {
        if (controller.GetPress(EVRButtonId.k_EButton_ApplicationMenu))
        {
            this.LineVisualizer.GetComponent<MeshRenderer>().enabled = true;

            Debug.Log("menu button pressed...");
            Ray forwardRay = new Ray(this.transform.position, this.transform.forward);
            Debug.DrawRay(this.transform.position, this.transform.forward);
            RaycastHit info = new RaycastHit();
            if (Physics.Raycast(forwardRay, out info))
            {
                Debug.Log(info.collider.gameObject.name);

                if (info.collider.gameObject.GetComponent<PanelControl>())
                {
                    info.collider.gameObject.GetComponent<PanelControl>().HandleHit(controller);
                }
                else if (info.collider.gameObject.GetComponent<CubeDescription>())
                {
                    info.collider.gameObject.GetComponent<CubeDescription>().HandleHit();//.description.gameObject.SetActive(!info.collider.gameObject.GetComponent<CubeDescription>().description.gameObject.activeSelf);
                }

                if (info.collider.gameObject.GetComponent<PanelsScript>())
                {
                    if (this.lastHit == null)
                    {
                        //info.collider.gameObject.GetComponent<PanelsScript>().switchDoorState();
                        if (info.collider.gameObject.GetComponent<PanelsScript>().doorOpen)
                        {
                            if (controller.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger))
                            {
                                info.collider.gameObject.GetComponent<PanelsScript>().switchDoorState();
                            }
                        }
                        else
                        {
                            info.collider.gameObject.GetComponent<PanelsScript>().switchDoorState();
                        }
                    }
                    else if (this.lastHit != null)
                    {
                        if (info.collider.gameObject == this.lastHit)
                        {
                            // just holding do nothing
                        }
                    }
                    this.lastHit = info.collider.gameObject;
                }
                else if (info.collider.gameObject.GetComponent<AbstractPanelItem>())
                {
                    info.collider.gameObject.GetComponent<AbstractPanelItem>().SwitchSelectionState();
                    if (info.collider.gameObject.GetComponent<CubeDescription>()) info.collider.gameObject.GetComponent<CubeDescription>().description.gameObject.SetActive(!info.collider.gameObject.GetComponent<CubeDescription>().description.gameObject.activeSelf);
                 //   this.description.gameObject.SetActive(!this.description.gameObject.activeSelf);
                }
            }
            else
            {
                if (this.lastHit != null)
                {
                    if (this.lastHit.gameObject.GetComponent<PanelsScript>())
                    {
                        if (this.lastHit.GetComponent<PanelsScript>().doorOpen)
                        {
                            if (controller.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger))
                            {
                                this.lastHit.GetComponent<PanelsScript>().switchDoorState();
                            }
                        }
                        else
                        {
                            this.lastHit.GetComponent<PanelsScript>().switchDoorState();
                        }
                    }
                }
                this.lastHit = null;
            }
        }
        else
        {
            this.LineVisualizer.GetComponent<MeshRenderer>().enabled = false;
            this.lastHit = null;
        }

        if (controller.GetPressUp(EVRButtonId.k_EButton_SteamVR_Trigger))
        {
            ControllerPointer.TriggerEvent();
        }
    }
}

using UnityEngine;
using System.Collections;
using Valve.VR;

public class WandController : MonoBehaviour
{
    private EVRButtonId triggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerButtonDown = false;
    public bool triggerButtonUp = false;
    public bool triggerButtonPressed = false;

    private EVRButtonId gripButton = EVRButtonId.k_EButton_Grip;
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;
    public bool gripButtonPressed = false;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        Debug.Log("trackedObj: " + trackedObj);                   //trackedObj is never null
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null)
        {
            Debug.Log("Controler Object ist null");             // controller object is never null
            return;
        }


        gripButtonDown = controller.GetPressDown(gripButton);   //is always false, also when pressing gripButton
        gripButtonUp = controller.GetPressUp(gripButton);
        gripButtonPressed = controller.GetPress(gripButton);



        triggerButtonDown = controller.GetPressDown(triggerButton);
        triggerButtonUp = controller.GetPressUp(triggerButton);
        triggerButtonPressed = controller.GetPress(triggerButton);
        Debug.Log("triggerButton: " + controller.GetPress(triggerButton));  //is always false, also when pressing triggerButton
        Debug.Log("triggerButtonDown: " + triggerButtonDown);              //is always false, also when pressing triggerButton


        //content of these if-statements never get activated/run -> also when pressing all Buttons permanently ! 
        if (gripButtonDown)
        {
            Debug.Log("Grip Button was just pressed");
        }


        if (gripButtonUp)
        {
            Debug.Log("Grip Button was just unpressed");
        }


        if (triggerButtonDown)
        {
            Debug.Log("Trigger Button was just pressed");
        }


        if (triggerButtonUp)
        {
            Debug.Log("Trigger Button was just unpressed");
        }

    }
}
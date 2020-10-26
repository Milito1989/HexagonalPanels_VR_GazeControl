using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeDescription : MonoBehaviour {

    public UnityEngine.UI.Text description;
    public GameObject button;
    public GameObject ImageFrame;

    public SampleData data;

    private Vector3 backupScale;

    void Start () {
        /*
        if (this.description == null)
        {
            this.description = this.gameObject.GetComponentInChildren<Text>();
        }
        */
        this.description.gameObject.SetActive(false);
        this.backupScale = this.transform.localScale;
    }
	
	
    void Update () {
	    if (Input.GetKeyUp(KeyCode.Space))
        {
            this.description.gameObject.SetActive(!this.description.gameObject.activeSelf);
        }	
	}

    public void HandleHit()
    {
        if (this.inTransit) return;
        // are we inside a container?
        PanelControl panel = this.gameObject.GetComponentInParent<PanelControl>();
        if (panel)
        {
            if (panel.PanelState == PanelControl.PanelStates.IN_FRONT)
            {

                foreach (CubeDescription description in panel.GetComponentsInChildren<CubeDescription>())
                {
                    if (description != this) description.description.gameObject.SetActive(false);
                    if (description != this) description.button.gameObject.SetActive(false);
                    if (description != this) description.ImageFrame.gameObject.SetActive(false);
                }

                StartCoroutine(this.componentTransition());

                //this.description.gameObject.SetActive(!this.description.gameObject.activeSelf);
                /*
                foreach(PanelControl otherPanel in GameObject.FindObjectsOfType<PanelControl>())
                {
                    if (otherPanel != panel)
                    {
                        otherPanel.SwitchSubComponentState(this.gameObject.name, this.description.gameObject.activeSelf);
                    }
                }
                */
            }
        }
        else
        {
            this.description.gameObject.SetActive(!this.description.gameObject.activeSelf);
            this.button.gameObject.SetActive(!this.button.gameObject.activeSelf);
            this.ImageFrame.gameObject.SetActive(!this.ImageFrame.gameObject.activeSelf);
        }
    }

    private bool inTransit = false;

    private IEnumerator componentTransition()
    {
        this.inTransit = true;

        GameObject component = this.gameObject;
        bool value = !component.GetComponent<CubeDescription>().description.gameObject.activeSelf;

        Vector3 targetScale = value ? component.transform.localScale * 2.0f : component.transform.localScale * 0.5f;

        while (Vector3.Distance(targetScale, component.transform.localScale) > .1f)
        {
            Vector3 currentScale = component.transform.localScale;
            component.transform.localScale = Vector3.Lerp(currentScale, targetScale, 0.01f);
            yield return 5;
        }

        component.GetComponent<CubeDescription>().description.gameObject.SetActive(value);
        component.GetComponent<CubeDescription>().button.gameObject.SetActive(value);
        component.GetComponent<CubeDescription>().ImageFrame.gameObject.SetActive(value);

        PanelControl panel = this.gameObject.GetComponentInParent<PanelControl>();

        foreach (PanelControl otherPanel in GameObject.FindObjectsOfType<PanelControl>())
        {
            if (otherPanel != panel)
            {
                if (this.data.clustername != null)
                {
                    otherPanel.SwitchSubComponentState(this.gameObject.name, this.description.gameObject.activeSelf, this.data.clustername);
                }
                else
                {
                    otherPanel.SwitchSubComponentState(this.gameObject.name, this.description.gameObject.activeSelf);
                }

                
            }
            // add check for subcomponents in current panel as well
        }

        this.inTransit = false;

        yield return null;
    }

    public void RevertScale()
    {
        this.transform.localScale = this.backupScale;
    }
}

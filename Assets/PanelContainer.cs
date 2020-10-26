using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelContainer : MonoBehaviour {

    private Collider[] PanelObjects;

	// Use this for initialization
	void Start () {
        //Following gets all colliders of including children of this component(here Panel frame)
        this.PanelObjects = this.transform.GetComponentsInChildren<Collider>();

        foreach (Collider col in this.PanelObjects)
        {
            col.enabled = false;
        }
	}
	
    public void SwitchColliderState(bool activate)
    {
        foreach (Collider col in this.PanelObjects)
        {
            col.enabled = activate;
        }

        if (!activate)
        {
            foreach (AbstractPanelItem item in this.transform.GetComponentsInChildren<AbstractPanelItem>())
            {
                item.Selected = false;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}

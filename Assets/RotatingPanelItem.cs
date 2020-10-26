using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPanelItem : AbstractPanelItem {
    
    public override void SwitchSelectionState()
    {
        base.SwitchSelectionState();
        this.Selected = !this.Selected;
    }

    private void Update()
    {
        if (this.Selected)
        {
            this.transform.Rotate(Vector3.up, Time.deltaTime * 20.0f);
        }
    }
}

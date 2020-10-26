using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalingPanelItem : AbstractPanelItem {

    private Vector3 initialScale;

    private Vector3 maxScale;

    //public UnityEngine.UI.Text description;


    private void Start()
    {
       /* if (this.description == null)
        {
            this.description = this.gameObject.GetComponentInChildren<Text>();
        }*/
      //  this.description.gameObject.SetActive(false);
        this.initialScale = this.transform.localScale;
        this.maxScale = 1.5f * this.initialScale;
    }

    public override void SwitchSelectionState()
    {
        base.SwitchSelectionState();
        this.Selected = !this.Selected;
    }

    private void Update()
    {
        if (this.Selected)
        {
            Vector3 cScale = Vector3.Lerp(this.initialScale, this.maxScale, (Time.frameCount % 100 / 100.0f));
            this.transform.localScale = cScale;

        /*    if (Input.GetKeyUp(KeyCode.Space))
            {
                this.description.gameObject.SetActive(!this.description.gameObject.activeSelf);
            }*/
        }
    }
}

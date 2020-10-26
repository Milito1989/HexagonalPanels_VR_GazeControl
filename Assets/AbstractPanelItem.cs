using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractPanelItem : MonoBehaviour {

	public bool Selected { get; set;  }

    public virtual void SwitchSelectionState() { }
}

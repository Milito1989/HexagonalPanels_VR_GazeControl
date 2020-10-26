using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContentPositioning : MonoBehaviour {
    public virtual void LayoutContent(GameObject[] components, PanelControl container) { }
    public virtual void LayoutContent(GameObject component, int index, PanelControl container) { }
}

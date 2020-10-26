using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourItemLayoutPositioning : ContentPositioning
{

    public override void LayoutContent(GameObject[] components, PanelControl container)
    {
        if (components.Length != 4)
        {
            Debug.LogWarning("number of compenents is not equal to 4...");
        }

        MeshRenderer renderer = container.GetComponent<MeshRenderer>();

        Vector3 center = renderer.bounds.center;
        Vector3 extents = renderer.bounds.extents;

        Vector3[] positions = new Vector3[]
        {
            center - extents,
            center + extents - new Vector3(0.0f, extents.y, 0.0f),
            center + extents - new Vector3(extents.x, 0.0f, 0.0f),
            center + extents
        };

        for (int i = 0; i < positions.Length; i++)
        {
            components[i].transform.position = positions[i];
        }
    }

    public override void LayoutContent(GameObject component, int index, PanelControl container)
    {

        MeshRenderer renderer = container.GetComponent<MeshRenderer>();

        Vector3 center = renderer.bounds.center;
        Vector3 extents = renderer.bounds.extents;

        Vector3[] positions = new Vector3[]
        {
            center - extents * .5f + new Vector3(0.0f, 0.0f, 2.0f * extents.z * .5f), // lower right
            center + extents * .5f - new Vector3(0.0f, 2.0f * extents.y * .5f, 0.0f), // upper right
            center + extents * .5f - new Vector3(2.0f * extents.x * .5f, 0.0f, 0.0f), // lower left
            center + extents * .5f // upper left
        };

        component.transform.position = positions[index];
    }
}

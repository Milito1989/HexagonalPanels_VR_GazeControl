using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLayout : MonoBehaviour {

    public GameObject Prefab;

    public int NumberOfObjects = 20;

    private List<float> ages;
    private List<GameObject> components;

	// Use this for initialization
	void Start () {
        this.ages = new List<float>();
        this.components = new List<GameObject>();

        Vector3 extents = this.GetComponent<Renderer>().bounds.extents;

        for (int i = 0; i < this.NumberOfObjects; i++)
        {
            float age = 0.0f;
            if (Random.value > .5f)
            {
                age = Random.Range(0.0f, 0.5f);
            }
            else
            {
                age = Random.Range(0.5f, 1.0f);
            }

            this.ages.Add(age);
            GameObject comp = GameObject.Instantiate(this.Prefab, null);
            comp.transform.position = this.transform.position;

            float x = this.transform.position.x - extents.x * .5f +  extents.x * this.ages[i];
            comp.transform.position = new Vector3(x, comp.transform.position.y - extents.y, comp.transform.position.z - .1f);

            foreach (GameObject obj in this.components)
            {
                int j = 0;
                while (comp.GetComponent<Renderer>().bounds.Intersects(obj.GetComponent<Renderer>().bounds))
                {
                    comp.transform.position = comp.transform.position + new Vector3(0.0f, /*Mathf.Abs(comp.transform.position.y - obj.transform.position.y)*/comp.GetComponent<Renderer>().bounds.extents.y, 0.0f);
                    j++;
                    if (j >= 20)
                    {
                        break;
                    }
                }
            }

            this.components.Add(comp);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IOSample2 : MonoBehaviour {

    public GameObject panel;

    public List<GameObject> observedObjects;

    private SampleData[] data;

    private Dictionary<string, GameObject> dataMap;

	// Use this for initialization
	void Start ()
    {

        this.observedObjects = new List<GameObject>();

        TextAsset jsonData = (TextAsset) Resources.Load("JSONStuff/Sample");
        
        string txt = jsonData.text;

        string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        this.data = new SampleData[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            //UnityEngine.Debug.Log(lines[i]);
            this.data[i] = JsonUtility.FromJson<SampleData>(lines[i]);
            UnityEngine.Debug.Log(this.data[i].ID);
        }

        this.dataMap = new Dictionary<string, GameObject>();
        Material textureMaterial = Resources.Load("DefMaterial") as Material;

        for (int i = 0; i < this.data.Length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = new Vector3(0.0f, 1.0f, 10.0f);
            var rend = go.GetComponent<Renderer>();
            rend.material = Instantiate(textureMaterial) as Material;
            //rend.material = textureMaterial;
            //textureMaterial.mainTexture = Resources.Load("Images/" + this.data[i].fileName) as Texture;
            rend.material.mainTexture = Resources.Load("Images/" + this.data[i].fileName) as Texture;
            go.transform.localScale = new Vector3(this.data[i].width, this.data[i].height, 1.0f);
            go.name = this.data[i].ID;

            this.observedObjects.Add(go);

            this.dataMap.Add(data[i].ID, go);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (this.panel.GetComponent<Renderer>().bounds.Contains(this.observedObjects[0].transform.position))
        {
            UnityEngine.Debug.Log("object in range...");

        }
        else
        {
            UnityEngine.Debug.Log("object lost...");

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            for (int i = 0; i < this.data.Length; i++)
            {
                if (this.dataMap.ContainsKey(this.data[i].ID))
                {
                    GameObject obj = null;
                    this.dataMap.TryGetValue(this.data[i].ID, out obj);

                    Vector3 center = this.panel.GetComponent<Renderer>().bounds.center;

                    switch (this.data[i].epoch)
                    {
                        case "North America":
                            obj.transform.position = center + this.panel.GetComponent<Renderer>().bounds.extents;
                            break;
                        case "Animalia":
                            obj.transform.position = center - this.panel.GetComponent<Renderer>().bounds.extents;
                            break;
                        default:
                            break;

                    }
                }
            }
        }
    }
}

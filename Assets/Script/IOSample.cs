using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IOSample : MonoBehaviour {

    private SampleData[] data;

	// Use this for initialization
	void Start () {
        this.data = new SampleData[3];

        this.data[0] = new SampleData();
        this.data[0].ID = "anatolian_shepherd_dog";
        this.data[0].fileName = "anatolian_shepherd_dog.jpg";
        this.data[0].epoch = "North America";
        this.data[0].width = 1;
        this.data[0].height = 1;

        this.data[1] = new SampleData();
        this.data[1].ID = "booby";
        this.data[1].fileName = "booby.jpg";
        this.data[1].epoch = "Animalia";
        this.data[1].width = 3;
        this.data[1].height = 1;

        this.data[2] = new SampleData();
        this.data[2].ID = "highland_cattle";
        this.data[2].fileName = "highland_cattle.jpg";
        this.data[2].epoch = "Mountainous and wet grasslands";
        this.data[2].width = 2;
        this.data[2].height = 1;

        string json = JsonUtility.ToJson(this.data[0]);
        string json2 = JsonUtility.ToJson(this.data[1]);
        string json3 = JsonUtility.ToJson(this.data[2]);

        Debug.Log(json);
        Debug.Log(json2);
        Debug.Log(json3);

        SampleData newData = JsonUtility.FromJson<SampleData>(json);
        


        Debug.Log(newData.ID);
    }

    // Update is called once per frame
    void Update () {
		
	}
}

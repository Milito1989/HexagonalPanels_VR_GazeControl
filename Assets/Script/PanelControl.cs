using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(Collider))]
public class PanelControl : MonoBehaviour {

    public Animator animator;

    //Existed in Perfab Folder , This will be the descriptive texts
    public GameObject DescriptionPrefab;
    public GameObject ImageFrame;
    public GameObject DetailsPrefab;
    public GameObject ButtonPrefab;

    public TextAsset JSONFile;

    public enum PanelStates
    {
        IN_FRONT,
        IN_BACKGROUND,
        IN_TRANSITION   //Is also used for detecting collider's collision
    }

    public PanelStates PanelState { get; private set; }

    private Dictionary<string, GameObject> SubComponents;

	void Start () {
        // just for testing...
        this.backupScale = this.transform.localScale;


        this.PanelState = PanelStates.IN_BACKGROUND;

        Vector3 backupPosition = this.transform.position;
        Quaternion backupRotation = this.transform.rotation;

        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;

        string txt = this.JSONFile.text;

        string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        // Make sure there are no empty lines in the json file...
        // It reads from Assets>>Resources>>JSONStuff and the file name is SampleData
        SampleData[] data = new SampleData[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            //keep in mind that ids are not unique at the moement, you can have for instance multiple instance of 'anatolian_sheperd_dog' in a single panel
            data[i] = JsonUtility.FromJson<SampleData>(lines[i]);
            //UnityEngine.Debug.Log(lines[i]);
            //UnityEngine.Debug.Log("Lines Are: " + lines[i]);
        }

        this.SubComponents = new Dictionary<string, GameObject>();
        Material textureMaterial = Resources.Load("DefMaterial") as Material;

        // an example for random placement on certain predefined positions
        float xOffset = .5f;
        float yOffset = .5f;

        List<Vector3> positions = new List<Vector3>();
        Vector3 scale = this.transform.localScale;
        float scalingFactor = .25f;


        for (float x = this.transform.position.x - scale.x * .5f + xOffset; x < this.transform.position.x + scale.x * .5f; x += 1.0f * scalingFactor)
        {
            for (float y = this.transform.position.y - scale.y * .5f + yOffset; y < this.transform.position.y + scale.y * .5f; y += 1.0f * scalingFactor)
            {
                positions.Add(new Vector3(x, y, 0.1f));
            }
        }

        //Following part is for rendering the images on cubes surrounding the panel(door)
      
        for (int i = 0; i < data.Length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = this.transform.position;
            go.transform.rotation = this.transform.rotation;
            var rend = go.GetComponent<Renderer>();
            rend.material = Instantiate(textureMaterial) as Material;
            UnityEngine.Debug.Log(data[i].fileName);
            Texture tex = Resources.Load("Images/" + data[i].fileName) as Texture;//From Resources folder (Resources.) ...
            Debug.Log("Dim for " + data[i].fileName + " : " + tex.height + " x " + tex.width);
            if (tex != null) rend.material.mainTexture = tex;
            //go.transform.localScale = new Vector3(data[i].width * scalingFactor, data[i].height * scalingFactor, 1.0f);
            float baseScale = Mathf.Min(tex.width, tex.height);
            go.transform.localScale = new Vector3(tex.width / baseScale * scalingFactor, tex.height / baseScale * scalingFactor, 0.1f);
            go.transform.parent = this.transform;

            int index = UnityEngine.Random.Range(0, positions.Count);
            go.transform.position = positions[index];
            positions.RemoveAt(index);

            go.name = data[i].ID;
            
            if (this.GetComponent<ContentPositioning>())
            {
                this.GetComponent<ContentPositioning>().LayoutContent(go, i, this);
            }
            
            /*
            Vector3 center = this.GetComponent<MeshRenderer>().bounds.center;
            Vector3 extents = this.GetComponent<MeshRenderer>().bounds.extents;

            go.transform.position = center + new Vector3(data[i].xPosition * extents.x, data[i].yPosition * extents.y, extents.z);
            */

            GameObject description = GameObject.Instantiate(this.DescriptionPrefab);
            /*description.transform.position = go.transform.position + new Vector3(go.transform.localScale.x * 1.1f, 0.0f, 0.0f);
            description.transform.rotation = go.transform.rotation;
            description.transform.parent = go.transform;*/
            // description.transform.position = this.transform.position + new Vector3(0.0f, 0.5f, 1.0f);


            description.transform.position = go.transform.position;
            description.transform.rotation = go.transform.rotation;
            description.transform.parent = go.transform;
            description.transform.localPosition += new Vector3(0.0f, go.transform.localScale.y * 2.1f, go.transform.localScale.z * 1.1f);


            //Then it will shows up corresponding text messages to each image as read from epoch
            if (description.GetComponentInChildren<UnityEngine.UI.Text>())
            {
                description.GetComponentInChildren<UnityEngine.UI.Text>().text = data[i].epoch;
            }
            GameObject imageFrame = GameObject.Instantiate(this.ImageFrame);
            imageFrame.transform.position = go.transform.position;
            imageFrame.transform.rotation = go.transform.rotation;
            imageFrame.transform.parent = go.transform;
            imageFrame.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) ;
            

            //Script for Cube Description is already created
            CubeDescription descriptionScript = go.AddComponent<CubeDescription>();
            if (description.GetComponentInChildren<UnityEngine.UI.Text>())
            {
                descriptionScript.description = description.GetComponentInChildren<UnityEngine.UI.Text>();
            }
            descriptionScript.data = data[i];

            descriptionScript.ImageFrame = imageFrame;
            imageFrame.SetActive(false);



            GazeBasedDetailButton buttonScript = null;

            for (int j = 0; j < description.transform.childCount; j++)
            {
                if (description.transform.GetChild(j).name == "ButtonPlaceHolder")
                {
                    GameObject placeHolder = description.transform.GetChild(j).gameObject;
                    //placeHolder.transform.localPosition += new Vector3(0.0f, -description.transform.localScale.y * 1.1f, 0.0f);
                    placeHolder.transform.position = go.transform.position - new Vector3(go.GetComponent<MeshRenderer>().bounds.extents.x * 1.0f, go.GetComponent<MeshRenderer>().bounds.extents.y * 1.1f, 0.0f);
                    placeHolder.transform.localPosition += new Vector3(0.0f, 0.0f, go.transform.localScale.z * 3.1f);
                    descriptionScript.button = placeHolder;
                    buttonScript = placeHolder.AddComponent<GazeBasedDetailButton>();
                    placeHolder.SetActive(false);
                }
            }
            

            if (this.DetailsPrefab)
            {
                GameObject details = GameObject.Instantiate(this.DetailsPrefab);
                details.transform.position = go.transform.position;
                details.transform.rotation = go.transform.rotation;
                details.transform.parent = go.transform;
                // scale according to the image
                //details.transform.localScale = go.transform.localScale;
                details.transform.localPosition += new Vector3(0.0f, 0.0f, go.transform.localScale.z * 1.1f);

                details.SetActive(true);

                Text detailsText = details.GetComponentInChildren<Text>();
                detailsText.text = data[i].fileName + "\n" + data[i].epoch;

                if (buttonScript)
                {
                    buttonScript.TextDescription = details;
                }

                if (details.GetComponent<RectTransform>())
                {
                    RectTransform rectangle = details.GetComponent<RectTransform>();
                    float xScale = rectangle.rect.width * go.transform.localScale.x;
                    float yScale = rectangle.rect.height * go.transform.localScale.y;

                    for (int j = 0; j < details.transform.childCount; j++)
                    {
                        if (details.transform.GetChild(j).GetComponent<RectTransform>())
                        {
                            RectTransform cRectangle = details.transform.GetChild(j).GetComponent<RectTransform>();
                            cRectangle.sizeDelta = new Vector2(xScale, yScale);
                        }
                        else
                        {
                            Vector3 currentScale = details.transform.GetChild(j).transform.localScale;
                            currentScale.x = xScale;
                            currentScale.y = yScale;
                            details.transform.GetChild(j).transform.localScale = currentScale;
                        }
                    }
                }

                details.SetActive(false);
            }

            if (this.ButtonPrefab)
            {

            }

            this.SubComponents.Add(data[i].ID, go);
        }

        this.transform.position = backupPosition;
        this.transform.rotation = backupRotation;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleHit(SteamVR_Controller.Device controller)
    {
        this.switchDoorState(controller);
    }


    private void switchDoorState(SteamVR_Controller.Device controller)
    {
        UnityEngine.Debug.Log("current state: " + this.PanelState);

        switch (this.PanelState)
        {
            case PanelStates.IN_FRONT:
                if (controller.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger))
                {
                    this.StartCoroutine(this.doAnimation("Closing", PanelStates.IN_BACKGROUND));
                }   
                break;
            case PanelStates.IN_BACKGROUND:
                this.StartCoroutine(this.doAnimation("Openning", PanelStates.IN_FRONT));
                break;
            default:
                break;
        }
    }

    public void SwitchSubComponentState(string objectID, bool value, string clustername = null)
    {
        GameObject component = null;
        if (this.SubComponents.TryGetValue(objectID, out component))
        {
            UnityEngine.Debug.Log("cross panel switch " + objectID + "...");
            if (component.GetComponent<CubeDescription>())
            {
                UnityEngine.Debug.Log("script found at " + objectID + "...");
                //Destroy(component);
                component.GetComponent<CubeDescription>().description.gameObject.SetActive(value);
                component.GetComponent<CubeDescription>().button.gameObject.SetActive(false);
                component.GetComponent<CubeDescription>().ImageFrame.gameObject.SetActive(value);
            }
        }
        foreach (CubeDescription description in this.GetComponentsInChildren<CubeDescription>())
        {
            if (description.data.clustername != null && clustername != null)
            {
                if (description.data.clustername == clustername)
                {
                    description.description.gameObject.SetActive(value);
                    description.button.gameObject.SetActive(false);
                    description.ImageFrame.gameObject.SetActive(value);
                }
            }
        }
    }
    

    private Vector3 backupScale;

    private IEnumerator doAnimation(string animation, PanelStates nextState)
    {
        UnityEngine.Debug.Log("starting animation at index: " + this.animator.GetLayerIndex(animation));

        this.PanelState = PanelStates.IN_TRANSITION;
        this.animator.SetTrigger(animation);

        yield return new WaitForSeconds(1.0f);

        this.PanelState = nextState;

        // just testing
        if (nextState == PanelStates.IN_BACKGROUND)
        {
            this.transform.localScale = this.backupScale;
            Debug.Log("backgorund...");
            foreach (GazeBasedDetailButton button in this.GetComponentsInChildren<GazeBasedDetailButton>())
            {
                button.FocusLost();
                button.TextDescription.SetActive(false);
            }
            foreach (CubeDescription description in this.GetComponentsInChildren<CubeDescription>())
            {
                // also reduce scale for selected objects
                description.RevertScale();
                description.description.gameObject.SetActive(false);
                description.button.gameObject.SetActive(false);
            }
        }
        else if (nextState == PanelStates.IN_FRONT)
        {
            this.transform.localScale = this.backupScale * 2.0f;
        }

        yield return null;
    }
}

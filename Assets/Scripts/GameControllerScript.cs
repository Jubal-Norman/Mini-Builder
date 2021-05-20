using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MODE
{
    NONE,
    SELECT,
    PLACE
}

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript Instance { get { return instance; } }
    public static GameControllerScript instance;

    public new Camera camera;
    public GameObject floor;
    public GameObject[] library;
    public Material green;
    public Material red;


    private MODE currentMode;
    private uint currentIndex;
    private GameObject currentPreview;
    private GameObject currentSelected;
    private Material currentSelectedMaterial;
    private bool canPlace;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMode = MODE.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMode == MODE.PLACE)
        {
            UpdateObject(currentPreview);
        }
        else if (currentMode == MODE.SELECT && currentSelected != null)
        {
            UpdateObject(currentSelected);

            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Destroy(currentSelected);
                currentSelected = null;
            }    
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetModeToNone();
        }
    }

    private void UpdateTransform(GameObject obj)
    {
        if (Input.GetKey(KeyCode.E))
        {
            obj.transform.Rotate(new Vector3(0, 0.5f, 0));
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            obj.transform.Rotate(new Vector3(0, -0.5f, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            obj.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            obj.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    void OnClick()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            switch (currentMode)
            {
                case MODE.NONE:
                    break;
                case MODE.SELECT:
                    if (objectHit.gameObject.layer != 3 && currentSelected == null)
                    {
                        print(objectHit.name + " selected");
                        currentSelected = objectHit.gameObject;
                        currentSelectedMaterial = currentSelected.GetComponent<MeshRenderer>().material;
                        currentSelected.layer = 2;
                        foreach (Collider collider in currentSelected.GetComponents<Collider>())
                        {
                            collider.isTrigger = true;
                        }
                        currentSelected.GetComponent<Rigidbody>().useGravity = false;
                    }
                    else if (currentSelected != null && canPlace)
                    {
                        currentSelected.GetComponent<MeshRenderer>().material = currentSelectedMaterial;
                        currentSelected.layer = 0;
                        foreach (Collider collider in currentSelected.GetComponents<Collider>())
                        {
                            collider.isTrigger = false;
                        }
                        currentSelected.GetComponent<Rigidbody>().useGravity = true;
                        currentSelected = null;
                    }
                    break;
                case MODE.PLACE:
                    if (canPlace)
                    {
                        Vector3 min = currentPreview.GetComponent<MeshRenderer>().bounds.min;
                        GameObject newObject = Instantiate(library[currentIndex], hit.point + new Vector3(0, currentPreview.transform.position.y - min.y + 0.02f, 0), Quaternion.identity);
                        newObject.transform.localScale = currentPreview.transform.localScale;
                        newObject.transform.rotation = currentPreview.transform.rotation;
                    }
                    break;
            }
        }
    }

    private void UpdateObject(GameObject obj)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            Vector3 min = obj.GetComponent<MeshRenderer>().bounds.min;
            obj.transform.position = hit.point + new Vector3(0, obj.transform.position.y - min.y + 0.02f, 0);// 0.2f * Vector3.up;
            if (obj.GetComponent<PreviewScript>().isOverlapping)
            {
                obj.GetComponent<MeshRenderer>().material = red;
                canPlace = false;
            }
            else
            {
                obj.GetComponent<MeshRenderer>().material = green;
                canPlace = true;
            }
        }
        UpdateTransform(obj);
    }

    public void SetModeToPlace(uint index)
    {
        currentMode = MODE.PLACE;
        UIControllerScript.Instance.HideTooltip();
        currentIndex = index;
        currentPreview = Instantiate(library[currentIndex]);
        currentPreview.layer = 2;
        foreach(Collider collider in currentPreview.GetComponents<Collider>())
        {
            collider.isTrigger = true;
        }
    }

    public void SetModeToNone()
    {
        currentMode = MODE.NONE;
        UIControllerScript.Instance.HideTooltip();
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }

    public void SetModeToSelect()
    {
        currentMode = MODE.SELECT;
        UIControllerScript.Instance.ShowTooltip();
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }

    public MODE GetCurrentMode()
    {
        return currentMode;
    }
}

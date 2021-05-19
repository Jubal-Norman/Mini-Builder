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
        if(currentMode == MODE.PLACE)
        {
            UpdateObject(currentPreview);
        }
        else if (currentMode == MODE.SELECT && currentSelected != null)
        {
            UpdateObject(currentSelected);
        }

        if(Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetModeToNone();
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
                        currentSelected.GetComponent<Collider>().isTrigger = true;
                    }
                    else if (currentSelected != null && canPlace)
                    {
                        currentSelected.GetComponent<MeshRenderer>().material = currentSelectedMaterial;
                        currentSelected.layer = 0;
                        currentSelected.GetComponent<Collider>().isTrigger = false;
                        currentSelected = null;
                    }
                    break;
                case MODE.PLACE:
                    if (canPlace)
                    {
                        Instantiate(library[currentIndex], hit.point + 0.6f * Vector3.up, Quaternion.identity);
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

            obj.transform.position = hit.point + 0.6f * Vector3.up;
            if(obj.GetComponent<PreviewScript>().isOverlapping)
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
    }

    public void SetModeToPlace(uint index)
    {
        currentMode = MODE.PLACE;
        currentIndex = index;
        currentPreview = Instantiate(library[currentIndex]);
        currentPreview.layer = 2;
        currentPreview.GetComponent<Collider>().isTrigger = true;
    }

    public void SetModeToNone()
    {
        currentMode = MODE.NONE;
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }

    public void SetModeToSelect()
    {
        currentMode = MODE.SELECT;
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

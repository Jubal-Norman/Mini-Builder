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

    public Camera camera;
    public GameObject floor;
    public GameObject[] library;
    public Material green;
    public Material red;


    private MODE currentMode;
    private uint currentIndex;
    private GameObject currentPreview;

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
            UpdatePreview();
        }
        if(Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            currentMode = MODE.NONE;
        }
    }


    void OnClick()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;


            print(hit.point);

            switch (currentMode)
            {
                case MODE.NONE:
                    break;
                case MODE.SELECT:
                    print(objectHit.name + " selected");
                    break;
                case MODE.PLACE:
                    if (objectHit.gameObject.Equals(floor))
                    {
                        Instantiate(library[currentIndex], hit.point, Quaternion.identity);
                    }
                    break;
            }
        }
    }

    private void UpdatePreview()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            currentPreview.transform.position = hit.point;
            currentPreview.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = green;
            //if(currentPreview.transform.GetChild(0).gameObject)
        }
    }

    public void SetModeToPlace(uint index)
    {
        currentMode = MODE.PLACE;
        currentIndex = index;
        currentPreview = Instantiate(library[currentIndex], this.transform);
    }

    public void SetModeToNone()
    {
        currentMode = MODE.NONE;
    }

    public void SetModeToSelect()
    {
        currentMode = MODE.SELECT;
    }

    public MODE GetCurrentMode()
    {
        return currentMode;
    }
}

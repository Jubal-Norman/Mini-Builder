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
    public MODE currentMode;
    public uint currentIndex;

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
        if(Input.GetMouseButtonDown(0))
        {
            OnClick();
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
}

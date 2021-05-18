using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public Camera camera;
    public GameObject floor;
    public GameObject[] library;
    // Start is called before the first frame update
    void Start()
    {

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


            print(objectHit.name);
            print(hit.point);

            if (objectHit.gameObject.Equals(floor))
            {
                Instantiate(library[0], hit.point, Quaternion.identity);
            }
        }
    }
}

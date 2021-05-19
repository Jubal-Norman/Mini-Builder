using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewScript : MonoBehaviour
{
    int overlaps;

    public bool isOverlapping
    {
        get
        {
            return overlaps > 0;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        overlaps++;
        print("I collided!");
    }

    private void OnTriggerExit(Collider collision)
    {
        overlaps--;
    }
}

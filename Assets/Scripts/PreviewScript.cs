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

    void OnCollisionEnter(Collision collision)
    {
        overlaps++;
        print("I collided!");
    }

    private void OnCollisionExit(Collision collision)
    {
        overlaps--;
    }
}

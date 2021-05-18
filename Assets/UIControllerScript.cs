using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    [SerializeField]
    public GameObject libraryMenuUI;

    private MODE previousMode;

    public void ShowLibrary()
    {
        libraryMenuUI.SetActive(true);
        previousMode = GameControllerScript.Instance.currentMode;
        SetNoneMode();
    }

    public void HideLibrary()
    {
        libraryMenuUI.SetActive(false);
        GameControllerScript.Instance.currentMode = previousMode; 
    }

    public void SelectLibraryObject(int index)
    {
        GameControllerScript.Instance.currentIndex = (uint)index;
        previousMode = MODE.PLACE;
    }

    public void SetSelectMode()
    {
        GameControllerScript.Instance.currentMode = MODE.SELECT;
    }

    public void SetNoneMode()
    {
        GameControllerScript.Instance.currentMode = MODE.NONE;
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    [SerializeField]
    public GameObject libraryMenuUI;

    private MODE previousMode;
    private uint currentIndex;

    public void ShowLibrary()
    {
        libraryMenuUI.SetActive(true);
        previousMode = GameControllerScript.Instance.GetCurrentMode();
        SetNoneMode();
    }

    public void HideLibrary()
    {
        libraryMenuUI.SetActive(false);
        switch(previousMode)
        {
            case MODE.NONE:
                GameControllerScript.Instance.SetModeToNone();
                break;
            case MODE.PLACE:
                GameControllerScript.Instance.SetModeToPlace(currentIndex);
                break;
            case MODE.SELECT:
                GameControllerScript.Instance.SetModeToSelect();
                break;
        }
    }

    public void SelectLibraryObject(int index)
    {
        currentIndex = (uint)index;
        previousMode = MODE.PLACE;
    }

    public void SetSelectMode()
    {
        GameControllerScript.Instance.SetModeToSelect();
    }

    public void SetNoneMode()
    {
        GameControllerScript.Instance.SetModeToNone();
    }    
}

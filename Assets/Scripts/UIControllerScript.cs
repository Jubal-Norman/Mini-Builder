using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    public static UIControllerScript Instance { get { return instance; } }
    public static UIControllerScript instance;

    [SerializeField]
    public GameObject libraryMenuUI;

    [SerializeField]
    public GameObject tooltipUI;

    private MODE previousMode;
    private uint currentIndex;

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

    public void ShowTooltip()
    {
        tooltipUI.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipUI.SetActive(false);
    }

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

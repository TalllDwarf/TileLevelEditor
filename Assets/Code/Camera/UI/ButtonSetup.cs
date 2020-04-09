using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSetup : MonoBehaviour {

    public MainMenuButtons button;

    GameObject canvas;
    SliderController sliderC;
    UnityEngine.UI.Button connectedButton;

    void Start () {

        canvas = GameObject.FindGameObjectWithTag(Tags.Canvas);

        connectedButton = GetComponent<UnityEngine.UI.Button>();

        switch (button)
        {
            case MainMenuButtons.New:
                connectedButton.onClick.AddListener(() => canvas.GetComponent<MainMenuUI>().NewMapBtn_click());
                break;

            case MainMenuButtons.Load:
                connectedButton.onClick.AddListener(() => canvas.GetComponent<MainMenuUI>().LoadMapBtn_Click());
                break;

            case MainMenuButtons.Exit:
                connectedButton.onClick.AddListener(() => canvas.GetComponent<MainMenuUI>().Exit_click());
                break;

            case MainMenuButtons.Back:
                connectedButton.onClick.AddListener(() => canvas.GetComponent<MainMenuUI>().ToMainMenu_Click());
                break;

            case MainMenuButtons.Create:
                sliderC = canvas.GetComponentInChildren<SliderController>();
                connectedButton.onClick.AddListener(() => canvas.GetComponent<MainMenuUI>().CreateMapBtn_Click(sliderC.GetCurrentSpawnObject()));
                break;
        }
	}

    private void OnDestroy()
    {
        if (connectedButton != null)
            connectedButton.onClick.RemoveAllListeners();
    }

}


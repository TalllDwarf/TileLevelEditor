using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : RadialButtonMenu {

    public MenuButtons buttonType = MenuButtons.Save;

    Button connectedButton;
    MainMenuUI mainGUI;

    protected override void Start()
    {
        base.Start();

        connectedButton = GetComponent<Button>();
        mainGUI = GetComponentInParent<MainMenuUI>();

        switch (buttonType)
        {
            case MenuButtons.Save:
                connectedButton.onClick.AddListener(() => mainGUI.SaveMapBtn_Click());
                break;
            case MenuButtons.Options:
                connectedButton.onClick.AddListener(() => mainGUI.OptionBtn_Click());
                break;
            case MenuButtons.Exit:
                connectedButton.onClick.AddListener(() => mainGUI.ToMainMenu_Click());
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}

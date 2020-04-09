using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelUI : MonoBehaviour {

    Dropdown uiDropdown = null;
    Button loadButton = null;

    MapGrid mapgrid;
    MainMenuUI mainMenu;

    SaveLoadManager loadManager;
    bool canLoad = false;

    // Use this for initialization
    void Start () {
        //Get map generating object
        mapgrid = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapGrid>();

        //Gets load button
        loadButton = GetComponentInChildren<Button>();
        loadButton.onClick.AddListener(() => LoadSelectedLevel());

        //Ui dropdown list
        uiDropdown = GetComponent<Dropdown>();

        //Map Loader
        loadManager = SaveLoadManager.Instance;

        //Add all saved levels to the dropdown ui
        AddListToUI(loadManager.GetListOfLevels());

        mainMenu = GetComponentInParent<MainMenuUI>();
	}

    //load the selected level
    void LoadSelectedLevel()
    {
        if (canLoad)
        {
            mapgrid.CreateMapFromFile(loadManager.LoadList<MapTileSave>(uiDropdown.options[uiDropdown.value].text), loadManager.Load<Coord>(uiDropdown.options[uiDropdown.value].text));
            mainMenu.SetGameUI();
        }
    }

    //Adds all loaded levels to the ui
    void AddListToUI(string[] files)
    {
        if (files.Length != 0)
        {
            uiDropdown.options.Clear();
            foreach (string s in files)
            {
                Dropdown.OptionData data = new Dropdown.OptionData(s);
                uiDropdown.options.Add(data);
                uiDropdown.RefreshShownValue();
                loadButton.enabled = true;
            }
            canLoad = true;
        }
        else
        {
            uiDropdown.options.Clear();
            uiDropdown.options.Add(new Dropdown.OptionData("No Saves Found"));
            uiDropdown.RefreshShownValue();
            loadButton.enabled = false;
            canLoad = false;
        }
    }
}
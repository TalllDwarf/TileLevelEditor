using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {

    public GameObject MainUI;
    public GameObject NewMapUI;
    public GameObject LoadMapUI;
    public GameObject GameUI;
    public GameObject SaveUI;

    GameObject currentUI;

    //Save Ui is separeate we want this open when we also have our main ui;
    GameObject currentSaveUI;

    CameraController cameraC;

    // Use this for initialization
    void Start()
    {
        currentUI = Instantiate(MainUI, transform);
        cameraC = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<CameraController>();
    }

    //Return to the main ui
    public void ToMainMenu_Click()
    {
        Destroy(currentUI);
        RemoveSaveUI();
        currentUI = Instantiate(MainUI, transform);
    }

    //Changes the UI to create a new map
    public void NewMapBtn_click()
    {
        Destroy(currentUI);
        RemoveSaveUI();
        currentUI = Instantiate(NewMapUI, transform);
    }

    //Change to Load map UI
    public void LoadMapBtn_Click()
    {
        Destroy(currentUI);
        RemoveSaveUI();
        currentUI = Instantiate(LoadMapUI, transform);
    }

    public void RemoveSaveUI()
    {
        if (currentSaveUI != null)
            Destroy(currentSaveUI);                
    }

    //Opens the save UI
    public void SaveMapBtn_Click()
    {
        RemoveSaveUI();
        currentSaveUI = Instantiate(SaveUI, transform);
    }

    public void OptionBtn_Click()
    {

    }

    //Tell the map create to create a new map
    //Set the UI to the game ui
    public void CreateMapBtn_Click(SpawnObject spawnV)
    {
        GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapCreator>().CreateNewMap(spawnV);
        cameraC.UpdateMaxMapSize(new Coord(spawnV.rowSize, spawnV.rowSize));
        SetGameUI();
    }

    public void SetGameUI()
    {
        Destroy(currentUI);
        currentUI = Instantiate(GameUI, transform);
    }

    //Exit the application
    public void Exit_click()
    {
        Application.Quit();
    }
}

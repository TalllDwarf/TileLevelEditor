using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour {

    Button connectedButton;
    MapGrid map;
    SaveLoadManager saveManager;
    InputField inputF;
    MainMenuUI mainMenu;


	// Use this for initialization
	void Start () {
        connectedButton = GetComponent<Button>();
        connectedButton.onClick.AddListener(() => Save());

        inputF = GetComponentInChildren<InputField>();

        map = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapGrid>();
        saveManager = SaveLoadManager.Instance;
        mainMenu = GetComponentInParent<MainMenuUI>();
	}

    public void Save()
    {
        if (!string.IsNullOrEmpty(inputF.text))
        {
            List<MapTileSave> saveList = new List<MapTileSave>();

            for (int x = 0; x < map.mapSize.x; x++)
            {
                for (int y = 0; y < map.mapSize.y; y++)
                {
                    saveList.Add(map.GetTileController(new Coord(x, y)));
                }
            }

            saveManager.SaveList(saveList, inputF.text);
            saveManager.Save(map.mapSize, inputF.text);
            mainMenu.RemoveSaveUI();       
        }
        else
        {
            foreach(Text t in GetComponentsInChildren<Text>())
            {
                t.enabled = true;
            }
        }
    }
}

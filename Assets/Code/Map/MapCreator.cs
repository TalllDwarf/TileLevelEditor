using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {
    MapGrid grid;

	// Use this for initialization
	void Start () {
        grid = GetComponent<MapGrid>();
	}
	
    public void CreateNewMap(SpawnObject spawning)
    {
        //Create new map
        grid.InitialiseMap(spawning);
    }

	// Update is called once per frame
	void Update () {
		
	}
}

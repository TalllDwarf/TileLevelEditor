using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : BuildingController {

    public int houseSize = 2;
    MapController mapC;

    void Start()
    {
        mapC = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapController>();
        mapC.HouseAdded(houseSize);
    }

    void OnDestroy()
    {
        if (mapC != null)
            mapC.HouseDeleted(houseSize);
    }
}

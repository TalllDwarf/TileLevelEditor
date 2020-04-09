using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    private int totalNumberOfVillagers = 0;
    private int currentNumberOfVillagers = 0;

    List<BuildingController> waitingVillager;

    void Start()
    {
        waitingVillager = new List<BuildingController>();
    }

    //Add maximum allowed villagers
    public void HouseAdded(int villagerSize)
    {
        totalNumberOfVillagers += villagerSize;
    }

    //Reduce maximum allowed villagers
    public void HouseDeleted(int villagerSize)
    {
        totalNumberOfVillagers -= villagerSize;
    }

    //Building asking if it can spawn a villager
    public bool RequstVillager(BuildingController requestedBY)
    {
        if (totalNumberOfVillagers == currentNumberOfVillagers)
        {
            waitingVillager.Add(requestedBY);
            return false;
        }
        else
            currentNumberOfVillagers++;
        return true;
    }

    //building is being destroyed and is returning a villager
    public void RemoveVillager()
    {
        while (waitingVillager.Count > 0)
        {
            if (waitingVillager[0] != null)
            {
                waitingVillager[0].SpawnVillager();
                waitingVillager.RemoveAt(0);
            }
            else
                waitingVillager.RemoveAt(0);
        }            
    }
}

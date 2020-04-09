using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMillController : BuildingController
{

    public int holdingAmount = 0;
    public int amountCreated = 0;

    public float createTime = 2f;
    public float timeLeft = 0;

    SawSpinner saw;

    private bool villagerSpawned = false;

    public override void Init(Coord spawnPosition, BuildingType bType, Coord[] otherPositions)
    {
        base.Init(spawnPosition, bType, otherPositions);

        timeLeft = createTime;
        saw = GetComponentInChildren<SawSpinner>();

        if(mapEditor.hasLoaded())
            SpawnVillager();
    }

    public override void Deposit(int depositAmount)
    {
        saw.isWorking = true;
        holdingAmount += depositAmount;
    }

    public override void SpawnVillager()
    {
        GameObject villager = Instantiate(mapEditor.villagerPrefab, new Vector3(Tags.TileSize * position.x, 0, Tags.TileSize * position.y), Quaternion.identity, transform);
        villager.GetComponent<EmptyBrain>().Init(position, gameObject, TileType.Wood, ResourceType.Wood);
        villagerSpawned = true;
    }

    private void Update()
    {
        if (holdingAmount > 0)
        {            
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                holdingAmount--;
                amountCreated += 2;
                timeLeft = createTime;

                if (holdingAmount <= 0)
                    saw.isWorking = false;
            }
        }

        if(!villagerSpawned && mapEditor.hasLoaded())
        {
            SpawnVillager();
        }
    }



}

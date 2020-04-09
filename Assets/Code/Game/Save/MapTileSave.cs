using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapTileSave
{
    public TileType tile;
    public BuildingType building;
    public ResourceType resourceType;

    public int maxResourceSize;
    public int resourceSize;
    public bool doesResourceRegen;
    public float regenTime;
    public int regenAmount;

    public MapTileSave()
    {

    }

    public MapTileSave(TileType tileT)
    {
        tile = tileT;
    }

    public MapTileSave(TileType tileT, BuildingType buildingT)
    {
        tile = tileT;
        building = buildingT;
    }

    public MapTileSave(TileType tileT, ResourceType resourceT, int maxResourceAmount, int resourceAmount, bool doesItRegen, float regenNewTime, int regenNewAmount)
    {
        tile = tileT;
        resourceType = resourceT;
        maxResourceSize = maxResourceAmount;
        resourceSize = resourceAmount;
        doesResourceRegen = doesItRegen;
        regenTime = regenNewTime;
        regenAmount = regenNewAmount;
    }

    public override string ToString()
    {
        return string.Format("TileID:{0},BuildingID:{1},ResourceSize:{2}", tile, building, resourceSize);
    }

    //Convert TileController to a MapTileSave
    public static implicit operator MapTileSave(TileController tile)
    {
        Resource tileResource = tile.GetConnectedResource();

        if (tileResource != null)
            return new MapTileSave(tile.GetTileType(), tileResource.resourceType, tileResource.maxResouse, tileResource.resourceLeft, tileResource.doesResourseRegen, tileResource.regenTime, tileResource.regenAmount);
        else if (tile.GetBuildingType() != BuildingType.NULL)
        {
            return new MapTileSave(tile.GetTileType(), tile.GetBuildingType());
        }
        else
            return new MapTileSave(tile.GetTileType());
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class Tags
{
    public const float TileSize = 2.0f;
    public const float UIButtonWidth = 60.0f;
    public const float UIButtonPadding = 10.0f;
    public const float pi = 3.14f;

    //Input
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    public const string MouseWheel = "ZoomWheel";

    //Tags
    public const string MainCamera = "MainCamera";
    public const string MapController = "MapController";
    public const string Villager = "Villager";
    public const string JobDelegator = "MapController";
    public const string Floor = "Floor";
    public const string Building = "Building";
    public const string NetworkManager = "NetworkManager";

    //UI
    public const string Canvas = "Canvas";
    public const string xSlider = "xSlider";
    public const string ySlider = "ySlider";
    public const string spawnSlider = "SpawnSlider";

    public const string treeSizeSlider = "treeSSlider";
    public const string treeSlider = "treeSlider";
    public const string waterSizeSlider = "waterSSlider";
    public const string waterSlider = "waterSlider";
    public const string rockSizeSlider = "rockSSlider";
    public const string rockSlider = "rockSlider";
    public const string ironSizeSlider = "ironSSlider";
    public const string ironSlider = "ironSlider";

    //Animation
    public const string IsIdleAnim = "IsIdle";
    public const string IsMovingAnim = "IsMoving";
    public const string PlayIsMovingAnim = "PlayIdle";

    //Resource object tag
    public const string RockTag = "Rock";
    public const string TreeTag = "Tree";
    public const string WaterTag = "Water";
    public const string IronTag = "Gold";

    //Item object tag
    public const string RockItemTag = "Stone";
    public const string TreeItemTag = "Wood";
    public const string WaterItemTag = "WaterBucket";
    public const string IronItemTag = "GoldOre";
    public const string MoneyItemTag = "GoldBar";
}

public enum Direction
{
    Right,
    Up,
    Left,
    Down
}

public enum TileType
{
    Empty,
    InUse,
    Wood,
    Stone,
    Iron,
    Water,
    Building,
    BuildingArea
};

public enum BuildingType
{
    House,
    Sawmill,
    StoneCutter,
    IronMine,
    Storage,
    NULL
};

public enum ResourceType
{
    Wood,
    Stone,
    Gold,
    Food,
    Water,
    Planks,
    Nothing
}

enum ResourceView
{
    Full,
    Moderate,
    Limited,
    Empty
}

public enum VillagerJobs
{
    WoodGatherer,
    StonerCutter,
    GoldMiner,
    FoodHavester,
    WaterGatherer
}

public enum MainMenuButtons
{
    New,
    Load,
    Exit,
    Back,
    Create
}

public enum MenuButtons
{
    Save,
    Options,
    Exit
}

public enum BuildButtons
{
    House,
    Saw,
    Stone,
    Mine,
    Bucket,
    Grass,
    Water,
    Tree,
    Rock,
    Wreck
}

public enum NetworkMessageID
{
    NULL,
    Map,
    NewBuilding,
    BuildingDestroyed,
    NewResource,
    ResourceDestroyed,
    NewVillager,
    VillagerDestroyed
}

public struct SpawnObject
{
    public int rowSize, colSize, spawnSize, waterCount, waterSize, treeCount, treeSize, rockCount, rockSize, ironCount, ironSize;

    public SpawnObject(int row, int col, int spawn, int waterC, int waterS, int treeC, int treeS, int rockC, int rockS, int ironC, int ironS)
    {
        rowSize = row;
        colSize = col;
        spawnSize = spawn;
        waterCount = waterC;
        waterSize = waterS;
        treeCount = treeC;
        treeSize = treeS;
        rockCount = rockC;
        rockSize = rockS;
        ironCount = ironC;
        ironSize = ironS;
    }
}

[Serializable]
public struct Coord
{
    public int x;
    public int y;

    public Coord(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public Coord(Vector2 vec)
    {
        x = (int)vec.x;
        y = (int)vec.y;
    }

    public static bool operator ==(Coord c1, Coord c2)
    {
        return (c1.x == c2.x && c1.y == c2.y);
    }

    public static bool operator !=(Coord c1, Coord c2)
    {
        return (c1.x != c2.x || c1.y != c2.y);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

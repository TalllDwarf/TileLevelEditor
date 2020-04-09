using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


class MapGrid : MonoBehaviour
{
    //Stores the prefab for tiles
    public GameObject tilePrefab = null;
    public GameObject waterPrefab = null;
    public GameObject rockPrefab = null;
    public GameObject treePrefab = null;
    public GameObject ironPrefab = null;
    public GameObject[] buildings = null;

    public int waterHoles = 4;
    public int maxWaterSize = 15;
    public int forests = 4;
    public int maxForestSize = 20;
    public int quarys = 4;
    public int maxQuarySize = 10;
    public int ironMines = 2;
    public int maxIronSize = 5;

    public int spawnSize = 1;

    public float spawnSpeed = 0.001f;
    public float nextSpawn;

    private bool start = false;
    private bool complete = false;
    private bool loadingFromFile = false;
    private bool showMap = false;

    //Stores the tiles of the map
    public List<List<GameObject>> tiles;

    int currentYPosition;
    Coord maxGrid;
    Coord spawnPoint;

    //Map we have loaded
    List<MapTileSave> loadedMap;

    CameraController mainCamera;

    //Get a read only version of the map
    public ReadOnlyCollection<List<GameObject>> GetMapTiles
    {
        get
        {
            return tiles.AsReadOnly();
        }
    }

    //Returns a single tile from the map
    public GameObject GetTile(Coord getCoord)
    {
        if (getCoord.x < 0 || getCoord.x >= maxGrid.x || getCoord.y < 0 || getCoord.y >= maxGrid.y)
            return null;

        return tiles[getCoord.x][getCoord.y];
    }

    //Returns the tile component from the map
    public TileController GetTileController(Coord getCoord)
    {
        if (getCoord.x < 0 || getCoord.x >= maxGrid.x || getCoord.y < 0 || getCoord.y >= maxGrid.y)
            return null;

        try
        {
            TileController tileC = tiles[getCoord.x][getCoord.y].GetComponent<TileController>();
            return tileC;
        }
        catch(System.ArgumentOutOfRangeException e)
        {
            return null;
        }

    }

    public Resource GetResourceAtTile(Coord tile)
    {
        return tiles[tile.x][tile.y].GetComponentInChildren<Resource>();
    }

    //replace a tile
    public void ReplaceTile(Coord position, GameObject replacement)
    {
        Destroy(tiles[position.x][position.y]);
        tiles[position.x][position.y] = replacement;
    }

    //Returns the map size
    public Coord mapSize
    {
        get
        {
            return maxGrid;
        }
    }

    //Creates map from a file save
    public void CreateMapFromFile(List<MapTileSave> savedMap, Coord newMapSize)
    {
        loadingFromFile = true;
        start = true;
        complete = false;

        DestroyMap();

        loadedMap = savedMap;

        currentYPosition = 0;

        //Create map from saved file
        maxGrid = newMapSize;
        tiles = new List<List<GameObject>>();

        for (int x = 0; x < maxGrid.x; x++)
            tiles.Add(new List<GameObject>());

        mainCamera.canPlayerMove = false;
        mainCamera.UpdateMaxMapSize(newMapSize);
        mainCamera.JumpToPosition(new Vector3(newMapSize.x, newMapSize.y / 2, -8));      
    }

    //Creates a map from a save file
    private GameObject CreateTileFromSave(MapTileSave tileSave, int x, int y)
    {
        if (tileSave.tile == TileType.Water)
        {
            return Instantiate(waterPrefab, new Vector3(Tags.TileSize * x, 0, Tags.TileSize * y), Quaternion.identity, transform);
        }
        else
        {
            GameObject gO = Instantiate(tilePrefab, new Vector3(Tags.TileSize * x, 0, Tags.TileSize * y), Quaternion.identity, transform);

            switch (tileSave.tile)
            {
                case TileType.Building:
                    GameObject newBuilding = Instantiate(buildings[(int)tileSave.building], new Vector3(x * Tags.TileSize, 0, y * Tags.TileSize), Quaternion.identity, gO.transform);
                    newBuilding.GetComponent<BuildingController>().Init(new Coord(x, y), BuildingType.House);
                    gO.GetComponent<TileController>().Init(x, y, TileType.Building, newBuilding);
                    break;

                case TileType.Iron:
                    //TODO:Iron spawn
                    break;

                case TileType.Stone:
                    GameObject stone = Instantiate(rockPrefab, new Vector3(x * Tags.TileSize, 0, y * Tags.TileSize), Quaternion.identity, gO.transform);
                    stone.GetComponent<Resource>().Init(tileSave.maxResourceSize, tileSave.resourceSize, tileSave.doesResourceRegen, tileSave.regenTime, tileSave.regenAmount);
                    gO.GetComponent<TileController>().Init(x, y, tileSave.tile, stone);
                    break;

                case TileType.Wood:
                    GameObject wood = Instantiate(treePrefab, new Vector3(x * Tags.TileSize, 0, y * Tags.TileSize), Quaternion.identity, gO.transform);
                    wood.GetComponent<Resource>().Init(tileSave.maxResourceSize, tileSave.resourceSize, tileSave.doesResourceRegen, tileSave.regenTime, tileSave.regenAmount);
                    gO.GetComponent<TileController>().Init(x, y, tileSave.tile, wood);
                    break;

                default:
                    gO.GetComponent<TileController>().Init(new Coord(x, y), tileSave.tile);
                    break;
            }

            return gO;
        }
    }

    public void Start()
    {
        maxGrid = new Coord(0, 0);
        nextSpawn = spawnSpeed;

        mainCamera = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<CameraController>();

        InitialiseMap(new SpawnObject(50, 50, 4, 30, 30, 30, 30, 30, 30, 30, 30));
        showMap = true;
    }

    //Slowly generate map
    public void Update()
    {
        if (start)
            if (!complete)
                if (!loadingFromFile)
                {
                    //Generate a new map
                    //Slowly builds the map
                    if (maxGrid.y > currentYPosition)
                    {
                        for (int x = 0; x < maxGrid.x; x++)
                        {
                            GameObject tile = Instantiate(tilePrefab, new Vector3(x * Tags.TileSize, 0, currentYPosition * Tags.TileSize), Quaternion.identity, transform);
                            tile.GetComponent<TileController>().Init(x, currentYPosition, TileType.Empty);
                            tiles[x].Add(tile);
                        }

                        currentYPosition++;

                    }
                    else if (currentYPosition == maxGrid.y)
                    {
                        if (nextSpawn <= 0)
                        {
                            GenerateMap();
                            nextSpawn = spawnSpeed;
                        }
                        else
                        {
                            nextSpawn -= Time.deltaTime;
                        }
                    }
                }
                else
                {
                    //Create the map
                    for(int x = 0; x < mapSize.x; x++)
                    {
                        tiles[x].Add(CreateTileFromSave(loadedMap[(mapSize.x * x) + currentYPosition], x, currentYPosition));
                    }

                    currentYPosition++;

                    if (currentYPosition == mapSize.y)
                    {
                        start = false;
                        complete = true;
                        loadingFromFile = false;
                        mainCamera.canPlayerMove = true;

                        loadedMap.Clear();
                        loadedMap = null;
                    }
                }
    }

    //Retruns true when the map has been loaded
    public bool hasLoaded()
    {
        return complete;
    }

    //Creates the starting area
    public void InitialiseMap(SpawnObject spawnV)
    {
        //Get variables for the spawning
        forests = spawnV.treeCount;
        maxForestSize = spawnV.treeSize;

        waterHoles = spawnV.waterCount;
        maxWaterSize = spawnV.waterSize;

        quarys = spawnV.rockCount;
        maxQuarySize = spawnV.rockSize;

        ironMines = spawnV.ironCount;
        maxIronSize = spawnV.ironSize;

        spawnSize = spawnV.spawnSize;

        mainCamera.JumpToPosition(new Vector3(spawnV.rowSize, spawnV.colSize / 2, -8));

        DestroyMap();
        maxGrid = new Coord(spawnV.rowSize, spawnV.colSize);

        mainCamera.UpdateMaxMapSize(maxGrid);
        currentYPosition = 0;

        tiles = new List<List<GameObject>>();

        for (int x = 0; x < maxGrid.x; x++)
            tiles.Add(new List<GameObject>());

        //Spawn people in the center of the map
        spawnPoint = new Coord((maxGrid.x / 2), (maxGrid.y / 2));

        start = true;
        complete = false;
        loadingFromFile = false;
        showMap = false;
    }

    //Removes all the tiles on the map
    private void DestroyMap()
    {
        if (tiles != null)
            foreach (List<GameObject> list in tiles)
            {
                foreach (GameObject gameO in list)
                {
                    Destroy(gameO);
                }
                list.Clear();
            }
        tiles = null;
    }

    public bool IsEmpty(Coord tile)
    {
        return tiles[tile.x][tile.y].GetComponent<TileController>().GetTileType() == TileType.Empty;
    }

    //Returns the tile gameobject at the coordinate
    public GameObject GetTileAt(Coord position)
    {
        if (position.x < 0 || position.x >= tiles.Count || position.y < 0 || position.y >= tiles[position.x].Count)
        {
            throw new System.IndexOutOfRangeException();
        }

        return tiles[position.x][position.y];
    }

    //Get the tile type and a coordinate
    public TileType GetTypeAt(Coord position)
    {
        if (position.x < 0 || position.x >= tiles.Count || position.y < 0 || position.y >= tiles[position.x].Count)
        {
            throw new System.IndexOutOfRangeException();
        }

        TileType positionType = tiles[position.x][position.y].GetComponent<TileController>().GetTileType();
        return positionType;
    }

    //Adds resources to the map
    private void GenerateMap()
    {
        Coord resourceSpawn = GetRandomCoord(spawnPoint);
        //Spawn Water
        if (waterHoles > 0)
        {
            //Get a spawn point for the water
            resourceSpawn = GetRandomCoord(spawnPoint);
            if (resourceSpawn.x != -1)
                AddResource(TileType.Water, resourceSpawn);
            waterHoles--;
        }
        //Spawn Wood
        else if (forests > 0)
        {
            resourceSpawn = GetRandomCoord(spawnPoint);
            if (resourceSpawn.x != -1)
                AddResource(TileType.Wood, resourceSpawn);
            forests--;
        }
        //Spawn stone
        else if (quarys > 0)
        {
            resourceSpawn = GetRandomCoord(spawnPoint);
            if (resourceSpawn.x != -1)
                AddResource(TileType.Stone, resourceSpawn);
            quarys--;
        }
        else
        {
            if (!showMap)
            {
                //Add the house to the center of the map
                GameObject house = Instantiate(buildings[(int)BuildingType.House], new Vector3(spawnPoint.x * Tags.TileSize, 0, spawnPoint.y * Tags.TileSize), Quaternion.identity, tiles[spawnPoint.x][spawnPoint.y].transform);
                tiles[spawnPoint.x][spawnPoint.y].GetComponent<TileController>().Init(spawnPoint.x, spawnPoint.y, TileType.Building, house);

                //Move camera to first house
                mainCamera.MoveToPosition(new Vector3(spawnPoint.x * Tags.TileSize, 8, (spawnPoint.y * Tags.TileSize) - 7));
            }

            complete = true;
            start = false;
        }

    }

    //Adds resources to the map
    private void AddResource(TileType typeToAdd, Coord resourceSpawn)
    {
        int maxSize = 0;
        Coord[] spread = null;

        switch (typeToAdd)
        {
            case TileType.Water:
                maxSize = Mathf.Clamp(Random.Range(maxWaterSize / 2, maxWaterSize), 1, maxWaterSize);
                spread = GetSpread(resourceSpawn, maxSize);
                SpawnResources(resourceSpawn, spread, TileType.Water);
                break;

            case TileType.Wood:
                maxSize = Mathf.Clamp(Random.Range(maxForestSize / 2, maxForestSize), 1, maxForestSize);
                spread = GetSpread(resourceSpawn, maxSize);
                SpawnResources(resourceSpawn, spread, TileType.Wood);
                break;

            case TileType.Stone:
                maxSize = Mathf.Clamp(Random.Range(maxQuarySize / 2, maxQuarySize), 1, maxQuarySize);
                spread = GetSpread(resourceSpawn, maxSize);
                SpawnResources(resourceSpawn, spread, TileType.Stone);
                break;

            case TileType.Iron:

                break;
        }
    }

    //Spawns the resources in the game world
    private void SpawnResources(Coord center, Coord[] spread, TileType type)
    {
        //Spawn water tiles by first removing the grass tile
        if (type == TileType.Water)
        {
            Destroy(tiles[center.x][center.y]);
            tiles[center.x][center.y] = Instantiate(waterPrefab, new Vector3(center.x * Tags.TileSize, 0, center.y * Tags.TileSize), Quaternion.identity, transform);
            tiles[center.x][center.y].GetComponent<TileController>().Init(center.x, center.y, TileType.Water);

            for (int i = 0; i < spread.Length; i++)
            {
                if (spread[i].x != -1 && (spread[i].x != 0 && spread[i].y != 0))
                {
                    Destroy(tiles[spread[i].x][spread[i].y]);
                    tiles[spread[i].x][spread[i].y] = Instantiate(waterPrefab, new Vector3(spread[i].x * Tags.TileSize, 0, spread[i].y * Tags.TileSize), Quaternion.identity, transform);
                    tiles[spread[i].x][spread[i].y].GetComponent<TileController>().Init(spread[i].x, spread[i].y, TileType.Water);
                }
                else
                    break;
            }
        }
        //spawn objects on the map including trees, rocks and gold
        else
        {
            {
                GameObject resourceObject = null;

                switch (type)
                {
                    case TileType.Wood:
                        resourceObject = Instantiate(treePrefab, new Vector3(center.x * Tags.TileSize, 0, center.y * Tags.TileSize), Quaternion.identity, tiles[center.x][center.y].transform);
                        tiles[center.x][center.y].GetComponent<TileController>().Init(center, TileType.Wood, resourceObject);
                        resourceObject.GetComponent<Resource>().Init(200, 200, false, 60, 1);
                        break;

                    case TileType.Stone:
                        resourceObject = Instantiate(rockPrefab, new Vector3(center.x * Tags.TileSize, 0, center.y * Tags.TileSize), Quaternion.identity, tiles[center.x][center.y].transform);
                        resourceObject.GetComponent<Resource>().Init(200, 200, true, 60, 1);
                        tiles[center.x][center.y].GetComponent<TileController>().Init(center, TileType.Stone, resourceObject);
                        break;

                    case TileType.Iron:
                        resourceObject = Instantiate(ironPrefab, new Vector3(center.x * Tags.TileSize, 0, center.y * Tags.TileSize), Quaternion.identity, tiles[center.x][center.y].transform);
                        resourceObject.GetComponent<Resource>().Init(200, 200, true, 60, 1);
                        tiles[center.x][center.y].GetComponent<TileController>().Init(center, TileType.Iron, resourceObject);
                        break;
                }
            }

            //Spawn resources near an area
            for (int i = 0; i < spread.Length; i++)
            {
                if (spread[i].x != -1 && (spread[i].x != 0 && spread[i].y != 0))
                {
                    GameObject resourceObject = null;

                    switch (type)
                    {
                        case TileType.Wood:
                            resourceObject = Instantiate(treePrefab, new Vector3(spread[i].x * Tags.TileSize, 0, spread[i].y * Tags.TileSize), Quaternion.identity, tiles[spread[i].x][spread[i].y].transform);
                            tiles[spread[i].x][spread[i].y].GetComponent<TileController>().Init(spread[i], TileType.Wood, resourceObject);
                            resourceObject.GetComponent<Resource>().Init(200, 2, true, 60, 1);                            
                            break;

                        case TileType.Stone:
                            resourceObject = Instantiate(rockPrefab, new Vector3(spread[i].x * Tags.TileSize, 0, spread[i].y * Tags.TileSize), Quaternion.identity, tiles[spread[i].x][spread[i].y].transform);
                            resourceObject.GetComponent<Resource>().Init(200, 2, true, 60, 1);
                            tiles[spread[i].x][spread[i].y].GetComponent<TileController>().Init(spread[i], TileType.Stone, resourceObject);
                            break;

                        case TileType.Iron:
                            resourceObject = Instantiate(ironPrefab, new Vector3(spread[i].x * Tags.TileSize, 0, spread[i].y * Tags.TileSize), Quaternion.identity, tiles[spread[i].x][spread[i].y].transform);
                            resourceObject.GetComponent<Resource>().Init(200, 2, true, 60, 1);
                            tiles[spread[i].x][spread[i].y].GetComponent<TileController>().Init(spread[i], TileType.Iron, resourceObject);
                            break;
                    }

                }
                else
                    break;
            }
        }
    }

    //gets locations near the start coord to spread the resource out
    private Coord[] GetSpread(Coord startCoord, int spreadAmount)
    {
        if (spreadAmount == 0)
        {
            return null;
        }

        Coord[] spread = new Coord[spreadAmount];
        int currentSpreadSize = 0;


        for (int i = 0; i < Mathf.Clamp(4, 0, spread.Length); i++)
        {
            Coord emptyPoint = GetAdjacentEmptyTile(startCoord);
            if (emptyPoint.x != -1)
            {
                spread[currentSpreadSize++] = emptyPoint;

                tiles[emptyPoint.x][emptyPoint.y].GetComponent<TileController>().SetTileType(TileType.InUse);
            }
        }

        int failover = 10;

        //Keep searcing for empty tiles
        while (currentSpreadSize < spreadAmount)
        {
            for (int i = 0; i < currentSpreadSize; i++)
            {
                Coord emptyTile = GetAdjacentEmptyTile(spread[i]);

                if (emptyTile.x != -1)
                {
                    spread[currentSpreadSize++] = emptyTile;

                    tiles[emptyTile.x][emptyTile.y].GetComponent<TileController>().SetTileType(TileType.InUse);

                    if (currentSpreadSize >= spreadAmount)
                        break;
                }
            }

            failover--;

            if (failover < 0)
                break;

        }

        return spread;
    }

    //Returns a coord that points to an empty tile
    private Coord GetAdjacentEmptyTile(Coord point)
    {
        int rand = Random.Range(1, 4);

        switch (rand)
        {
            case 1:
                if (CheckRight(point))
                    return new Coord(point.x + 1, point.y);
                if (CheckUp(point))
                    return new Coord(point.x, point.y + 1);
                if (CheckLeft(point))
                    return new Coord(point.x - 1, point.y);
                if (CheckDown(point))
                    return new Coord(point.x, point.y - 1);
                break;

            case 2:
                if (CheckUp(point))
                    return new Coord(point.x, point.y + 1);
                if (CheckDown(point))
                    return new Coord(point.x, point.y - 1);
                if (CheckRight(point))
                    return new Coord(point.x + 1, point.y);
                if (CheckLeft(point))
                    return new Coord(point.x - 1, point.y);
                break;

            case 3:
                if (CheckLeft(point))
                    return new Coord(point.x - 1, point.y);
                if (CheckRight(point))
                    return new Coord(point.x + 1, point.y);
                if (CheckDown(point))
                    return new Coord(point.x, point.y - 1);
                if (CheckUp(point))
                    return new Coord(point.x, point.y + 1);
                break;

            case 4:
                if (CheckDown(point))
                    return new Coord(point.x, point.y - 1);
                if (CheckLeft(point))
                    return new Coord(point.x - 1, point.y);
                if (CheckUp(point))
                    return new Coord(point.x, point.y + 1);
                if (CheckRight(point))
                    return new Coord(point.x + 1, point.y);
                break;
        }
        return new Coord(-1, -1);
    }

    //Check for empty tiles
    public bool CheckRight(Coord point, TileType type = TileType.Empty)
    {
        if (IsInSpawnArea(point.x + 1, point.y))
            return false;

        if (point.x + 1 < maxGrid.x)
        {
            return (tiles[point.x + 1][point.y].GetComponent<TileController>().GetTileType() == type);
        }
        return false;
    }

    public bool CheckLeft(Coord point, TileType type = TileType.Empty)
    {
        if (IsInSpawnArea(point.x - 1, point.y))
            return false;

        if (point.x - 1 > 0)
        {
            return (tiles[point.x - 1][point.y].GetComponent<TileController>().GetTileType() == type);
        }
        return false;
    }

    public bool CheckUp(Coord point, TileType type = TileType.Empty)
    {
        if (IsInSpawnArea(point.x, point.y + 1))
            return false;

        if (point.y + 1 < maxGrid.y)
        {
            return (tiles[point.x][point.y + 1].GetComponent<TileController>().GetTileType() == type);
        }
        return false;
    }

    public bool CheckDown(Coord point, TileType type = TileType.Empty)
    {
        if (IsInSpawnArea(point.x, point.y - 1))
            return false;

        if (point.y - 1 >= 0)
        {
            return (tiles[point.x][point.y - 1].GetComponent<TileController>().GetTileType() == type);
        }
        return false;
    }

    //Checks if point is in spawn area
    private bool IsInSpawnArea(int x, int y)
    {
        return IsInSpawnArea(new Coord(x, y));
    }

    //Checks if the point is in the spawn area
    private bool IsInSpawnArea(Coord point)
    {
        return (point.x <= (spawnPoint.x + spawnSize) && point.x >= (spawnPoint.x - spawnSize)) && (point.y <= (spawnPoint.y + spawnSize) && point.y >= (spawnPoint.y - spawnSize));
    }

    //Get a radom coord missing a 1 + spawnsize * 2 square around the spawn point
    private Coord GetRandomCoord(Coord spawnPoint)
    {
        Coord newPos = new Coord();

        //if we fail to find a place over 100 times give up
        int failSafe = 100;

        do
        {
            do
            {
                newPos.x = Random.Range(0, maxGrid.x - 1);
                newPos.y = Random.Range(0, maxGrid.y - 1);

                failSafe--;

                //While the new position is in an empty space
            } while (IsInSpawnArea(newPos));

            if (failSafe == 0)
                return GetFirstEmptyTile();

            //While the position is empty
        } while (GetTypeAt(newPos) != TileType.Empty);

        return newPos;
    }

    //returns the first empty tile
    private Coord GetFirstEmptyTile()
    {
        for (int x = 0; x < maxGrid.x; x++)
        {
            for (int y = 0; y < maxGrid.y; y++)
            {
                if (GetTypeAt(new Coord(x, y)) == TileType.Empty)
                    return new Coord(x, y);
            }
        }

        return new Coord(-1, -1);
    }

    //Checks if our tile is connected to a specific type
    public bool isCloseTo(int x, int y, TileType type)
    {
        if (tiles.Count != 0)
        {
            if (tiles.Count < x + 1 && tiles.Count != 0)
            {
                if (tiles[x + 1][y].GetComponent<TileController>().GetTileType() == type)
                    return true;
            }

            if ((x - 1) >= 0)
            {
                if (tiles[x - 1][y].GetComponent<TileController>().GetTileType() == type)
                    return true;
            }
        }

        if (tiles[x].Count != 0)
        {
            if (tiles[x].Count < y + 1)
            {
                if (tiles[x][y + 1].GetComponent<TileController>().GetTileType() == type)
                    return true;
            }

            if (y - 1 >= 0)
            {
                if (tiles[x][y - 1].GetComponent<TileController>().GetTileType() == type)
                    return true;
            }
        }
        return false;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditor : MonoBehaviour
{

    MapGrid mapGrid;
    CameraController cameraRayCast;

    public Texture2D wreckCursor;

    public GameObject adjustableTileUI;
    public GameObject adjustableResourceUI;
    GameObject SizeUI;

    public GameObject villagerPrefab;

    //TODO: Add ghost buildings
    public GameObject[] ghostBuildings;
    //0 = tree, 1 = rock
    public GameObject[] ghostResource;

    //Spawn buildings
    private List<GameObject> ghostObject;
    int brushSize = 1;

    private BuildingType ghost = BuildingType.NULL;
    private ResourceType resource = ResourceType.Nothing;

    //Change map tiles
    private TileType tileType = TileType.InUse;

    //Wrecking
    public bool isWrecking = false;

    // Use this for initialization
    void Start()
    {
        mapGrid = GetComponent<MapGrid>();
        cameraRayCast = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<CameraController>();

        ghostObject = new List<GameObject>();
    }

    public bool hasLoaded()
    {
        return mapGrid.hasLoaded();
    }

    void Update()
    {
        if (!cameraRayCast.IsMouseOverUI())
            if (isWrecking && Input.GetMouseButton(0))
            {
                RaycastHit? rayHit = cameraRayCast.GetMouseHit();

                //if the ray hit something
                if (rayHit != null)
                {
                    TileController tile = rayHit.Value.collider.GetComponent<TileController>();

                    if (tile != null)
                    {
                        tile.DetroyConnectedObject();
                    }
                }
            }
            //If we are currently placing a building 
            else if (ghost != BuildingType.NULL)
            {
                RaycastHit? rayHit = cameraRayCast.GetMouseHit();

                //if the ray hit something and we are not currently clicking on the UI
                if (rayHit != null && !cameraRayCast.GetComponentInChildren<EventSystem>().IsPointerOverGameObject())
                {
                    TileController tile = rayHit.Value.collider.GetComponent<TileController>();

                    if (Input.GetMouseButtonDown(0) && CanSpawn(tile.location))
                    {
                        SpawnBuilding();
                    }
                    else
                    {
                        if (tile != null)
                            if (tile.connectedObject == null && tile.GetTileType() == TileType.Empty)
                            {
                                //Create the ghost object if we do not have one
                                if (ghostObject.Count == 0)
                                {
                                    ghostObject.Add(Instantiate(ghostBuildings[(int)ghost], tile.transform.position, Quaternion.identity));
                                    ghostObject[0].GetComponent<GhostController>().gridPosition = tile.location;
                                }
                                else
                                {
                                    //Move the ghost object
                                    ghostObject[0].transform.position = tile.transform.position;
                                    ghostObject[0].GetComponent<GhostController>().gridPosition = tile.location;

                                    //Can spawn = white, cannot spawn = red
                                    if (CanSpawn(tile.location))
                                    {
                                        ghostObject[0].GetComponentInChildren<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                                    }
                                    else
                                    {
                                        ghostObject[0].GetComponentInChildren<MeshRenderer>().material.color = new Color(1.0f, 0.1f, 0.1f, 0.5f);
                                    }
                                }
                            }
                    }
                }
            }
            else if (tileType != TileType.InUse)
            {
                RaycastHit? rayHit = cameraRayCast.GetMouseHit();

                if (rayHit != null)
                {
                    TileController tile = rayHit.Value.collider.GetComponent<TileController>();

                    if (Input.GetMouseButton(0))
                    {
                        SpawnObjectsFromGhost(tile.location);
                    }
                    else
                    {
                        if (ghostObject.Count == 0)
                        {
                            SpawnGhostObjects(tile.location);
                        }
                        else
                        {
                            //Move the ghost object
                            MoveGhostObjects(tile.location);
                        }
                    }
                }
            }
            else if (resource != ResourceType.Nothing)
            {
                RaycastHit? rayHit = cameraRayCast.GetMouseHit();

                if (rayHit != null)
                {
                    TileController tile = rayHit.Value.collider.GetComponent<TileController>();

                    if (Input.GetMouseButton(0))
                    {
                        SpawnObjectsFromGhost(tile.location);
                    }
                    else
                    {
                        if (ghostObject.Count == 0)
                        {
                            SpawnGhostObjects(tile.location);
                        }
                        else
                        {
                            //Move the ghost object
                            MoveGhostObjects(tile.location);
                        }
                    }
                }
            }

        //If we right click remove everything
        if (Input.GetMouseButtonDown(1))
        {
            SetWrecking(false);
        }
    }

    //Spawn ghost objects to the world
    private void SpawnObjectsFromGhost(Coord centerPosition)
    {
        if (brushSize == 1)
        {
            TileController tile = mapGrid.GetTileController(centerPosition);
            SetUpObject(tile);
        }
        else
        {
            int index = 0;

            for (int x = (int)centerPosition.x - brushSize; x <= centerPosition.x + brushSize; x++)
            {
                for (int y = (int)centerPosition.y - brushSize; y <= centerPosition.y + brushSize; y++)
                {
                    if (x >= 0 && x < mapGrid.mapSize.x && y >= 0 && y < mapGrid.mapSize.y)
                    {
                        TileController tile = mapGrid.GetTileController(new Coord(x, y));
                        SetUpObject(tile);

                        index++;
                    }
                }
            }
        }
    }

    //Set up spawned objects
    private void SetUpObject(TileController tile)
    {
        if (tile.GetTileType() == TileType.Empty)
            if (tileType != TileType.InUse)
            {
                if (tileType != tile.GetTileType())
                {
                    if (tileType == TileType.Water)
                    {
                        GameObject newTile = Instantiate(mapGrid.waterPrefab, tile.transform.position, Quaternion.identity, mapGrid.transform);
                        newTile.GetComponent<TileController>().Init(tile.location, TileType.Water);
                        mapGrid.ReplaceTile(tile.location, newTile);
                    }
                    else if (tileType == TileType.Empty)
                    {
                        GameObject newTile = Instantiate(mapGrid.tilePrefab, tile.transform.position, Quaternion.identity, mapGrid.transform);
                        newTile.GetComponent<TileController>().Init(tile.location, TileType.Empty);
                        mapGrid.ReplaceTile(tile.location, newTile);
                    }
                }
            }
            else if (resource != ResourceType.Nothing)
            {
                if (tile.connectedObject == null)
                {
                    GameObject resourceObject = null;
                    if (resource == ResourceType.Wood)
                    {
                        resourceObject = Instantiate(mapGrid.treePrefab, tile.transform.position, Quaternion.identity, tile.transform);
                        tile.Init(TileType.Wood, resourceObject);

                    }
                    else if (resource == ResourceType.Stone)
                    {
                        resourceObject = Instantiate(mapGrid.rockPrefab, tile.transform.position, Quaternion.identity, tile.transform);
                        tile.Init(TileType.Stone, resourceObject);
                    }

                    if (resourceObject != null)
                    {
                        ResourceAmountController amountcontroller = SizeUI.GetComponent<ResourceAmountController>();
                        resourceObject.GetComponent<Resource>().Init((int)amountcontroller.maxSlider.value, (int)amountcontroller.maxSlider.value, amountcontroller.regenToggle.isOn, amountcontroller.regenSpeedSlider.value, (int)amountcontroller.regenAmountSlider.value);
                    }
                }
            }
    }

    //Create ghost objects
    private void SpawnGhostObjects(Coord centerPosition)
    {
        GameObject prefab = null;

        //Get the prefab we need
        if (tileType != TileType.InUse)
        {
            if (tileType == TileType.Water)
            {
                prefab = mapGrid.waterPrefab;
            }
            else if (tileType == TileType.Empty)
            {
                prefab = mapGrid.tilePrefab;
            }
            brushSize = (int)SizeUI.GetComponentInChildren<SizeTileController>().CurrentValue;
        }
        else if (resource != ResourceType.Nothing)
        {
            if (resource == ResourceType.Wood)
            {
                prefab = mapGrid.treePrefab;
            }
            else if (resource == ResourceType.Stone)
            {
                prefab = mapGrid.rockPrefab;
            }
            brushSize = (int)SizeUI.GetComponent<ResourceAmountController>().sizeSlider.value;
        }

        //If we have a prefab then spawn the ghost objects
        if (prefab != null)
        {
            if (brushSize == 1)
            {
                ghostObject.Clear();
                ghostObject.Add(Instantiate(prefab, new Vector3(centerPosition.x * Tags.TileSize, 0, centerPosition.y * Tags.TileSize), Quaternion.identity));
            }
            else
            {
                for (int x = (int)centerPosition.x - brushSize; x <= centerPosition.x + brushSize; x++)
                {
                    for (int y = (int)centerPosition.y - brushSize; y <= centerPosition.y + brushSize; y++)
                    {
                        ghostObject.Add(Instantiate(prefab, new Vector3(Tags.TileSize * x, 0, Tags.TileSize * y), Quaternion.identity));
                    }
                }
            }

            //Not we have the objects they need setting up to be ghost objects
            if (tileType != TileType.InUse)
            {
                if (tileType == TileType.Water || tileType == TileType.Empty)
                {
                    foreach (GameObject g in ghostObject)
                    {
                        Destroy(g.GetComponent<BoxCollider>());
                    }
                }
            }
            else if (resource != ResourceType.Nothing)
            {
                if (resource == ResourceType.Wood || resource == ResourceType.Stone)
                {
                    foreach (GameObject g in ghostObject)
                    {
                        g.GetComponent<Resource>().IsItem();
                    }
                }
            }
        }
    }

    //Moves ghost objects to the mouse
    private void MoveGhostObjects(Coord centerPosition)
    {
        if (brushSize == 1)
        {
            ghostObject[0].transform.position = new Vector3(Tags.TileSize * centerPosition.x, 0, Tags.TileSize * centerPosition.y);
        }
        else
        {
            int index = 0;

            for (int x = (int)centerPosition.x - brushSize; x <= centerPosition.x + brushSize; x++)
            {
                for (int y = (int)centerPosition.y - brushSize; y <= centerPosition.y + brushSize; y++)
                {
                    ghostObject[index++].transform.position = new Vector3(Tags.TileSize * x, 0, Tags.TileSize * y);
                }
            }
        }
    }

    //Update the brush size
    public void SetBrushSize()
    {
        foreach (GameObject g in ghostObject)
        {
            Destroy(g);
        }

        ghostObject.Clear();

        SpawnGhostObjects(new Coord(mapGrid.mapSize.x, mapGrid.mapSize.y));
    }

    //Can we spawn the building in this location
    public bool CanSpawn(Coord newPosition)
    {
        //If we do not have a ghost object 
        if (ghostObject.Count == 0 || ghostObject[0] == null)
        {
            return false;
        }

        GhostController ghostC = ghostObject[0].GetComponent<GhostController>();
        Coord buildingSize = ghostC.placementSize;

        //Make sure the bulding is fully on the map
        if ((newPosition.x - buildingSize.x) < 0 || (newPosition.y - buildingSize.y) < 0)
            return false;

        //If the building is only on top of empty tiles
        for (int x = newPosition.x; x > (newPosition.x - buildingSize.x); x--)
        {
            for (int y = newPosition.y; y > (newPosition.y - buildingSize.y); y--)
            {
                if (!mapGrid.IsEmpty(new Coord(x, y)))
                {
                    return false;
                }
            }
        }
        return true;
    }

    //Are we removing objects
    public void SetWrecking(bool toSet)
    {
        Clear();

        isWrecking = toSet;

        if (isWrecking)
        {
            Cursor.SetCursor(wreckCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    //Set our ghost building we want to set
    public void SetGhost(BuildingType buildType)
    {
        if (buildType != ghost)
        {
            Clear();
            ghost = buildType;
        }
        else
            Clear();
    }

    //Remove the ghost building
    public void RemoveGhost()
    {
        foreach (GameObject g in ghostObject)
        {
            Destroy(g);
        }

        ghostObject.Clear();

        ghost = BuildingType.NULL;
    }

    public void SetResource(ResourceType resType)
    {
        if (resource != resType)
        {
            Clear();
            resource = resType;
            CreateResourceUI();
        }
        else
            Clear();
    }

    public void RemoveResource()
    {
        resource = ResourceType.Nothing;
    }

    //Sets the tile we want to start placing
    public void SetTileGhost(TileType newTileType)
    {
        if (newTileType != tileType)
        {
            Clear();
            tileType = newTileType;
            CreateTileUI();
        }
        else
            Clear();
    }

    public void Clear()
    {
        RemoveGhost();
        RemoveGhostTile();
        RemoveUI();
        RemoveResource();
    }

    private void CreateTileUI()
    {
        if (SizeUI != null)
            RemoveUI();

        SizeUI = Instantiate(adjustableTileUI, GameObject.FindGameObjectWithTag(Tags.Canvas).transform);

    }

    private void CreateResourceUI()
    {
        if (SizeUI != null)
            RemoveUI();

        SizeUI = Instantiate(adjustableResourceUI, GameObject.FindGameObjectWithTag(Tags.Canvas).transform);
    }

    private void RemoveUI()
    {
        if (SizeUI != null)
        {
            Destroy(SizeUI);
        }
    }

    //Removes the ghost tile
    public void RemoveGhostTile()
    {
        tileType = TileType.InUse;
    }

    //Touch all tiles the building is on top of
    public Coord[] TouchTiles(Coord position, Coord buildingSize, GameObject building)
    {
        int size = (buildingSize.x + buildingSize.y) - 1;
        Coord[] tilesTouched = new Coord[size];
        int touch = 0;

        for (int x = position.x; x > (position.x - buildingSize.x); x--)
        {
            for (int y = position.y; y > (position.y - buildingSize.y); y--)
            {
                //Don't want to touch the main position this is the spawn position
                if (x != position.x || y != position.y)
                {
                    tilesTouched[touch] = new Coord(x, y);
                    mapGrid.GetTileController(tilesTouched[touch]).Init(tilesTouched[touch], TileType.BuildingArea, building);
                    touch++;
                }
            }
        }
        return tilesTouched;
    }

    //Spawn a building onto a tile
    public void SpawnBuilding()
    {
        GameObject newBuilding;

        //Switch can be replaced with when all buildings are added
        //newBuilding = Instantiate(mapGrid.buildings[(int)ghost]

        //Buildings can only have one object
        TileController mainTile = mapGrid.GetTileAt(ghostObject[0].GetComponent<GhostController>().gridPosition).GetComponent<TileController>();

        switch (ghost)
        {
            case BuildingType.House:
                newBuilding = Instantiate(mapGrid.buildings[(int)BuildingType.House], mainTile.transform.position, Quaternion.identity, mainTile.transform);
                newBuilding.GetComponent<BuildingController>().Init(mainTile.location, BuildingType.House);
                mainTile.Init(mainTile.location, TileType.Building, newBuilding);
                break;
            case BuildingType.Sawmill:
                newBuilding = Instantiate(mapGrid.buildings[(int)BuildingType.Sawmill], mainTile.transform.position, Quaternion.identity, mainTile.transform);
                newBuilding.GetComponent<BuildingController>().Init(mainTile.location, BuildingType.Sawmill, TouchTiles(mainTile.location, ghostObject[0].GetComponent<GhostController>().placementSize, newBuilding));
                mainTile.Init(mainTile.location, TileType.Building, newBuilding);
                break;
            case BuildingType.StoneCutter:
                newBuilding = Instantiate(mapGrid.buildings[(int)BuildingType.Sawmill], mainTile.transform.position, Quaternion.identity, mainTile.transform);
                newBuilding.GetComponent<BuildingController>().Init(mainTile.location, BuildingType.Sawmill, TouchTiles(mainTile.location, ghostObject[0].GetComponent<GhostController>().placementSize, newBuilding));
                mainTile.Init(mainTile.location, TileType.Building, newBuilding);
                break;
            case BuildingType.IronMine:
                newBuilding = Instantiate(mapGrid.buildings[(int)BuildingType.Sawmill], mainTile.transform.position, Quaternion.identity, mainTile.transform);
                newBuilding.GetComponent<BuildingController>().Init(mainTile.location, BuildingType.Sawmill, TouchTiles(mainTile.location, ghostObject[0].GetComponent<GhostController>().placementSize, newBuilding));
                mainTile.Init(mainTile.location, TileType.Building, newBuilding);
                break;
            case BuildingType.Storage:
                newBuilding = Instantiate(mapGrid.buildings[(int)BuildingType.Sawmill], mainTile.transform.position, Quaternion.identity, mainTile.transform);
                newBuilding.GetComponent<BuildingController>().Init(mainTile.location, BuildingType.Sawmill, TouchTiles(mainTile.location, ghostObject[0].GetComponent<GhostController>().placementSize, newBuilding));
                mainTile.Init(mainTile.location, TileType.Building, newBuilding);
                break;
            case BuildingType.NULL:
                Clear();
                break;
            default:
                break;
        }

        Clear();
    }

    //Sets a tile back to default
    public void ClearTile(Coord clearCoord)
    {
        TileController toClear = mapGrid.GetTileController(clearCoord);

        if (toClear != null)
        {
            toClear.RemoveConnectedObject();
        }
    }

    //Clear and array of coords
    public void ClearTiles(Coord[] clearCoords)
    {
        foreach (Coord c in clearCoords)
            ClearTile(c);
    }

    //Delete an object connected to a tile
    public void DeleteObject(Coord position)
    {
        TileController toRemove = mapGrid.GetTileController(position);

        if (toRemove != null)
        {
            toRemove.RemoveConnectedObject();
        }
    }
}

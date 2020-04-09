using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBrain : MonoBehaviour
{
    //time takes to move between tiles
    private float hopTimeLeft = 1.0f;
    private float secondsToHop = 1.0f;

    public ResourceType heldResource = ResourceType.Nothing;
    public int carryResourceAmount = 0;
    public int maxCarryAmount = 5;
    public GameObject carringObject;

    public Coord gridLocation;
    public bool started = false;

    //Gathering
    private TileType gatheringResource;

    //Pathfinding
    private MapGrid map;
    private PathFinder pathfinder;
    private int pathPosition;
    public List<Coord> pathTiles;
    public Resource gatherLocation;
    private GameObject depositLocation;

    //Moving vectors
    public Vector3 startPosition;
    public Vector3 jumpToPosition;

    //Delay if we can not find a path before searching again
    bool waitForPathfinding = false;
    float waitMaxTime = 5.0f;
    float waitTime = 5.0f;


    void pathFailed()
    {
        waitForPathfinding = true;
        waitTime = waitMaxTime;
    }

    public void Start()
    {
        //Map grid so we can get objects we need
        map = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapGrid>();

        //Pathfinder 
        pathfinder = GetComponent<PathFinder>();

        //Give us a little delay before pathfinding
        pathFailed();
    }

    public void Update()
    {
        if(map.hasLoaded())
            if (started && !waitForPathfinding)
            {
                //If we do not have a path
                if (pathTiles == null || pathTiles.Count == 0)
                {
                    if (carryResourceAmount == 0)
                        FindClosestResource();
                    else
                        FindDepositBuilding();
                }
                else
                {
                    if (carryResourceAmount == 0)
                        MoveToGatherLocation();
                    else
                        MoveToDepositLocation();
                }
            }
            else if(waitForPathfinding)
            {
                waitTime -= Time.deltaTime;
                if(waitTime <= 0)
                {
                    waitForPathfinding = false;
                }
            }
    }

    //Deposit the resource to the building
    private void depositResource()
    {
        depositLocation.GetComponent<BuildingController>().Deposit(carryResourceAmount);
        carryResourceAmount = 0;
        secondsToHop = 1.0f;

        if (carringObject != null)
            Destroy(carringObject);
    }

    //Gather a resource from the map
    private void gatherResource()
    {
        carryResourceAmount += gatherLocation.ExtractResource(maxCarryAmount - carryResourceAmount);
        secondsToHop = 1.0f + (carryResourceAmount / 2);

        switch (heldResource)
        {
            case ResourceType.Wood:
                carringObject = Instantiate(map.treePrefab, transform.position + new Vector3(0, GetComponentInChildren<Renderer>().bounds.size.y, 0), Quaternion.identity, transform);
                break;
            case ResourceType.Stone:
                carringObject = Instantiate(map.rockPrefab, transform.position + new Vector3(0, GetComponentInChildren<Renderer>().bounds.size.y, 0), Quaternion.identity, transform);
                break;
            case ResourceType.Gold:
                break;
            case ResourceType.Food:
                break;
            case ResourceType.Water:
                carringObject = Instantiate(map.waterPrefab, transform.position + new Vector3(0, GetComponentInChildren<Renderer>().bounds.size.y, 0), Quaternion.identity, transform);
                break;
            case ResourceType.Planks:
                break;
            default:
                break;
        }

        if(carringObject != null)
        {
            carringObject.GetComponent<Resource>().IsItem();
            carringObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    //Sets all the initial values that we need
    public void Init(Coord villagerLocation, GameObject depositLoc, TileType gatheringRes, ResourceType resType)
    {
        heldResource = resType;
        gridLocation = villagerLocation;
        depositLocation = depositLoc;
        gatheringResource = gatheringRes;
        started = true;
    }

    //Get a path to the closest resource
    private void FindClosestResource()
    {
        pathTiles = pathfinder.GetPath(gridLocation, gatheringResource);

        if (pathTiles != null)
        {
            gatherLocation = map.GetResourceAtTile(pathTiles[pathTiles.Count - 1]);
            pathPosition = 0;
            hopTimeLeft = 1.0f;

            startPosition = transform.position;
            jumpToPosition = new Vector3(pathTiles[pathPosition].x * Tags.TileSize, 0, pathTiles[pathPosition].y * Tags.TileSize);
        }
        else
        {
            pathFailed();
        }
    }

    //Find the closest part of our designated building
    private void FindDepositBuilding()
    {
        if (depositLocation != null)
        {
            BuildingController tControl = depositLocation.GetComponent<BuildingController>();
            pathTiles = pathfinder.GetPath(gridLocation, tControl.GetPosition(), tControl.GetOtherPostions());
            if (pathTiles != null)
            {
                hopTimeLeft = 1.0f;
                pathPosition = 0;
                startPosition = transform.position;
                jumpToPosition = new Vector3(pathTiles[pathPosition].x * Tags.TileSize, 0, pathTiles[pathPosition].y * Tags.TileSize);
            }
            else
            {
                pathFailed();
            }

        }
        else
            Destroy(this);
    }

    //Move to our gather location
    public void MoveToGatherLocation()
    {
        //2 seconds to hop to tiles
        hopTimeLeft -= (Time.deltaTime / secondsToHop);

        if (hopTimeLeft <= 0)
        {
            if (pathPosition == pathTiles.Count - 1)
            {
                gatherResource();
                pathTiles.Clear();
                pathTiles = null;
            }
            else
            {
                gridLocation = pathTiles[pathPosition];
                pathPosition++;

                hopTimeLeft = 1.0f;
                startPosition = jumpToPosition;
                jumpToPosition = new Vector3(pathTiles[pathPosition].x * Tags.TileSize, 0, pathTiles[pathPosition].y * Tags.TileSize);
            }
        }
        else if (pathPosition < pathTiles.Count - 1)
        {
            transform.position = Vector3.Lerp(startPosition, jumpToPosition, Mathf.Abs(hopTimeLeft - 1));
        }
    }

    //Move to the deposit location
    public void MoveToDepositLocation()
    {
        //2 seconds to hop to tiles
        hopTimeLeft -= (Time.deltaTime / secondsToHop);

        if (hopTimeLeft <= 0)
        {
            if (pathPosition == pathTiles.Count - 1)
            {
                depositResource();
                pathTiles.Clear();
                pathTiles = null;
            }
            else
            {
                gridLocation = pathTiles[pathPosition];
                pathPosition++;

                hopTimeLeft = 1.0f;
                startPosition = jumpToPosition;
                jumpToPosition = new Vector3(pathTiles[pathPosition].x * Tags.TileSize, 0, pathTiles[pathPosition].y * Tags.TileSize);
            }
        }
        else if (pathPosition < pathTiles.Count - 1)
        {
            transform.position = Vector3.Lerp(startPosition, jumpToPosition, Mathf.Abs(hopTimeLeft - 1));
        }
    }

    public void OnDestroy()
    {
        if (carringObject != null)
            Destroy(carringObject);
    }
}

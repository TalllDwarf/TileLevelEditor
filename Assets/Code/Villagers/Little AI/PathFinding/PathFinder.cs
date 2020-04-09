using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PathFinder : MonoBehaviour {

    MapGrid mapGridGen;

	// Use this for initialization
	void Start () {
        mapGridGen = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapGrid>();
	}

    //TODO: Get path from A-B return List<Coord>
    public List<Coord> GetPath(Coord from, Coord to)
    {
        //Create the lists we need
        ReadOnlyCollection<List<GameObject>> mapGrid = mapGridGen.GetMapTiles;
        List<Node> touchedNode = new List<Node>();

        touchedNode.Add(new Node(from, GetTotalDistanceCost(from, to), 1));
        touchedNode[0].state = NodeState.Closed;

        //Get the first node we need
        int nodePosition = 0;
       
        while (touchedNode[nodePosition].distanceToEnd != 0)
        {
            GetAdjacentNodes(mapGrid, ref touchedNode, nodePosition, to);
            touchedNode[nodePosition].state = NodeState.Closed;

            nodePosition = -1;

            //get next node that has not been tested
            for(int index = 0; index < touchedNode.Count; index++)
            {                
                if(touchedNode[index].state == NodeState.Open)
                {
                    if(nodePosition == -1 || touchedNode[nodePosition].distanceToEnd > touchedNode[index].distanceToEnd)
                    {
                        nodePosition = index;
                    }                    
                }
            }

            if (nodePosition == -1)
                return null;
        }

        List<Coord> finalPath = CalculatePath(ref touchedNode, nodePosition);

        touchedNode.Clear();
        touchedNode = null;

        return finalPath;
    }

    //Gets a path to the closest tile of a building
    public List<Coord> GetPath(Coord from, Coord to, Coord[] extendedTo)
    {
        Coord endCoord = to;

        if(extendedTo != null)
            foreach(Coord endPosition in extendedTo)
            {
                if (GetTotalDistanceCost(from, endPosition) < GetTotalDistanceCost(from, endCoord))
                    endCoord = endPosition;
            }

        return GetPath(from, endCoord);
    }


    //Calculates the path from the list of nodes
    //since the path is backwards
    private List<Coord> CalculatePath(ref List<Node> touched, int finalNode)
    {
        List<Coord> reversePath = new List<Coord>();

        Node nextNode = touched[finalNode];

        do
        {
            reversePath.Add(nextNode.position);
            nextNode = touched[nextNode.GetParent()];
        }
        //-1 means no parent, the only one without a parent should be the starting position
        while (nextNode.GetParent() != -1);

        List<Coord> path = new List<Coord>();
        for(int index = reversePath.Count-1; index >= 0; index--)
        {
            path.Add(reversePath[index]);
        }

        return path;
    }

    //Get all adjacent nodes to the 
    private void GetAdjacentNodes(ReadOnlyCollection<List<GameObject>> map, ref List<Node> touched, int currentNode, Coord destination)
    {
        Coord[] adjacent = new Coord[4];
        //Up - Down = Left - Right
        adjacent[0] = new Coord(touched[currentNode].position.x, touched[currentNode].position.y + 1);
        adjacent[1] = new Coord(touched[currentNode].position.x, touched[currentNode].position.y - 1);
        adjacent[2] = new Coord(touched[currentNode].position.x - 1, touched[currentNode].position.y);
        adjacent[3] = new Coord(touched[currentNode].position.x + 1, touched[currentNode].position.y);

        foreach(Coord c in adjacent)
        {
            if(c.x >= 0 && c.x < mapGridGen.mapSize.x && c.y >= 0 && c.y < mapGridGen.mapSize.y)
            {
                if ((map[c.x][c.y].GetComponent<TileController>().GetTileType() != TileType.Water && (map[c.x][c.y].GetComponent<TileController>().GetTileType() != TileType.Building) && (map[c.x][c.y].GetComponent<TileController>().GetTileType() != TileType.BuildingArea)) || (c.x == destination.x && c.y == destination.y))
                {
                    int found = -1;

                    for (int index = 0; index < touched.Count; index++)
                    {
                        if (touched[index].position == c)
                        {
                            found = index;
                            break;
                        }
                    }

                    if (found != -1)
                    {
                        if((touched[currentNode].moveCost + 1) < touched[found].moveCost)
                            touched[found].SetParentIndex(currentNode, touched[currentNode].moveCost + 1);
                    }
                    else
                    {
                        touched.Add(new Node(c, currentNode, GetTotalDistanceCost(c, destination), touched[currentNode].moveCost + 1));
                    }
                }         
            }
        }
    }

    //Gets the move cost from one tile to another
    public int GetTotalDistanceCost(Coord a, Coord b)
    {
        return Mathf.Abs(Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y));
    }

    //TODO: Get shortest path to a building 
    //Not 100% working gets closest on the grid without looking for tiles we cannot walk on
    public List<Coord> GetPath(Coord from, BuildingType typeToGetTo)
    {
        //TODO: Set all tags on buildings as buildings
        GameObject[] buildings = GameObject.FindGameObjectsWithTag(Tags.Building);
        int index = -1;

        for(int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i].GetComponent<BuildingController>().GetBuilingType() == typeToGetTo)
            {
                if (index == -1 || GetTotalDistanceCost(from, buildings[i].GetComponent<BuildingController>().GetBuildingMainPosition()) < GetTotalDistanceCost(from, buildings[index].GetComponent<BuildingController>().GetBuildingMainPosition()))
                {
                    index = i;
                }
            }
        }

        if (index == -1)
            return null;

        return GetPath(from, buildings[index].GetComponent<BuildingController>().GetBuildingMainPosition());

    } 

    public List<Coord> GetPath(Coord from, TileType typeToGetTo)
    {
        //Create the lists we need
        ReadOnlyCollection<List<GameObject>> mapGrid = mapGridGen.GetMapTiles;
        List<Node> touchedNode = new List<Node>();

        touchedNode.Add(new Node(from, GetTotalDistanceCost(new Coord(0,1), new Coord(1,0)), 1));

        //Get the first node we need
        int nodePosition = 0;

        while (mapGridGen.GetTileController(touchedNode[nodePosition].position).GetTileType() != typeToGetTo)
        {
            GetAdjacentNodes(mapGrid, ref touchedNode, nodePosition, new Coord(-1,-1));
            touchedNode[nodePosition].state = NodeState.Closed;

            nodePosition = -1;

            //get next node that has not been tested
            for (int index = 0; index < touchedNode.Count; index++)
            {
                if (touchedNode[index].state == NodeState.Open)
                {
                    if (nodePosition == -1)
                    {
                        nodePosition = index;
                        break;
                    }
                }
            }

            if (nodePosition == -1)
                return null;
        }


        return GetPath(from, touchedNode[nodePosition].position);
    }

    public GameObject[] GetResourceTiles(TileType typeToGet)
    {
        Coord map = mapGridGen.mapSize;
        List<GameObject> tilesWithResource = new List<GameObject>();

        for(int x = 0; x < map.x; x++)
        {
            for(int y = 0; y < map.y; y++)
            {
                if (mapGridGen.GetTileController(new Coord(x,y)).GetTileType() == typeToGet)
                {
                    tilesWithResource.Add(mapGridGen.GetTileAt(new Coord(x,y)));
                }
            }
        }

        return tilesWithResource.ToArray();
    }
    
}

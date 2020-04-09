using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    public Coord location;

    public TileType type;

    public GameObject connectedObject;
    public MeshRenderer[] meshRender;

    private bool oldHit = false;
    private bool hit = false;

    public void Start()
    {
        FindconnectedRenderes();
    }

    //Init the variables for the tile
    public void Init(int x, int y, TileType newType, GameObject connectedO = null)
    {
        location = new Coord(x, y);

        type = newType;
        connectedObject = connectedO;
        FindconnectedRenderes();
    }

    public void Init(Coord _location, TileType newType, GameObject connectedO = null)
    {
        location = _location;

        type = newType;
        connectedObject = connectedO;
        FindconnectedRenderes();
    }

    public void Init(TileType newType, GameObject connected0 = null)
    {
        type = newType;
        connectedObject = connected0;
        FindconnectedRenderes();
    }

    //Adds the child object to the tile
    public void AddChildObject(GameObject child)
    {
        connectedObject = child;

        if(connectedObject != null)
        {
            FindconnectedRenderes();
        }
    }

    //Find the mesh renderes for the 
    private void FindconnectedRenderes()
    {
        meshRender = GetComponentsInChildren<MeshRenderer>();
    }

    //Returns the resource connected to the child
    public Resource GetConnectedResource()
    {
        if(type == TileType.Iron || type == TileType.Stone || type == TileType.Wood || type == TileType.Water)
            return GetComponentInChildren<Resource>();

        return null;
    }

    //Updates ever frame
    public void Update()
    {
       if(oldHit == true && hit == false)
        {
            meshRender = null;
            FindconnectedRenderes();

            //Not Hovering
            RemoveHit();
        
            oldHit = hit;
            hit = false;
        }
        else if(hit == true)
        {
            oldHit = hit;
            hit = false;
        }
        else
        {
            oldHit = hit;
        }
    }

    //When the tile has been hit glow
    public void HitByRay()
    {
        hit = true;

        meshRender = null;
        FindconnectedRenderes();

        //mouse hover
        Hit();
    }

    //We have been hit by the mouse
    private void Hit()
    {
        foreach(MeshRenderer r in meshRender)
        {
            foreach(Material m in r.materials)
            {
                m.SetFloat("_BlendAlpha", 1);
            }
        }
    }

    //We are no longer hit 
    private void RemoveHit()
    {
        foreach (MeshRenderer r in meshRender)
        {
            foreach (Material m in r.materials)
            {
                m.SetFloat("_BlendAlpha", 0);
            }
        }
    }

    //Update the tile type
    public void SetTileType(TileType newType)
    {
        type = newType;
    }

    //return the current tile type
    public TileType GetTileType()
    {
        return type;
    }

    public BuildingType GetBuildingType()
    {
        if (connectedObject == null)
            return BuildingType.NULL;

        BuildingController bc = connectedObject.GetComponent<BuildingController>();

        if (bc != null)
            return bc.GetBuilingType();

        return BuildingType.NULL;

    }

    //Destroy any building/resouce attached to this tile
    public void DetroyConnectedObject()
    {
        Destroy(connectedObject);

        RemoveConnectedObject();
    }

    public void RemoveConnectedObject()
    {
        connectedObject = null;
        type = TileType.Empty;
    }

    private void OnDestroy()
    {
        Destroy(connectedObject);
    }
}

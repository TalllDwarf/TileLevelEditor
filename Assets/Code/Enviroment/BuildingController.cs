using UnityEngine;

public class BuildingController : MonoBehaviour {

    protected Coord position;
    protected BuildingType buildType = BuildingType.House;

    protected Coord[] usingPosition;

    protected MapEditor mapEditor;

    public virtual void Init(Coord spawnPosition, BuildingType bType, Coord[] otherPositions = null)
    {
        position = spawnPosition;
        buildType = bType;
        usingPosition = otherPositions;

        mapEditor = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapEditor>();
    }

    public Coord GetPosition()
    {
        return position;
    }

    public Coord[] GetOtherPostions()
    {
        return usingPosition;
    }

    public BuildingType GetBuilingType()
    {
        return buildType;
    }

    public Coord GetBuildingMainPosition()
    {
        return position;
    }

    public virtual void SpawnVillager()
    {

    }

    public virtual void Deposit(int depositAmount)
    {

    }

    private void OnDestroy()
    {
        if(mapEditor != null && usingPosition != null)
        {
            //Tell the tiles we are on that we have been destroyed
            mapEditor.ClearTile(position);
            mapEditor.ClearTiles(usingPosition);
        }
    }

}

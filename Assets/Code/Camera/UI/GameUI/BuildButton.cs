using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : RadialButtonMenu {

    public BuildButtons buttonType = BuildButtons.Wreck;
    Button connectedButton;
    MapEditor mapE;

    protected override void Start()
    {
        base.Start();

        connectedButton = GetComponent<Button>();
        mapE = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapEditor>();

        switch (buttonType)
        {
            case BuildButtons.House:
                connectedButton.onClick.AddListener(() => Build(BuildingType.House));
                break;
            case BuildButtons.Saw:
                connectedButton.onClick.AddListener(() => Build(BuildingType.Sawmill));
                break;
            case BuildButtons.Stone:
                connectedButton.onClick.AddListener(() => Build(BuildingType.Sawmill));
                break;
            case BuildButtons.Mine:
                connectedButton.onClick.AddListener(() => Build(BuildingType.Sawmill));
                break;
            case BuildButtons.Bucket:
                connectedButton.onClick.AddListener(() => Build(BuildingType.Sawmill));
                break;
            case BuildButtons.Grass:
                connectedButton.onClick.AddListener(() => SwapTile(TileType.Empty));
                break;
            case BuildButtons.Water:
                connectedButton.onClick.AddListener(() => SwapTile(TileType.Water));
                break;
            case BuildButtons.Tree:
                connectedButton.onClick.AddListener(() => SetResource(ResourceType.Wood));
                break;
            case BuildButtons.Rock:
                connectedButton.onClick.AddListener(() => SetResource(ResourceType.Stone));
                break;
            case BuildButtons.Wreck:
                connectedButton.onClick.AddListener(() => Wreck());
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    public void SetResource(ResourceType type)
    {
        mapE.SetResource(type);
    }

    public void Build(BuildingType toBuild)
    {
        mapE.SetGhost(toBuild);
    }

    public void SwapTile(TileType toSwap)
    {
        mapE.SetTileGhost(toSwap);
    }

    public void Wreck()
    {
        mapE.SetWrecking(true);
    }
        

}

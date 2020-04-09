using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {


    public Vector2 placementS;

    public Coord placementSize;
    public Coord gridPosition;

    void Start()
    {
        placementSize = new Coord(placementS);
    }

}

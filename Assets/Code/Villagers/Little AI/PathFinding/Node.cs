using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Coord position;
    public int distanceToEnd;
    public int moveCost;
    public NodeState state = NodeState.Open;

    int parentIndex = -1;

    public int totalMoveCost
    {
        get
        {
            return (distanceToEnd + moveCost);
        }
    }

    public Node(Coord _position, int _parent, int _distanceToEnd, int _moveCost)
    {
        position = _position;
        parentIndex = _parent;
        distanceToEnd = _distanceToEnd;
        moveCost = _moveCost;
    }

    public Node(Coord _position, int _distanceToEnd, int _moveCost)
    {
        position = _position;
        distanceToEnd = _distanceToEnd;
        moveCost = _moveCost;
    }

    public void SetParentIndex(int newParaent, int _moveCost)
    {
        parentIndex = newParaent;
    }

    public int GetParent()
    {
        return parentIndex;
    }
}

public enum NodeState
{
    Open,
    Closed
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkMessage{

    public NetworkMessageID messageID;
    public Coord position;
    public BuildingType building;
    public ResourceType resource;

    public NetworkMessage()
    {
        messageID = NetworkMessageID.NULL;
    }

    public NetworkMessage(NetworkMessageID message, Coord positionP, BuildingType buildingT)
    {
        messageID = message;
        position = positionP;
        building = buildingT;
    }

    public NetworkMessage(NetworkMessageID message, Coord positionP, ResourceType resourceT)
    {
        messageID = message;
        position = positionP;
        resource = resourceT;
    }

}

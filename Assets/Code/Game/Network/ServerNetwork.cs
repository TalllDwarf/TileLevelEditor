using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerNetwork : NetworkBase
{
    private Queue<MapTileSave> m_Map = new Queue<MapTileSave>();
    private MapGrid mapGrid;

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    protected override void InitialiseUdpServer()
    {

    }

    protected override void Receive(IAsyncResult result)
    {
        throw new NotImplementedException();
    }
}

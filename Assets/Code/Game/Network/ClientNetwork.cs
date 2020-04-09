using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


public class ClientNetwork : NetworkBase
{
    private List<MapTileSave> mapSync;
    private MapGrid mapGrid;

    public List<NetworkMessage> messageList = new List<NetworkMessage>();

    public override void Start()
    {
        mapGrid = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapGrid>();

        InitialiseUdpServer();

        mSocket.BeginReceive(new AsyncCallback(Receive), mSocket);
    }

    //Use update to send any changes to the server
    public override void Update()
    {
        
    }

    protected override void InitialiseUdpServer()
    {
        try
        {
            IPAddress ipAddr = IPAddress.Parse(mIpAddress);
            mIpAddress = ipAddr.ToString();
            mEndpoint = new IPEndPoint(IPAddress.Parse(mIpAddress), mOutgoingPort);

            mSocket = new UdpClient(mIncomingPort);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    protected override void Receive(IAsyncResult result)
    {
        throw new NotImplementedException();
    }

    public void SetServerByIPAddress(String ipAddr)
    {
        mEndpoint.Address = IPAddress.Parse(ipAddr);
        mIpAddress = ipAddr;
    }
}

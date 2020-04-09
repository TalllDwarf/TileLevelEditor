using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NetworkBase : MonoBehaviour {

    [SerializeField]
    protected int mOutgoingPort;

    [SerializeField]
    protected int mIncomingPort;

    [SerializeField]
    protected Text mText;

    [SerializeField]
    protected string mIpAddress;

    protected UdpClient mSocket = null;
    protected IPEndPoint mEndpoint = null;

    protected abstract void InitialiseUdpServer();
    protected abstract void Receive(IAsyncResult result);
    public abstract void Start();
    public abstract void Update();
}

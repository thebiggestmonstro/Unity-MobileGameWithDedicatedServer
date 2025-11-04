using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkClient : MonoBehaviour, INetEventListener
{
    private NetManager _netManager;
    private NetPeer _server;
    private NetDataWriter _writer;

    private static NetworkClient _instance;

    public static NetworkClient Instance { get { return _instance; } }

    public void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        { 
            _instance = this;
        }
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _writer = new NetDataWriter();
        _netManager = new NetManager(this)
        {
            DisconnectTimeout = 10000
        };
        _netManager.Start();
    }

    private void Update()
    {
        _netManager.PollEvents();
    }

    public void Connect()
    {
        _netManager.Connect("LocalHost", 8888, "");
    }

    public void SendServer(string data)
    { 
        var bytes = Encoding.UTF8.GetBytes(data);
        _server.Send(bytes, DeliveryMethod.ReliableOrdered);
    }

    // 서버로부터 데이터를 받은 경우 호출하는 콜백 함수
    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        var data = Encoding.UTF8.GetString(reader.RawData).Replace("\0", "");
        Debug.Log($"Received Data From Server : {data}");
    }

    // 클라이언트가 서버에 성공적으로 접속했을 경우 호출하는 콜백 함수
    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log($"Connect To Port : {peer.Port}");
        Debug.Log($"Connect To IP Address : {peer.Address}");
        _server = peer;
    }

    // 클라이언트가 서버에서 성공적으로 접속해제했을 경우 호출하는 콜백 함수
    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log($"Disconnect From Port : {peer.Port}");
        Debug.Log($"Disconnect From IP Address : {peer.Address}");
        _server = null;
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {

    }

    public void OnConnectionRequest(ConnectionRequest request)
    {

    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {

    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {

    }
}

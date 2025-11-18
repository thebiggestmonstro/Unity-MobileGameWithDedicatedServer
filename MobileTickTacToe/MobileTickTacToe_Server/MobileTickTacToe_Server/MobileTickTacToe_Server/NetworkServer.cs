using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.Handlers;
using MobileTickTacToe_Server.NetworkShared.Registries;
using NetworkShared;
using System.Net;
using System.Net.Sockets;

namespace TickTackToeWithDedicated_Server
{
    public class NetworkServer : INetEventListener
    {
        NetManager _netManager;
        private Dictionary<int, NetPeer> _connections = new Dictionary<int, NetPeer>();
        private readonly ILogger<NetworkServer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public NetworkServer(ILogger<NetworkServer> logger, IServiceProvider provider) 
        {
            _logger = logger;
            _serviceProvider = provider;
        }

        public void Start()
        {
            _netManager = new NetManager(this)
            {
                DisconnectTimeout = 10000
            };

            _netManager.Start(8888);

            Console.WriteLine("Server Listening on Port 8888...");
        }

        public void PollEvents()
        {
            _netManager.PollEvents();
        }

        // 클라이언트가 서버에 접속을 요청하는 경우 호출하는 콜백 함수
        public void OnConnectionRequest(ConnectionRequest request)
        {
            Console.WriteLine($"Connection Request from {request.RemoteEndPoint}");
            request.Accept();
        }

        // 클라이언트로부터 데이터를 받은 경우 호출하는 콜백 함수
        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var packetType = (PacketType)reader.GetByte();
                    var packet = RessolvePacket(packetType, reader);
                    var handler = RessolveHandler(packetType);

                    handler.Handle(packet, peer.Id);

                    reader.Recycle();
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error {e} Happend while ressolving packet");
                }
            }
        }

        // 서버에 클라이언트가 성공적으로 접속되면 호출하는 콜백 함수
        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine($"Connected Client Port : {peer.Port}");
            Console.WriteLine($"Connected Client Address : {peer.Address}");
            Console.WriteLine($"Connected Client ID : {peer.Id}");
            _connections.Add(peer.Id, peer);
        }

        // 서버에 클라이언트가 성공적으로 접속해제되면 호출하는 콜백 함수
        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"DisConnected Client Port : {peer.Port}");
            Console.WriteLine($"DisConnected Client Address : {peer.Address}");
            Console.WriteLine($"DisConnected Client ID : {peer.Id}");
            _connections.Remove(peer.Id);
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {

        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        public IPacketHandler RessolveHandler(PacketType packetType)
        {
            var registry = _serviceProvider.GetRequiredService<HandlerRegistry>();
            var type = registry.Handlers[packetType];
            return (IPacketHandler)_serviceProvider.GetRequiredService(type);
        }

        private INetPacket RessolvePacket(PacketType packetType, NetPacketReader reader)
        {
            var registry = _serviceProvider.GetRequiredService<PacketRegistry>();
            var type = registry.PacketTypes[packetType];
            var packet = (INetPacket)Activator.CreateInstance(type);
            packet.Deserialize(reader);
            return packet;
        }
    }
}

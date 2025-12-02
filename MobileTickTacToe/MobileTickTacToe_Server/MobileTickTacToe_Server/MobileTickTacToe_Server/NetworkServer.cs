using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.Game;
using MobileTickTacToe_Server.NetworkShared.Registries;
using NetworkShared;
using System.Net;
using System.Net.Sockets;

namespace TickTackToeWithDedicated_Server
{
    public class NetworkServer : INetEventListener
    {
        NetManager _netManager;
        private readonly ILogger<NetworkServer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private UsersManager _usersManager;
        private readonly NetDataWriter _cachedWriter = new NetDataWriter();

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
            _usersManager = _serviceProvider.GetRequiredService<UsersManager>();

            Console.WriteLine("Server Listening on Port 8888...");
        }

        public void PollEvents()
        {
            _netManager.PollEvents();
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            Console.WriteLine($"Connection Request from {request.RemoteEndPoint}");
            request.Accept();
        }

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

        public void OnPeerConnected(NetPeer peer)
        {
            _usersManager.AddConnection(peer);

            var connection = _usersManager.GetConnection(peer.Id);
            _logger.LogInformation($"{connection?.User?.Id} has been connected into : {peer.Port}");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            var connection = _usersManager.GetConnection(peer.Id);
            _logger.LogInformation($"{connection?.User?.Id} has been disconnected from : {peer.Port}");

            _netManager.DisconnectPeer(peer);
            _usersManager.Disconnect(peer.Id);
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

        public void SendClient(int peerId, INetPacket packet, DeliveryMethod method = DeliveryMethod.ReliableOrdered)
        {
            var peer = _usersManager.GetConnection(peerId).Peer;
            peer.Send(WriteSerializable(packet), method);
        }

        private NetDataWriter WriteSerializable(INetPacket packet)
        {
            _cachedWriter.Reset();
            packet.Serialize(_cachedWriter);
            return _cachedWriter;
        }
    }
}

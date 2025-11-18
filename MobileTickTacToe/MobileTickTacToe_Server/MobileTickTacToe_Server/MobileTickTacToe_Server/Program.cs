using TickTackToeWithDedicated_Server;
using MobileTickTacToe_Server.InfraStructure;
using Microsoft.Extensions.DependencyInjection;

//var server = new NetworkServer();
//server.Start();

var serviceProvider = Container.Configure();
var _server = serviceProvider.GetRequiredService<NetworkServer>();
_server.Start();

while (true)
{
    _server.PollEvents();
    Thread.Sleep(15);
}
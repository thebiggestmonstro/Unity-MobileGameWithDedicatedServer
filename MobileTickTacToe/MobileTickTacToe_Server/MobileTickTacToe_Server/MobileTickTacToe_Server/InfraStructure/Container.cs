using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.Data;
using MobileTickTacToe_Server.Extensions;
using MobileTickTacToe_Server.Game;
using MobileTickTacToe_Server.MatchMaking;
using NetworkShared.Registries;
using TickTackToeWithDedicated_Server;

namespace MobileTickTacToe_Server.InfraStructure
{
    public class Container
    {
        public static IServiceProvider Configure()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(e => e.AddSimpleConsole());
            services.AddSingleton<NetworkServer>();
            services.AddSingleton<PacketRegistry>();
            services.AddSingleton<HandlerRegistry>();
            services.AddSingleton<UsersManager>();
            services.AddSingleton<MatchMaker>();
            services.AddSingleton<GameManager>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddPacketHandlers();
        }
    }
}

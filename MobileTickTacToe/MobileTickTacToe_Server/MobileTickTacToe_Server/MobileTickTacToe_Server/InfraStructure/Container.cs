using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MobileTickTacToe_Server.NetworkShared.Registries;
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
        }
    }
}

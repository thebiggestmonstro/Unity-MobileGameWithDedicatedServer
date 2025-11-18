using Microsoft.Extensions.DependencyInjection;
using MobileTickTacToe_Server.Handlers;
using NetworkShared.Attributes;
using System.Reflection;

namespace MobileTickTacToe_Server.Extensions
{
    public static class ExServiceCollections
    {
        public static IServiceCollection AddPacketHandlers(this IServiceCollection services)
        {
            var handlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && !x.IsGenericTypeDefinition)
                .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
                .Select(t => (Type: t, attr: t.GetCustomAttribute<HandlerRegisterAttribute>()))
                .Where(x => x.attr != null);

            foreach (var (type, attr) in handlers)
            {
                services.AddScoped(type);
            }

            return services;
        }
    }
}

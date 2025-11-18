using MobileTickTacToe_Server.Handlers;
using NetworkShared;
using NetworkShared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.NetworkShared.Registries
{
    public class HandlerRegistry
    {
        private Dictionary<PacketType, Type> _handlers = new Dictionary<PacketType, Type>();

        public Dictionary<PacketType, Type> Handlers
        {
            get 
            {
                if (_handlers.Count == 0)
                {
                    Initialize();
                }

                return _handlers;
            }
        }

        private void Initialize()
        {
            var handlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && !x.IsGenericTypeDefinition)
                .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
                .Select(t => (Type: t, attr: t.GetCustomAttribute<HandlerRegisterAttribute>()))
                .Where(x => x.attr != null);

            foreach (var (type, attr) in handlers)
            {
                if (!_handlers.ContainsKey(attr.PacketType))
                {
                    _handlers[attr.PacketType] = type;
                }
                else
                {
                    throw new Exception($"Multiple Handlers in {attr.PacketType}");
                }
            }
        }
    }
}

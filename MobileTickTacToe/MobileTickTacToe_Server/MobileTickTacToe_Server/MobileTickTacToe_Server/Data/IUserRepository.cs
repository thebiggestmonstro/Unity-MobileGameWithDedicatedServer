using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Data
{
    public interface IUserRepository : IRepository<User>
    {
        void SetOnline(string id);

        void SetOffline(string id);
    }
}

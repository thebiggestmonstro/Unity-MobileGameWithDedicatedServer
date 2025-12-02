using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Data
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Update(T entity);

        T Get(string id);

        IQueryable<T> GetQuery();

        ushort GetTotalCount();

        void Delete(string id);
    }
}

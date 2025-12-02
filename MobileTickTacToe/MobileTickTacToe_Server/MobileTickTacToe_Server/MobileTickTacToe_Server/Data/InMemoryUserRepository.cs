using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTickTacToe_Server.Data
{
    // 해당 클래스를 DB로 사용
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _userEntities;

        public InMemoryUserRepository()
        {
            _userEntities = new List<User>()
            {
                new User()
                { 
                    Id = "User1",
                    Password = "111",
                    IsOnline = true,
                    Score = 30
                },
                new User()
                {
                    Id = "User2",
                    Password = "222",
                    IsOnline = true,
                    Score = 60
                },
                new User()
                {
                    Id = "User3",
                    Password = "333",
                    IsOnline = true,
                    Score = 90
                }
            };
        }

        void IRepository<User>.Add(User entity)
        {
            _userEntities.Add(entity);
        }

        void IRepository<User>.Delete(string id)
        {
            var entity = _userEntities.FirstOrDefault(e => e.Id == id);
            _userEntities.Remove(entity);
        }

        User IRepository<User>.Get(string id)
        {
            return _userEntities.FirstOrDefault(e => e.Id == id);
        }

        IQueryable<User> IRepository<User>.GetQuery()
        {
            return _userEntities.AsQueryable();
        }

        ushort IRepository<User>.GetTotalCount()
        {
            return (ushort)_userEntities.Count(e => e.IsOnline == true);
        }

        void IUserRepository.SetOffline(string id)
        {
            var entity = _userEntities.FirstOrDefault(e => e.Id == id).IsOnline = false; 
        }

        void IUserRepository.SetOnline(string id)
        {
            var entity = _userEntities.FirstOrDefault(e => e.Id == id).IsOnline = true;
        }

        void IRepository<User>.Update(User entity)
        {
            int index = _userEntities.FindIndex(e => e.Id == entity.Id);
            _userEntities[index] = entity;
        }
    }
}

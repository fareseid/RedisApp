using RedisServer.Model.RedisModel;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RedisServer.Repository
{
    public class InMemoryRepository : IRepository
    {
        private readonly ConcurrentDictionary<string, IRedisItem> ListRedisItems;
        private readonly ConcurrentDictionary<string, string> Accounts;
        private readonly ConcurrentDictionary<string, User> Users;

        public InMemoryRepository()
        {
            ListRedisItems = new ConcurrentDictionary<string, IRedisItem>();
            Accounts = new ConcurrentDictionary<string, string>();
            Users = new ConcurrentDictionary<string, User>();
            CreateDefaultUser();
        }

        #region CRUD Item
        public IRedisItem Get(string Key)
        { 
            return ListRedisItems.GetValueOrDefault(Key);  
        }

        public void Insert(string Key,IRedisItem Item)
        {
            ListRedisItems.AddOrUpdate(Key, Item, (key, oldValue) => Item);
        }
        public void Remove(string Key)
        {
            ListRedisItems.TryRemove(Key, out _);
        }
        #endregion

        #region Authentication
        public bool Authenticate(string Username, string Password)
        {
            return Accounts.ContainsKey(Username) && Accounts[Username] == Password;
        }

        public User GetUserByUsername(string Username)
        {
            return Users.ContainsKey(Username)? Users[Username] : null;
        }
        #endregion

        private void CreateDefaultUser()
        {
            Accounts.AddOrUpdate("User1", "2AC9CB7DC02B3C0083EB70898E549B63", (key, oldValue) => "");
            Users.AddOrUpdate("User1", new User
            {
                Id = 1,
                FullName = "User 1",
                Email = "user1@gmail.com"
            }, (key, oldValue) => new User());
        }
    }
}

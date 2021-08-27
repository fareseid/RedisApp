using RedisServer.Model.RedisModel;

namespace RedisServer.Repository
{
    public interface IRepository
    {
        User GetUserByUsername(string Username);
        bool Authenticate(string Username,string Password);
        IRedisItem Get(string Key);
        void Insert(string Key, IRedisItem Item);
        void Remove(string Key);
    }
}

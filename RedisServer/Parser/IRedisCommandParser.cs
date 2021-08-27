using RedisServer.Model.CommandModel;

namespace RedisServer.Parser
{
    public interface IRedisCommandParser
    {
        public RedisCommand Parse(string Command);
    }
}

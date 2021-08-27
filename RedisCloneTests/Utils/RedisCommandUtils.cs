using RedisServer.Model.CommandModel;
using static RedisServer.Model.CommandModel.CommandType;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisCloneTests.Utils
{
    public static class RedisCommandUtils
    {
        public static RedisCommand BuildRedisCommand(string Key, object Value, EnumCommandType CommandType, EnumVariableType VariableType)
        {
            return new RedisCommand(CommandType, Key, Value, VariableType);
        }

        public static RedisCommand BuildRedisCommand(string Key, EnumCommandType CommandType)
        {
            return new RedisCommand(CommandType, Key);
        }
    }
}

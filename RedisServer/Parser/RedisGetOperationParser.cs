using RedisServer.Helper;
using RedisServer.Model.CommandModel;
using System;
namespace RedisServer.Parser
{
    public static class RedisGetOperationParser 
    { 
        public static RedisCommand BuildGetCommand(string[] CommandParameters)
        {
            try
            {
                return new RedisCommand(CommandType.EnumCommandType.GET, CommandParameters[1]);
            }
            catch (Exception)
            {
                throw new AppException("Invalid GET Command");
            }
        }
    }
}
using RedisServer.Helper;
using RedisServer.Model.CommandModel;
using System;
using static RedisServer.Model.CommandModel.VariableType;


namespace RedisServer.Parser
{
    public static class RedisExpireOperationParser
    { 
        public static RedisCommand BuildCommand(string[] CommandParameters)
        {
            try
            { 
                return new RedisCommand(CommandType.EnumCommandType.EXPIRE, CommandParameters[1], Convert.ToInt32(CommandParameters[2]), EnumVariableType.INT);
            }
            catch (Exception)
            { 
                throw new AppException("Invalid EXPIRE Command");
            }
        }
    }
}

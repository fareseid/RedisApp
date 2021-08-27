using RedisServer.Helper;
using RedisServer.Model.CommandModel;
using System;
using static RedisServer.Model.CommandModel.VariableType;
using static RedisServer.Model.CommandModel.CommandType;

namespace RedisServer.Parser
{
    public static class RedisSetOperationParser
    {  
        public static RedisCommand BuildSetCommand(string[] CommandParameters)
        {
            try
            {  
                return new RedisCommand(EnumCommandType.SET, CommandParameters[1], CommandParameters[2], ToVariableType(CommandParameters[2]));
            }
            catch (Exception)
            { 
                throw new AppException("Invalid Set Command");
            }
        }
    }
}
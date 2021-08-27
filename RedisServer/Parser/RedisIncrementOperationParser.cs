using RedisServer.Helper;
using RedisServer.Model.CommandModel;
using System;
using static RedisServer.Model.CommandModel.CommandType;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisServer.Parser
{
    public static class RedisIncrementOperationParser
    {
        public static RedisCommand BuildIncrCommand(string[] CommandParameters)
        {
            return BuildCommand(CommandParameters, EnumCommandType.INCR);
        }

        public static RedisCommand BuildDecrCommand(string[] CommandParameters)
        {
            return BuildCommand(CommandParameters, EnumCommandType.DECR);
        }

        private static RedisCommand BuildCommand(string[] CommandParameters, EnumCommandType CommandType)
        {
            try
            { 
                return new RedisCommand(CommandType, CommandParameters[1], GetValueOrDefault(CommandParameters), EnumVariableType.INT);
            }
            catch (Exception)
            { 
                throw new AppException("Invalid "+ CommandType +" Command");
            }
        }

        private static int GetValueOrDefault(string[] CommandParameters)
        { 
            return CommandParameters.Length > 2 ? Convert.ToInt32(CommandParameters[2]) : 1;
        }
    }
}

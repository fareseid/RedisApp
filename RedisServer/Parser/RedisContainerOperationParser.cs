using RedisServer.Helper;
using RedisServer.Model.CommandModel;
using System;
using static RedisServer.Model.CommandModel.CommandType;
using static RedisServer.Model.CommandModel.VariableType;
namespace RedisServer.Parser
{
    public static class RedisContainerOperationParser
    {
        public static RedisCommand BuildRPushCommand(string[] CommandParameters)
        {
            return BuildPushCommand(CommandParameters, EnumCommandType.RPUSH);
        }

        public static RedisCommand BuildLPushCommand(string[] CommandParameters)
        {
            return BuildPushCommand(CommandParameters, EnumCommandType.LPUSH);
        }

        public static RedisCommand BuildRPopCommand(string[] CommandParameters)
        {
            return BuildPopCommand(CommandParameters, EnumCommandType.RPOP);
        }

        public static RedisCommand BuildLPopCommand(string[] CommandParameters)
        {
            return BuildPopCommand(CommandParameters, EnumCommandType.LPOP);
        }

        public static RedisCommand BuildLIndexCommand(string[] CommandParameters)
        {
            try
            {  
                int CommandValue = Convert.ToInt32(CommandParameters[2]);
                return new RedisCommand(EnumCommandType.LINDEX, CommandParameters[1], CommandValue, EnumVariableType.INT);
            }
            catch (Exception)
            {
                throw new AppException("Invalid LINDEX Command");
            }
        }

        private static RedisCommand BuildPushCommand(string[] CommandParameters, EnumCommandType CommandType)
        {
            try
            {
                EnumVariableType VariableType = ToVariableType(CommandParameters[2]);
                return new RedisCommand(CommandType, CommandParameters[1], CommandParameters[2], VariableType);
            }
            catch (Exception)
            {
                throw new AppException("Invalid " + CommandType + " Command");
            }
        }

        private static RedisCommand BuildPopCommand(string[] CommandParameters, EnumCommandType CommandType)
        {
            try
            {
                return new RedisCommand(CommandType, CommandParameters[1]);
            }
            catch (Exception)
            {
                throw new AppException("Invalid " + CommandType + " Command");
            }
        }
    }
}

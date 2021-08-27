using RedisServer.Helper;
using RedisServer.Model.CommandModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedisServer.Parser
{
    public class RedisCommandParser : IRedisCommandParser
    {
        private readonly Dictionary<string, Func<string[], RedisCommand>> ParserRegistry;

        public RedisCommandParser() {
            ParserRegistry = new Dictionary<string, Func<string[], RedisCommand>>();
            ParserRegistry.Add("SET", c => RedisSetOperationParser.BuildSetCommand(c));
            ParserRegistry.Add("GET", c => RedisGetOperationParser.BuildGetCommand(c));
            ParserRegistry.Add("INCR", c => RedisIncrementOperationParser.BuildIncrCommand(c));
            ParserRegistry.Add("DECR", c => RedisIncrementOperationParser.BuildDecrCommand(c));
            ParserRegistry.Add("RPUSH", c => RedisContainerOperationParser.BuildRPushCommand(c));
            ParserRegistry.Add("RPOP", c => RedisContainerOperationParser.BuildRPopCommand(c));
            ParserRegistry.Add("LPUSH", c => RedisContainerOperationParser.BuildLPushCommand(c));
            ParserRegistry.Add("LPOP", c => RedisContainerOperationParser.BuildLPopCommand(c));
            ParserRegistry.Add("LINDEX", c => RedisContainerOperationParser.BuildLIndexCommand(c));
            ParserRegistry.Add("EXPIRE", c => RedisExpireOperationParser.BuildCommand(c));
        }

        public RedisCommand Parse(string Command)
        {  
            string[] CommandParameters = RetrieveCommandParameters(Command);
            Func<string[], RedisCommand>  Parser = GetParserBuilderFunction(CommandParameters);
            ThrowExceptionIfNull(Parser);
            return Parser.Invoke(CommandParameters); 
        }

        private Func<string[], RedisCommand> GetParserBuilderFunction(string[] CommandParameters)
        {
            return CommandParameters.Count() == 0 ? null : ParserRegistry.GetValueOrDefault(CommandParameters[0].ToUpper());
        }

        private void ThrowExceptionIfNull(Func<string[], RedisCommand> Parser)
        {
            if (Parser == null)
            {
                throw new AppException("Invalid Command"); 
            }
        }

        private string[] RetrieveCommandParameters(string Command)
        {
            return Command.Trim().Split(' ').ToList().Where(o => o.Length > 0).ToArray();
        }
    }
}

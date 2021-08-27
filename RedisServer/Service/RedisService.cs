using Newtonsoft.Json;
using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Parser;
using RedisServer.RedisOperations;
using RedisServer.Repository;

namespace RedisServer.Service
{
    public interface IRedisService {
        string Launch(string Command);
    }

    public class RedisService : IRedisService
    {  
        private readonly IRepository _Repository;
        private readonly IRedisCommandParser _CommandParser;
        private readonly IRedisLogger _RedisLogger;

        public RedisService( 
            IRepository Repository,
            IRedisCommandParser CommandParser,
            IRedisLogger RedisLogger) {  
            _Repository = Repository;
            _CommandParser = CommandParser;
            _RedisLogger = RedisLogger;
        }

        public string Launch(string Command)
        {

            _RedisLogger.Trace("[Start] Parsing Command");
            RedisCommand RedisCommand = _CommandParser.Parse(Command);
            _RedisLogger.Trace("[End] Parsing Command : {0}", JsonConvert.SerializeObject(RedisCommand));
             
            RedisOperationLauncher Caller = new RedisOperationLauncher(_Repository,_RedisLogger);
            return Caller.Launch(RedisCommand); 
        } 
    }
}
using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Model.RedisModel;
using RedisServer.Model.RedisOperationModel;
using RedisServer.Repository;
using System;
using System.Threading.Tasks;

namespace RedisServer.RedisOperations
{
    public interface IRedisExpireOperation {
        IRedisItem Expire(RedisCommand RedisCommand);
    }

    public class RedisExpireOperation : AbstractRedisOperation, IRedisExpireOperation
    {  
        public RedisExpireOperation(IRepository Repository, IRedisLogger RedisLogger) : base(Repository, RedisLogger)
        { }

        public IRedisItem Expire(RedisCommand RedisCommand)
        { 
            IRedisItem Item = _Repository.Get(RedisCommand.Key);
            if (Item == null) { return null; }
            ThrowExceptionIfOperationNotSupported(Item, typeof(IRedisExpirableItem));

            int TimeInSeconds = (int)RedisCommand.Value *1000; 
            Task.Delay(TimeInSeconds).ContinueWith(o => { _Repository.Remove(RedisCommand.Key); });

            return Item; 
        }
    }
}

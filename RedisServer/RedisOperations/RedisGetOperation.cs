using Newtonsoft.Json;
using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Model.RedisModel;
using RedisServer.Model.RedisOperationModel;
using RedisServer.Repository;

namespace RedisServer.RedisOperations
{
    public interface IRedisGetOperation {
        object Get(RedisCommand RedisCommand);
    }

    public class RedisGetOperation: AbstractRedisOperation, IRedisGetOperation
    { 
        public RedisGetOperation(IRepository Repository, IRedisLogger RedisLogger) : base(Repository, RedisLogger)
        { }

        public object Get(RedisCommand RedisCommand) {  
            IRedisItem Item = _Repository.Get(RedisCommand.Key); 
            if (Item == null) { return null; }
            ThrowExceptionIfOperationNotSupported(Item, typeof(IRedisGettableItem));

            return ((IRedisGettableItem)Item).GetValue(); 
        }   
    }
}

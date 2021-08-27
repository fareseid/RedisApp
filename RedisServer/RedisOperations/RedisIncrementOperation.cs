using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Model.RedisModel;
using RedisServer.Model.RedisOperationModel;
using RedisServer.Repository;
using System;

namespace RedisServer.RedisOperations
{
    public interface IRedisIncrementOperation {
        IRedisItem Increment(RedisCommand RedisCommand);
        IRedisItem Decrement(RedisCommand RedisCommand);
    }

    public class RedisIncrementOperation:AbstractRedisOperation, IRedisIncrementOperation
    { 
        public RedisIncrementOperation(IRepository Repository, IRedisLogger RedisLogger) : base(Repository, RedisLogger)
        { }

        public IRedisItem Increment(RedisCommand RedisCommand)
        { 
            IRedisItem RedisItem = GetOrCreateItemIfNull(RedisCommand.Key, RedisCommand.VariableType); 
            ThrowExceptionIfOperationNotSupported(RedisItem, typeof(IRedisIncrementableItem));

            ((IRedisIncrementableItem)RedisItem).Increment((int)RedisCommand.Value); 
            _Repository.Insert(RedisCommand.Key, RedisItem);

            return RedisItem; 
        }

        public IRedisItem Decrement(RedisCommand RedisCommand)
        {
            IRedisItem RedisItem = GetOrCreateItemIfNull(RedisCommand.Key, RedisCommand.VariableType);
            ThrowExceptionIfOperationNotSupported(RedisItem, typeof(IRedisIncrementableItem));

            ((IRedisIncrementableItem)RedisItem).Decrement((int)RedisCommand.Value); 
            _Repository.Insert(RedisCommand.Key, RedisItem);

            return RedisItem; 
        }
    }
}

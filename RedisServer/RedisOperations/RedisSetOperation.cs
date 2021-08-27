using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Model.RedisModel;
using RedisServer.Model.RedisOperationModel;
using RedisServer.Repository;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisServer.RedisOperations
{
    public interface IRedisSetOperation {
        IRedisItem Set(RedisCommand RedisCommand);
    }

    public class RedisSetOperation : AbstractRedisOperation, IRedisSetOperation
    { 
        public RedisSetOperation(IRepository Repository, IRedisLogger RedisLogger) : base(Repository, RedisLogger)
        { }

        public IRedisItem Set(RedisCommand RedisCommand)
        { 
            IRedisItem RedisItem = CreateItem(RedisCommand.VariableType);
            ThrowExceptionIfOperationNotSupported(RedisItem, typeof(IRedisSettableItem));
             
            ((IRedisSettableItem)RedisItem).SetValue(RedisCommand.Value); 
            _Repository.Insert(RedisCommand.Key, RedisItem);

            return RedisItem; 
        }  
    }
}
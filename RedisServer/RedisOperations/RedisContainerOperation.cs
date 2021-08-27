using RedisServer.Logging;
using RedisServer.Model.RedisModel;
using RedisServer.Model.RedisOperationModel;
using RedisServer.Model.CommandModel;
using RedisServer.Repository;
using System;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisServer.RedisOperations
{
    public interface IRedisContainerOperation
    {
        IRedisItem PushRight(RedisCommand RedisCommand);
        IRedisItem PushLeft(RedisCommand RedisCommand);
        IRedisItem PopRight(RedisCommand RedisCommand);
        IRedisItem PopLeft(RedisCommand RedisCommand);
        IRedisItem LIndex(RedisCommand RedisCommand);
    }

    public class RedisContainerOperation : AbstractRedisOperation, IRedisContainerOperation
    { 
        public RedisContainerOperation(IRepository Repository, IRedisLogger RedisLogger) : base(Repository, RedisLogger)
        { }
         
        public IRedisItem PushRight(RedisCommand RedisCommand)
        { 
            IRedisItem ListItem = GetOrCreateItemIfNull(RedisCommand.Key, EnumVariableType.LIST);
            ThrowExceptionIfOperationNotSupported(ListItem, typeof(IRedisContainerItem));

            IRedisItem redisItem = CreateEnumerableItem(RedisCommand); 
            ((IRedisContainerItem)ListItem).PushRight(redisItem);

            _Repository.Insert(RedisCommand.Key, ListItem);

            return ListItem;  
        }

        public IRedisItem PushLeft(RedisCommand RedisCommand)
        { 
            IRedisItem ListItem = GetOrCreateItemIfNull(RedisCommand.Key, EnumVariableType.LIST);
            ThrowExceptionIfOperationNotSupported(ListItem, typeof(IRedisContainerItem));

            IRedisItem redisItem = CreateEnumerableItem(RedisCommand); 
            ((IRedisContainerItem)ListItem).PushLeft(redisItem);

            _Repository.Insert(RedisCommand.Key, ListItem);

            return ListItem; 
        } 

        public IRedisItem PopRight(RedisCommand RedisCommand)
        { 
            IRedisItem Item = _Repository.Get(RedisCommand.Key);
            if (Item == null) { return null; }
            ThrowExceptionIfOperationNotSupported(Item, typeof(IRedisContainerItem));

            IRedisItem PoppedItem = ((IRedisContainerItem)Item).PopRight(); 
            _Repository.Insert(RedisCommand.Key, Item);

            return PoppedItem;  
        }

        public IRedisItem PopLeft(RedisCommand RedisCommand)
        {  
            IRedisItem Item = _Repository.Get(RedisCommand.Key);
            if (Item == null) { return null; }
            ThrowExceptionIfOperationNotSupported(Item, typeof(IRedisContainerItem));

            IRedisItem PoppedItem = ((IRedisContainerItem)Item).PopLeft(); 
            _Repository.Insert(RedisCommand.Key, Item);

            return PoppedItem; 
        }

        public IRedisItem LIndex(RedisCommand RedisCommand)
        { 
            IRedisItem Item = _Repository.Get(RedisCommand.Key);
            if (Item == null) { return null; }
            ThrowExceptionIfOperationNotSupported(Item, typeof(IRedisContainerItem));

            return ((IRedisContainerItem)Item).LIndex((int)RedisCommand.Value); 
        }

        private IRedisItem CreateEnumerableItem(RedisCommand RedisCommand)
        {
            IRedisItem EnumerableRedisItem = CreateItem(RedisCommand.VariableType);
            ThrowExceptionIfOperationNotSupported(EnumerableRedisItem, typeof(IRedisEnumerableItem));
            ((IRedisEnumerableItem)EnumerableRedisItem).SetEnumerableValue(RedisCommand.Value);
            return EnumerableRedisItem;
        }
    }
}
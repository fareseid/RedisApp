using Newtonsoft.Json;
using RedisServer.Helper;
using RedisServer.Logging;
using RedisServer.Model.RedisModel;
using RedisServer.Repository;
using System;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisServer.RedisOperations
{
    public abstract class AbstractRedisOperation
    {
        protected readonly IRepository _Repository;
        protected readonly IRedisLogger _RedisLogger;

        protected AbstractRedisOperation(IRepository Repository,
            IRedisLogger RedisLogger)
        {
            _Repository = Repository;
            _RedisLogger = RedisLogger;
        }

        protected IRedisItem GetOrCreateItemIfNull(string Key, EnumVariableType VariableType)
        { 
            IRedisItem Item = _Repository.Get(Key); 
            return Item == null ? CreateItem(VariableType) : Item;
        }

        protected static void ThrowExceptionIfOperationNotSupported(IRedisItem Item, Type Type)
        { 
            if (!Type.IsAssignableFrom(Item.GetType()))
            {
                throw new AppException("Method Not Allowed");
            } 
        }
    }
}

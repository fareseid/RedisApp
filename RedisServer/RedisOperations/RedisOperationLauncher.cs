using RedisServer.Helper;
using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Model.RedisModel;
using RedisServer.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using static RedisServer.Model.CommandModel.CommandType;

namespace RedisServer.RedisOperations
{
    public class RedisOperationLauncher
    { 
        private readonly IRedisGetOperation GetOperation;
        private readonly IRedisContainerOperation ContainerOperation;
        private readonly IRedisExpireOperation ExpireOperation;
        private readonly IRedisIncrementOperation IncrementOperation;
        private readonly IRedisSetOperation SetOperation;
        private readonly IRedisLogger _RedisLogger;
        private readonly Dictionary<EnumCommandType, Func<RedisCommand, string>> OperationRegistry;

        public RedisOperationLauncher(IRepository Repository,
            IRedisLogger RedisLogger) :this(RedisLogger, 
                new RedisGetOperation(Repository,RedisLogger),
                new RedisContainerOperation(Repository, RedisLogger),
                new RedisExpireOperation(Repository, RedisLogger),
                new RedisIncrementOperation(Repository, RedisLogger),
                new RedisSetOperation(Repository, RedisLogger))
        { }

        public RedisOperationLauncher(IRedisLogger RedisLogger,
            IRedisGetOperation _GetOperation,
            IRedisContainerOperation _ContainerOperation,
            IRedisExpireOperation _ExpireOperation,
            IRedisIncrementOperation _IncrOperation,
            IRedisSetOperation _SetOperation)
        { 
            _RedisLogger = RedisLogger; 

            GetOperation = _GetOperation;
            ContainerOperation = _ContainerOperation;
            ExpireOperation = _ExpireOperation;
            IncrementOperation = _IncrOperation;
            SetOperation = _SetOperation;

            OperationRegistry = new Dictionary<EnumCommandType, Func<RedisCommand, string>>(); 
            OperationRegistry.Add(EnumCommandType.GET, c => LaunchGetOperation(c));
            OperationRegistry.Add(EnumCommandType.SET, c => LaunchSetOperation(c));
            OperationRegistry.Add(EnumCommandType.INCR, c => LaunchIncrOperation(c));
            OperationRegistry.Add(EnumCommandType.DECR, c => LaunchDecrOperation(c));
            OperationRegistry.Add(EnumCommandType.RPUSH, c => LaunchRPushOperation(c));
            OperationRegistry.Add(EnumCommandType.RPOP, c => LaunchRPopOperation(c));
            OperationRegistry.Add(EnumCommandType.LPUSH, c => LaunchLPushOperation(c));
            OperationRegistry.Add(EnumCommandType.LPOP, c => LaunchLPopOperation(c));
            OperationRegistry.Add(EnumCommandType.LINDEX, c => LaunchLIndexOperation(c));
            OperationRegistry.Add(EnumCommandType.EXPIRE, c => LaunchExpireOperation(c));
        }

        public string Launch(RedisCommand RedisCommand)
        {
            Mutex Mutex = new Mutex(false, RedisCommand.Key);
            WaitIfBlockingCommand(Mutex, RedisCommand);
             
            try
            {
                _RedisLogger.Trace("[Start] Launching {0} Command", RedisCommand.Key);  
                return LaunchOperation(RedisCommand);
            }
            finally
            {
                ReleaseIfBlockingCommand(Mutex, RedisCommand);
                _RedisLogger.Trace("[End] Launching {0} Command", RedisCommand.Key);
            }
        }

        private string LaunchOperation(RedisCommand RedisCommand)
        {
            Func<RedisCommand, string> Operation = OperationRegistry.GetValueOrDefault(RedisCommand.CommandType);
            ThrowExceptionIfNull(Operation);
            return Operation.Invoke(RedisCommand);
        }

        private string LaunchGetOperation(RedisCommand RedisCommand)
        {
            object Value = GetOperation.Get(RedisCommand);
            return Value == null ? "(nil)" : Value.ToString();
        }

        private string LaunchSetOperation(RedisCommand RedisCommand)
        {
            SetOperation.Set(RedisCommand);
            return "OK";
        }

        private string LaunchDecrOperation(RedisCommand RedisCommand)
        {
            return IncrementOperation.Decrement(RedisCommand).ToString();
        }

        private string LaunchIncrOperation(RedisCommand RedisCommand)
        {
            return IncrementOperation.Increment(RedisCommand).ToString();
        }

        private string LaunchExpireOperation(RedisCommand RedisCommand)
        {
            return ExpireOperation.Expire(RedisCommand) == null ? "0" : "1";
        }

        private string LaunchLIndexOperation(RedisCommand RedisCommand)
        { 
            IRedisItem LIndexItem = ContainerOperation.LIndex(RedisCommand);
            return LIndexItem == null ? "(nil)" : LIndexItem.ToString();
        }

        private string LaunchLPopOperation(RedisCommand RedisCommand)
        {
            IRedisItem LPopItem = ContainerOperation.PopLeft(RedisCommand);
            return LPopItem == null ? "(nil)" : LPopItem.ToString();
        }

        private string LaunchLPushOperation(RedisCommand RedisCommand)
        {
            return ContainerOperation.PushLeft(RedisCommand).ToString();
        }

        private string LaunchRPopOperation(RedisCommand RedisCommand)
        {
            IRedisItem RPopItem = ContainerOperation.PopRight(RedisCommand);
            return RPopItem == null ? "(nil)" : RPopItem.ToString();
        }

        private string LaunchRPushOperation(RedisCommand RedisCommand)
        {
            return ContainerOperation.PushRight(RedisCommand).ToString();
        } 

        private void WaitIfBlockingCommand(Mutex mutex, RedisCommand RedisCommand)
        {
            if (RedisCommand.IsBlockingCommand())
            {
                mutex.WaitOne();
            }
        }
        private void ReleaseIfBlockingCommand(Mutex mutex, RedisCommand RedisCommand)
        {
            if (RedisCommand.IsBlockingCommand())
            {
                mutex.ReleaseMutex();
            }
        }

        private static void ThrowExceptionIfNull(Func<RedisCommand, string> Operation)
        {
            if (Operation == null)
            {
                throw new AppException("Invalid Command");

            }
        }
    }
}
using System;
using System.Collections.Generic;
using static RedisServer.Model.CommandModel.CommandType;
using static RedisServer.Model.CommandModel.VariableType;
namespace RedisServer.Model.CommandModel
{
    public class RedisCommand
    {
        private readonly string _Key;
        private readonly object _Value;
        private readonly EnumCommandType _CommandType;
        private readonly EnumVariableType _VariableType;

        public RedisCommand(EnumCommandType CommandType, string Key, object Value=null, EnumVariableType VariableType = EnumVariableType.NA)
        {
            _CommandType = CommandType;
            _Key = Key;
            _Value = Value;
            _VariableType = VariableType;
        }

        public string Key {
            get {
                return _Key;
            }
        }

        public object Value
        {
            get
            {
                return _Value;
            }
        }

        public EnumCommandType CommandType
        {
            get
            {
                return _CommandType;
            }
        }

        public EnumVariableType VariableType
        {
            get
            {
                return _VariableType;
            }
        }

        public bool IsBlockingCommand()
        {
            return CommandModel.CommandType.IsBlockingCommand(_CommandType);
        } 
        public override bool Equals(object obj)
        {
            return obj is RedisCommand command &&
                   _Key == command._Key &&
                   EqualityComparer<object>.Default.Equals(_Value, command._Value) &&
                   _CommandType == command._CommandType &&
                   _VariableType == command._VariableType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_Key, _Value, _CommandType, _VariableType);
        }
    }
}

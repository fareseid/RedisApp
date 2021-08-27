using RedisServer.Model.RedisOperationModel;
using System;

namespace RedisServer.Model.RedisModel
{
    public class RedisStringItem : IRedisItem, IRedisGettableItem, IRedisSettableItem, IRedisExpirableItem, IRedisEnumerableItem
    {
        private string _Value;

        public object GetValue()
        {
            return _Value;
        }
         
        public void SetValue(object Value)
        {
            _Value = Value.ToString();
        }

        public void SetEnumerableValue(object Value)
        {
            SetValue(Value);
        }

        public override string ToString()
        {
            return _Value;
        }

        public override bool Equals(object obj)
        {
            return obj is RedisStringItem item &&
                   _Value == item._Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_Value);
        }
    }
}

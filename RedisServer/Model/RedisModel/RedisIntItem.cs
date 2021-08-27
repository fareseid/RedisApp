using RedisServer.Model.RedisOperationModel;
using System;

namespace RedisServer.Model.RedisModel
{
    public class RedisIntItem : IRedisItem, IRedisGettableItem, IRedisSettableItem, IRedisIncrementableItem, IRedisExpirableItem, IRedisEnumerableItem
    {
        private int _Value; 

        public object GetValue()
        {
            return _Value;
        } 

        public void SetValue(object Value)
        {
            _Value = Convert.ToInt32(Value);
        }

        public void SetEnumerableValue(object Value)
        {
            SetValue(Value);
        }

        public void Increment(int Count=1) {
            ThrowExceptionIfMaxValueReached(_Value, Count);
            _Value += Count;
        }

        public void Decrement(int Count=1)
        {
            ThrowExceptionIfMinValueReached(_Value, Count);
            _Value -= Count;
        }

        public override string ToString()
        {
            return "(integer) " + _Value;
        }
        

        public override bool Equals(object obj)
        {
            return obj is RedisIntItem item &&
                   _Value == item._Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_Value);
        }

        private static void ThrowExceptionIfMaxValueReached(int Value, int Count)
        {
            if (Value > int.MaxValue - Count)
            {
                throw new InvalidOperationException("Maximum capacity is reached");
            }
        }

        private static void ThrowExceptionIfMinValueReached(int Value, int Count)
        {
            if (Value < int.MinValue + Count)
            {
                throw new InvalidOperationException("Minimum capacity is reached");
            }
        }
    }
}

using RedisServer.Model.RedisOperationModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedisServer.Model.RedisModel
{
    public class RedisListItem : IRedisItem, IRedisContainerItem, IRedisExpirableItem
    {
        private List<IRedisItem> _Value;

        public RedisListItem() {
            _Value = new List<IRedisItem>();
        } 

        public void PushRight(IRedisItem Value)
        {
            _Value.Add(Value);
        }

        public IRedisItem PopRight()
        {  
            var PoppedItem = _Value.LastOrDefault();
            if (PoppedItem != null)
            {
                _Value.RemoveAt(_Value.Count-1);
            }
            return PoppedItem;
        } 

        public void PushLeft(IRedisItem Value)
        {
            List<IRedisItem> TempList = new List<IRedisItem> { Value };
            TempList.AddRange(_Value);
            _Value = TempList;
        }

        public IRedisItem PopLeft()
        {
            var PoppedItem = _Value.FirstOrDefault();
            if (PoppedItem != null) {
                _Value.RemoveAt(0);
            } 
            return PoppedItem;
        }

        public IRedisItem LIndex(int Value)
        {
            if (_Value.Count  < Value + 1)
            {
                return null;
            }
            else {
                return _Value[Value];
            }
        }
         
        public override string ToString()
        {
            return "(integer) " + _Value.Count.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is RedisListItem item &&
                        _Value.SequenceEqual(item._Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_Value);
        }
    }
}

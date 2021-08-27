namespace RedisServer.Model.RedisOperationModel
{
    public interface IRedisIncrementableItem
    {
        void Increment(int Count = 1);
        void Decrement(int Count = 1);
    }
}
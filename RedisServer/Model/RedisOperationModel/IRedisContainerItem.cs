using RedisServer.Model.RedisModel;

namespace RedisServer.Model.RedisOperationModel
{
    public interface IRedisContainerItem
    {
        IRedisItem PopRight();
        void PushRight(IRedisItem Item);
        IRedisItem PopLeft();
        void PushLeft(IRedisItem Item);
        IRedisItem LIndex(int Index);
    }
}

using RedisClient.Models.AppModels;

namespace RedisClient.RedisIO.Retriever.RedisUserRetriever
{
    public interface IRedisUserRetriever
    {
        RedisUser RetrieveUser();
    }
}

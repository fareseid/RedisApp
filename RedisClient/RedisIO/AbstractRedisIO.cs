using RedisClient.Models.AppModels;
using RedisClient.RedisIO.Printer;
using RedisClient.RedisIO.Retriever.RedisCommandRetriever;
using RedisClient.RedisIO.Retriever.RedisUserRetriever;

namespace RedisClient.RedisIO
{
    public abstract class AbstractRedisIO
    {
        private readonly IRedisUserRetriever _RedisUserRetriever;
        private readonly IRedisCommandRetriever _RedisCommandRetriever;
        private readonly IRedisResponsePrinter _RedisResponsePrinter;

        public AbstractRedisIO(IRedisUserRetriever RedisUserRetriever,
            IRedisCommandRetriever RedisCommandRetriever,
            IRedisResponsePrinter RedisResponsePrinter)
        {
            _RedisUserRetriever = RedisUserRetriever;
            _RedisCommandRetriever = RedisCommandRetriever;
            _RedisResponsePrinter = RedisResponsePrinter;
        }

        public RedisUser RetrieveUser()
        {
            return _RedisUserRetriever.RetrieveUser();
        }

        public string RetrieveCommand()
        {
            return _RedisCommandRetriever.RetrieveCommand();
        }

        public void Print(string Response)
        {
            _RedisResponsePrinter.Print(Response);
        }
    }
}

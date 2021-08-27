using RedisClient.RedisIO.Printer;
using RedisClient.RedisIO.Retriever.RedisCommandRetriever;
using RedisClient.RedisIO.Retriever.RedisUserRetriever;

namespace RedisClient.RedisIO
{
    public class RedisConsoleIO : AbstractRedisIO
    {
        public RedisConsoleIO() : base(new RedisUserConsoleRetriever(), new RedisCommandConsoleRetriever(), new RedisResponseConsolePrinter()) { }
    }
}

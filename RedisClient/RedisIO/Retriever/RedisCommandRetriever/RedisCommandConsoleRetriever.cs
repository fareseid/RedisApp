using System;

namespace RedisClient.RedisIO.Retriever.RedisCommandRetriever
{
    public class RedisCommandConsoleRetriever : IRedisCommandRetriever
    {
        public string RetrieveCommand()
        {
            Console.WriteLine("Write Command");
            return Console.ReadLine();
        }
    }
}

using System;

namespace RedisClient.RedisIO.Printer
{
    public class RedisResponseConsolePrinter : IRedisResponsePrinter
    {
        public void Print(string Response)
        {
            Console.WriteLine(Response);
        }
    }
}

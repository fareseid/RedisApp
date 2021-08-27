using System;

namespace RedisServer.Helper
{ 
    public class AppException : Exception
    {  
        public AppException(string Message) : base(Message) { } 
    }
}

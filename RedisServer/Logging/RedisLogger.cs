using NLog; 

namespace RedisServer.Logging
{
    public interface IRedisLogger {
        void Info(string Message,params string[] MessageArgs);
        void Debug(string Message, params string[] MessageArgs);
        void Error(string Message, params string[] MessageArgs);
        void Trace(string Message, params string[] MessageArgs);
        void Warn(string Message, params string[] MessageArgs);
    }
    public class RedisLogger: IRedisLogger
    {
        static Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

        public void Debug(string Message, string[] MessageArgs)
        {
            Logger.Debug(Message, MessageArgs);
        }

        public void Error(string Message, string[] MessageArgs)
        {
            Logger.Error(Message, MessageArgs);
        }

        public void Info(string Message, string[] MessageArgs)
        {
            Logger.Info(Message, MessageArgs);
        }

        public void Trace(string Message, string[] MessageArgs)
        {
            Logger.Trace(Message, MessageArgs);
        }

        public void Warn(string Message, string[] MessageArgs)
        {
            Logger.Warn(Message, MessageArgs);
        }
    }
}

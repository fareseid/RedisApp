namespace RedisClient.Models.AppModels
{
    public interface IAppSettings { }
    public class AppSettings: IAppSettings
    { 
        private readonly string _RedisServerHost; 
        public AppSettings(string RedisServerHost)
        {
            _RedisServerHost = RedisServerHost;
        } 
        public string RedisServerHost
        {
            get
            {
                return _RedisServerHost;
            }
        }
    }
}

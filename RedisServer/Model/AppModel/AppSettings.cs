namespace RedisServer.Model.AppModel
{
    public interface IAppSettings { }
    public class AppSettings : IAppSettings
    {
        private readonly SecuritySettings _SecuritySettings;
        public AppSettings(SecuritySettings SecuritySettings)
        {
            _SecuritySettings = SecuritySettings;
        }
        public SecuritySettings SecuritySettings
        {
            get
            {
                return _SecuritySettings;
            }
        }
    }

     
    public class SecuritySettings
    {
        private readonly string _JwtSecretKey;
        public SecuritySettings(string JwtSecretKey)
        {
            _JwtSecretKey = JwtSecretKey;
        }
        public string JwtSecretKey
        {
            get
            {
                return _JwtSecretKey;
            }
        }
    }

}

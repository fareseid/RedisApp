
using Microsoft.Extensions.Configuration;
using RedisServer.Model.AppModel;

namespace RedisServer.Helper
{
    public static class AppSettingsHelper
    { 
        public static AppSettings RetrieveAppSettings()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var JwtSecretKey = config.GetSection("SecuritySettings")["JwtSecretKey"];
            return new AppSettings(new SecuritySettings(JwtSecretKey));
        } 
    }
}

using Microsoft.Extensions.Configuration;
using RedisClient.Models.AppModels;

namespace RedisClient.Helpers
{
    public static class AppSettingsHelper
    {
        public static AppSettings RetrieveAppSettings()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                 .AddJsonFile($"appsettings.json", true, true)
                 .Build();
             
            string RedisServerHost = config["RedisServerHost"];
            return new AppSettings(RedisServerHost);
        }
    }
}
using RedisClient.Helpers;
using RedisClient.Models.AppModels;
using RedisClient.RedisIO;
using RedisClient.RedisService.RedisServiceClient;
using System.Threading.Tasks;

namespace RedisClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppSettings AppSettings = AppSettingsHelper.RetrieveAppSettings();  

            await new ClientApp(new RedisConsoleIO(), new RedisRestServiceClient(AppSettings))
                .LaunchApp();
        } 
    }
}

using RedisClient.Models.RestAPIModels;
using RedisClient.RedisService.RedisServiceClient;
using System.Threading.Tasks;

namespace RedisClient.RedisService
{
    public class RedisServiceCaller
    {
        private readonly IServiceClient _ServiceClient;

        public RedisServiceCaller(IServiceClient ServiceClient) {
            _ServiceClient = ServiceClient;

        }

        public Task<string> Authenticate(AuthenticateModel AuthModel)
        {
            return _ServiceClient.Authenticate(AuthModel);
        }

        public Task<string> LaunchCommand(LaunchCommandModel CommandModel)
        {
            return _ServiceClient.LaunchCommand(CommandModel);
        }
    }
}

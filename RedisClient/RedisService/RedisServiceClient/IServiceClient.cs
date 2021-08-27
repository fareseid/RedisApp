using RedisClient.Models.RestAPIModels;
using System.Threading.Tasks;

namespace RedisClient.RedisService.RedisServiceClient
{
    public interface IServiceClient
    {
        Task<string> Authenticate(AuthenticateModel AuthModel);
        Task<string> LaunchCommand(LaunchCommandModel CommandModel);
    }
}

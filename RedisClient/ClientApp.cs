using RedisClient.Models.AppModels;
using RedisClient.Models.RestAPIModels;
using RedisClient.RedisIO;
using RedisClient.RedisService;
using RedisClient.RedisService.RedisServiceClient;
using System;
using System.Threading.Tasks;

namespace RedisClient
{
    public class ClientApp
    {
        private const string LOG_OUT = "LOGOUT";
        private const string EXIT = "EXIT";
        private const string UNAUTHORIZED_RESPONSE = "-1";

        private readonly RedisServiceCaller _RedisServiceCaller;
        private readonly AbstractRedisIO _RedisIO;
        private string JwtToken;

        public ClientApp(AbstractRedisIO RedisIO,
            IServiceClient ServiceClient) {
            _RedisIO = RedisIO;
            _RedisServiceCaller = new RedisServiceCaller(ServiceClient);
        }

        public async Task LaunchApp() { 
            while (1 == 1)
            {
                if (JwtToken == null)
                {
                    RedisUser User = _RedisIO.RetrieveUser();
                    JwtToken = await Login(User); 
                    RaiseErrorIfNull(JwtToken);
                }
                else
                {
                    string Command = _RedisIO.RetrieveCommand();

                    ResetJwtTokenIfLogout(Command);
                    CloseApplicationIfExit(Command);

                    if (JwtToken != null)
                    {
                        string Response = await LaunchCommand(Command, JwtToken);
                        if (Response == UNAUTHORIZED_RESPONSE)
                        {
                            JwtToken = null;
                            Response = "Kindly login again.";
                        }
                        _RedisIO.Print(Response); 
                    }
                }
            }
        }

        private async Task<string> Login(RedisUser User)
        {
            AuthenticateModel Model = new AuthenticateModel
            {
                Username = User.Username,
                Password = User.Password
            };
            return await _RedisServiceCaller.Authenticate(Model);
        }

        private async Task<string> LaunchCommand(string Command, string JwtToken)
        {
            LaunchCommandModel Model = new LaunchCommandModel
            {
                Command = Command,
                JwtToken = JwtToken
            };
            return await _RedisServiceCaller.LaunchCommand(Model);
        }

        private void RaiseErrorIfNull(string JwtToken)
        {
            if (JwtToken == null)
            {
                _RedisIO.Print("An error has occurred!"); 
            }
        }

        private void CloseApplicationIfExit(string Command)
        {
            if (Command.ToUpper() == EXIT)
            {
                Environment.Exit(0);
            }
        }

        private void ResetJwtTokenIfLogout(string Command)
        {
            if (Command.ToUpper() == LOG_OUT)
            {
                JwtToken = null;
            }
        }
    }
}

using Newtonsoft.Json;
using RedisClient.Models.AppModels;
using RedisClient.Models.RestAPIModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RedisClient.RedisService.RedisServiceClient
{
    public class RedisRestServiceClient: IServiceClient
    { 
        private readonly HttpClient _Client;
        private readonly AppSettings _AppSettings;

        public RedisRestServiceClient( AppSettings AppSettings)
        {
            _Client = new HttpClient();
            _AppSettings = AppSettings;

            _Client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
        }

        public async Task<string> Authenticate(AuthenticateModel AuthModel)
        {
            StringContent data = BuildRequestBody(AuthModel);

            string url = _AppSettings.RedisServerHost + "/Auth";

            HttpResponseMessage response = await _Client.PostAsync(url, data);
            return BuildAuthenticateResponse(response);
        }

        public async Task<string> LaunchCommand(LaunchCommandModel CommandModel)
        {
            StringContent data = BuildRequestBody(CommandModel); 

            _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommandModel.JwtToken);

            string url = _AppSettings.RedisServerHost + "/Command";

            HttpResponseMessage response = await _Client.PostAsync(url, data);
            return await BuildCommandResponse(response);
        }

        private static StringContent BuildRequestBody(object Model)
        {
            string ModelJson = JsonConvert.SerializeObject(Model);
            StringContent data = new StringContent(ModelJson, Encoding.UTF8, "application/json");
            return data;
        }

        private static string BuildAuthenticateResponse(HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode ? (string)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result) : null;
        }

        private static async Task<string> BuildCommandResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) { 
                    return "-1";
                }
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}

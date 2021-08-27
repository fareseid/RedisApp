namespace RedisServer.Model.APIModel
{
    public class IAuthenticationReturnModel
    { 
    }
    public class JwtAuthenticationModel: IAuthenticationReturnModel
    {
        public string JwtToken { get; set; }
    }
}

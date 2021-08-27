using Microsoft.IdentityModel.Tokens;
using RedisServer.Helper;
using RedisServer.Logging;
using RedisServer.Model.APIModel;
using RedisServer.Model.RedisModel;
using RedisServer.Repository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RedisServer.Service
{
    public interface IAuthService
    {
        IAuthenticationReturnModel Authenticate(AuthenticationModel AuthModel);
    }

    public class JwtAuthService : IAuthService
    {
        private readonly IRepository _Repository; 
        private readonly IRedisLogger _RedisLogger;

        public JwtAuthService(
            IRepository Repository, 
            IRedisLogger RedisLogger)
        {
            _Repository = Repository; 
            _RedisLogger = RedisLogger;
        }

        public IAuthenticationReturnModel Authenticate(AuthenticationModel AuthModel)
        {
            _RedisLogger.Trace("Authenticating User : {0}", AuthModel.Username);
            if (!_Repository.Authenticate(AuthModel.Username, AuthModel.Password)) {
                return null;
            }
            return new JwtAuthenticationModel
            {
                JwtToken = GenerateTokenString(_Repository.GetUserByUsername(AuthModel.Username))
            };
        }
        
        public static string GenerateTokenString(User User)
        {
            JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
            byte[] Key = Encoding.ASCII.GetBytes(AppSettingsHelper.RetrieveAppSettings().SecuritySettings.JwtSecretKey);
            SecurityTokenDescriptor TokenDescriptor = BuildTokenDescriptor(User, Key);

            SecurityToken Token = TokenHandler.CreateToken(TokenDescriptor);

            return TokenHandler.WriteToken(Token);
        }

        private static SecurityTokenDescriptor BuildTokenDescriptor(User User, byte[] Key)
        {
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, User.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, User.Email),
                        new Claim("id", User.Id.ToString())
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(720), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "RedisCloneServer",
                Audience = "RedisCloneClient"
            };
            return TokenDescriptor;
        }
    }
}

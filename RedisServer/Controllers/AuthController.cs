using Microsoft.AspNetCore.Mvc;
using RedisServer.Model.APIModel;
using RedisServer.Service;

namespace RedisServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {  
        private readonly IAuthService _RedisAuthService;

        public AuthController(IAuthService RedisAuthService)
        {
            _RedisAuthService = RedisAuthService; 
        }
        
        [HttpPost] 
        public ActionResult Login([FromBody] AuthenticationModel model)
        {
            IAuthenticationReturnModel jwtModel = _RedisAuthService.Authenticate(model);
            if (jwtModel == null) { 
                return Unauthorized("Wrong Username or Password");
            }
            else { 
                return Ok(((JwtAuthenticationModel)jwtModel).JwtToken);
            } 
        }
    }
}

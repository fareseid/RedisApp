using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedisServer.Logging;
using RedisServer.Model.APIModel;
using RedisServer.Service;

namespace RedisServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IRedisLogger _Logger;
        private readonly IRedisService _RedisService;

        public CommandController(IRedisService RedisCallerService,
            IRedisLogger Logger) {
            _RedisService = RedisCallerService;
            _Logger = Logger;
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult Launch([FromBody] LaunchCommandModel Model)
        { 
            _Logger.Trace("Requested a Launch Command API : {0}", Model.Command);
            string Response = _RedisService.Launch(Model.Command);
            return Ok(Response);
        } 
    }
}
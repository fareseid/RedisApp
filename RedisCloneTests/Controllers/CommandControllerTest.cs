using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RedisServer.Controllers;
using RedisServer.Logging;
using RedisServer.Model.APIModel;
using RedisServer.Service;
using System.Net;
using static RedisCloneTests.Utils.AssertionUtils;

namespace RedisCloneTests.Controllers
{
    [TestClass]
    public class CommandControllerTest
    {
        private CommandController _CommandController;  
        private Mock<IRedisService> _RedisService;
        private Mock<IRedisLogger> _Logger;

        [TestInitialize]
        public void Setup() { 
            _RedisService = new Mock<IRedisService>(); 
            _Logger = new Mock<IRedisLogger>();
            _CommandController = new CommandController(_RedisService.Object, _Logger.Object);
        }

        [TestMethod]
        public void Should_Return_Response_From_Service_On_Launch()
        {
            //given
            LaunchCommandModel Model = BuildLaunchCommand("set test 1");
            _RedisService.Setup(x => x.Launch(Model.Command)).Returns("Response From Launch Command");

            //when
            ActionResult ActualResponse = _CommandController.Launch(Model);

            //then   
            AssertOnHttpResponse(ActualResponse, HttpStatusCode.OK, "Response From Launch Command");
        }

        [TestMethod]
        public void Should_Call_Service_With_Correct_Command()
        {
            //given
            LaunchCommandModel Model = BuildLaunchCommand("set test 1");

            //when
            _CommandController.Launch(Model);

            //then   
            _RedisService.Verify(x => x.Launch("set test 1"));
        }

        private static LaunchCommandModel BuildLaunchCommand(string Command) { 
            return new LaunchCommandModel { Command = Command };
        }
    }
}

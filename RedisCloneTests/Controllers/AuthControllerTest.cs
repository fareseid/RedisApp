using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RedisServer.Controllers;
using RedisServer.Model.APIModel;
using RedisServer.Service;
using System.Net;
using static RedisCloneTests.Utils.AssertionUtils;

namespace RedisCloneTests.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        private AuthController _AuthController;
        private Mock<IAuthService> _AuthService; 

        [TestInitialize]
        public void Setup()
        {
            _AuthService = new Mock<IAuthService>(); 
            _AuthController = new AuthController(_AuthService.Object);
        }

        [TestMethod]
        public void Should_Return_Success_Response_From_Service_On_Login()
        {
            //given
            AuthenticationModel Model = BuildAuthenticationModel("User1", "2AC9CB7DC02B3C0083EB70898E549B63");
            _AuthService.Setup(x => x.Authenticate(Model)).Returns(new JwtAuthenticationModel { JwtToken = "TokenForUser1" });

            //when
            ActionResult ActualResponse = _AuthController.Login(Model);

            //then    
            AssertOnHttpResponse(ActualResponse, HttpStatusCode.OK, "TokenForUser1");
        }

        [TestMethod]
        public void Should_Return_Unauthorized_Response_From_Service_On_Login()
        {
            //given
            AuthenticationModel Model = BuildAuthenticationModel("User1", "WrongPassword");
            _AuthService.Setup(x => x.Authenticate(Model)).Returns((JwtAuthenticationModel)null);

            //when
            ActionResult ActualResponse = _AuthController.Login(Model);

            //then  
            AssertOnHttpResponse(ActualResponse,HttpStatusCode.Unauthorized, "Wrong Username or Password");
        }

        [TestMethod]
        public void Should_Call_Service_With_Correct_Credentials()
        {
            //given
            AuthenticationModel Model = BuildAuthenticationModel("User1", "2AC9CB7DC02B3C0083EB70898E549B63");

            //when
            _AuthController.Login(Model);

            //then   
            _AuthService.Verify(x => x.Authenticate(Model));
        } 

        private static AuthenticationModel BuildAuthenticationModel(string Username, string Password)
        {
            return new AuthenticationModel { Username = Username,Password=Password };
        }
    }
}
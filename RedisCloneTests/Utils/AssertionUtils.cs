using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedisServer.Helper;
using System;
using System.Net;

namespace RedisCloneTests.Utils
{
    public static class AssertionUtils
    {
        public static void AssertOnAppException(Exception Ex, string ExpectedMessage) { 
            Assert.AreEqual(typeof(AppException), Ex.GetType());
            Assert.AreEqual(ExpectedMessage, Ex.Message);
        }

        public static void AssertOnHttpResponse(ActionResult ActualResponse, HttpStatusCode ExpectedResponseStatusCode, string ExpectedResponseMessage)
        {
            Assert.AreEqual((int)ExpectedResponseStatusCode, ((ObjectResult)ActualResponse).StatusCode);
            Assert.AreEqual(ExpectedResponseMessage, ((ObjectResult)ActualResponse).Value);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedisServer.Model.CommandModel;
using RedisServer.Parser;
using System;
using static RedisCloneTests.Utils.AssertionUtils;
using static RedisCloneTests.Utils.RedisCommandUtils;
using static RedisServer.Model.CommandModel.CommandType;

namespace RedisCloneTests.Parser
{
    [TestClass]
    public class RedisGetOperationParserTest
    {
        #region Get 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Two_Parameters_On_Get()
        {
            //given 
            string[] CommandParameters = { "get" } ;

            try
            {
                //when
                RedisGetOperationParser.BuildGetCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid GET Command");
            }
        } 

        [TestMethod]
        public void Should_Build_RedisCommand_On_Get()
        {
            //given 
            string[] CommandParameters = { "get", "List1" };

            //when
            RedisCommand ActualRedisCommand = RedisGetOperationParser.BuildGetCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", EnumCommandType.GET);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion
    }
}

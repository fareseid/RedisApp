using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedisServer.Model.CommandModel;
using RedisServer.Parser;
using System;
using static RedisCloneTests.Utils.AssertionUtils;
using static RedisCloneTests.Utils.RedisCommandUtils;
using static RedisServer.Model.CommandModel.CommandType;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisCloneTests.Parser
{
    [TestClass]
    public class RedisSetOperationParserTest
    {
        #region Set 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Three_Parameters_On_Set()
        {
            //given 
            string[] CommandParameters = { "set","Item1" } ;

            try
            {
                //when
                RedisSetOperationParser.BuildSetCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid Set Command");
            }
        } 

        [TestMethod]
        public void Should_Build_RedisCommand_On_Set()
        {
            //given 
            string[] CommandParameters = { "set", "Item1", "Value1" };

            //when
            RedisCommand ActualRedisCommand = RedisSetOperationParser.BuildSetCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("Item1","Value1", EnumCommandType.SET, EnumVariableType.STRING);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion
    }
}

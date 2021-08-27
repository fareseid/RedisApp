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
    public class RedisExpireOperationParserTest
    {
        #region Expire 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Three_Parameters_On_Expire()
        {
            //given 
            string[] CommandParameters = { "expire", "Item1" } ;

            try
            {
                //when
                RedisExpireOperationParser.BuildCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid EXPIRE Command");
            }
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Third_Parameter_Not_Int_On_Expire()
        {
            //given 
            string[] CommandParameters = { "expire", "List1", "Value0" };

            try
            {
                //when
                RedisExpireOperationParser.BuildCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid EXPIRE Command");
            }
        }

        [TestMethod]
        public void Should_Build_RedisCommand_On_Expire()
        {
            //given 
            string[] CommandParameters = { "expire", "List1", "0" };

            //when
            RedisCommand ActualRedisCommand = RedisExpireOperationParser.BuildCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", 0, EnumCommandType.EXPIRE, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion
    }
}

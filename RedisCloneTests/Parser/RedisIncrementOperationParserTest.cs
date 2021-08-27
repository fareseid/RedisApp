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
    public class RedisIncrementOperationParserTest
    {
        #region Increment 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Two_Parameters_On_Increment()
        {
            //given 
            string[] CommandParameters = { "incr" } ;

            try
            {
                //when
                RedisIncrementOperationParser.BuildIncrCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid INCR Command");
            }
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Third_Parameter_Not_Int_On_Increment()
        {
            //given 
            string[] CommandParameters = { "incr", "Item1", "Value0" };

            try
            {
                //when
                RedisIncrementOperationParser.BuildIncrCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid INCR Command");
            }
        }

        [TestMethod]
        public void Should_Build_RedisCommand_With_Default_Value_On_Increment()
        {
            //given 
            string[] CommandParameters = { "incr", "Item1" };

            //when
            RedisCommand ActualRedisCommand = RedisIncrementOperationParser.BuildIncrCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("Item1", 1, EnumCommandType.INCR, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_RedisCommand_On_Increment()
        {
            //given 
            string[] CommandParameters = { "incr", "Item1", "2" };

            //when
            RedisCommand ActualRedisCommand = RedisIncrementOperationParser.BuildIncrCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("Item1", 2, EnumCommandType.INCR, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion

        #region Decrement 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Two_Parameters_On_Decrement()
        {
            //given 
            string[] CommandParameters = { "decr" };

            try
            {
                //when
                RedisIncrementOperationParser.BuildDecrCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid DECR Command");
            }
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Third_Parameter_Not_Int_On_Decrement()
        {
            //given 
            string[] CommandParameters = { "decr", "Item1", "Value0" };

            try
            {
                //when
                RedisIncrementOperationParser.BuildDecrCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid DECR Command");
            }
        }

        [TestMethod]
        public void Should_Build_RedisCommand_With_Default_Value_On_Decrement()
        {
            //given 
            string[] CommandParameters = { "decr", "Item1" };

            //when
            RedisCommand ActualRedisCommand = RedisIncrementOperationParser.BuildDecrCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("Item1", 1, EnumCommandType.DECR, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_RedisCommand_On_Decrement()
        {
            //given 
            string[] CommandParameters = { "decr", "Item1", "2" };

            //when
            RedisCommand ActualRedisCommand = RedisIncrementOperationParser.BuildDecrCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("Item1", 2, EnumCommandType.DECR, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion
    }
}

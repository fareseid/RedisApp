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
    public class RedisContainerOperationParserTest
    {
        #region RPush 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Three_Parameters_On_RPush()
        {
            //given 
            string[] CommandParameters = { "rpush","List1"} ;

            try
            {
                //when
                RedisContainerOperationParser.BuildRPushCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid RPUSH Command");
            }
        }

        [TestMethod]
        public void Should_Build_RedisCommand_With_String_Value_On_RPush()
        {
            //given 
            string[] CommandParameters = { "rpush", "List1","Value0" };
             
            //when
            RedisCommand ActualRedisCommand = RedisContainerOperationParser.BuildRPushCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", "Value0", EnumCommandType.RPUSH, EnumVariableType.STRING);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_RedisCommand_With_Int_Value_On_RPush()
        {
            //given 
            string[] CommandParameters = { "rpush", "List1", "2" };

            //when
            RedisCommand ActualRedisCommand = RedisContainerOperationParser.BuildRPushCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", "2", EnumCommandType.RPUSH, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion

        #region LPush 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Three_Parameters_On_LPush()
        {
            //given 
            string[] CommandParameters = { "lpush", "List1" };

            try
            {
                //when
                RedisContainerOperationParser.BuildLPushCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid LPUSH Command");
            }
        }

        [TestMethod]
        public void Should_Build_RedisCommand_With_String_Value_On_LPush()
        {
            //given 
            string[] CommandParameters = { "lpush", "List1", "Value0" };

            //when
            RedisCommand ActualRedisCommand = RedisContainerOperationParser.BuildLPushCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", "Value0", EnumCommandType.LPUSH, EnumVariableType.STRING);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_RedisCommand_With_Int_Value_On_LPush()
        {
            //given 
            string[] CommandParameters = { "lpush", "List1", "2" };

            //when
            RedisCommand ActualRedisCommand = RedisContainerOperationParser.BuildLPushCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", "2", EnumCommandType.LPUSH, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion

        #region RPOP 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Two_Parameters_On_RPop()
        {
            //given 
            string[] CommandParameters = { "rpop" };

            try
            {
                //when
                RedisContainerOperationParser.BuildRPopCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid RPOP Command");
            }
        }

        [TestMethod]
        public void Should_Build_RedisCommand_On_RPop()
        {
            //given 
            string[] CommandParameters = { "rpop", "List1" };

            //when
            RedisCommand ActualRedisCommand = RedisContainerOperationParser.BuildRPopCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", EnumCommandType.RPOP);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion

        #region LPOP 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Two_Parameters_On_LPop()
        {
            //given 
            string[] CommandParameters = { "lpop" };

            try
            {
                //when
                RedisContainerOperationParser.BuildLPopCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid LPOP Command");
            }
        }

        [TestMethod]
        public void Should_Build_RedisCommand_On_LPop()
        {
            //given 
            string[] CommandParameters = { "lpop", "List1" };

            //when
            RedisCommand ActualRedisCommand = RedisContainerOperationParser.BuildLPopCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", EnumCommandType.LPOP);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion

        #region LIndex 
        [TestMethod]
        public void Should_Throw_Exception_If_Less_Than_Three_Parameters_On_LIndex()
        {
            //given 
            string[] CommandParameters = { "lindex", "List1" };

            try
            {
                //when
                RedisContainerOperationParser.BuildLIndexCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid LINDEX Command");
            }
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Third_Parameter_Not_Int_On_LIndex()
        {
            //given 
            string[] CommandParameters = { "lindex", "List1", "Value0" };

            try
            {
                //when
                RedisContainerOperationParser.BuildLIndexCommand(CommandParameters);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid LINDEX Command");
            }
        }

        [TestMethod]
        public void Should_Build_RedisCommand_On_LIndex()
        {
            //given 
            string[] CommandParameters = { "lindex", "List1", "0" };

            //when
            RedisCommand ActualRedisCommand = RedisContainerOperationParser.BuildLIndexCommand(CommandParameters);

            //then    
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", 0, EnumCommandType.LINDEX, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion
    }
}

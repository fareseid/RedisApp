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
    public class RedisCommandParserTest
    {
        private RedisCommandParser RedisCommandParser;
        
        [TestInitialize]
        public void Setup()
        {
            RedisCommandParser = new RedisCommandParser();
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Empty_String()
        {
            //given 
            string Command = "";

            try
            {
                //when
                RedisCommandParser.Parse(Command);

            } catch (Exception Ex) { 
                //then   
                AssertOnAppException(Ex, "Invalid Command"); 
            }
        }

        [TestMethod]
        public void Should_Throw_Exception_If_String_Contains_Only_WhiteSpace()
        {
            //given 
            string Command = "               ";

            try
            {
                //when
                RedisCommandParser.Parse(Command);

            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Invalid Command");
            }
        }

        #region ContainerCommand
        [TestMethod]
        public void Should_Build_Correct_RPush_Container_Command()
        {
            //given 
            string Command = "   rpush       List1       Value0       ";
             
            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", "Value0", EnumCommandType.RPUSH, EnumVariableType.STRING);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_Correct_LPush_Container_Command()
        {
            //given 
            string Command = "   lpush       List1       Value0       ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", "Value0", EnumCommandType.LPUSH, EnumVariableType.STRING);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_Correct_RPop_Container_Command()
        {
            //given 
            string Command = "   rpop       List1       ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", EnumCommandType.RPOP);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_Correct_LPop_Container_Command()
        {
            //given 
            string Command = "   lpop       List1       ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", EnumCommandType.LPOP);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_Correct_LIndex_Container_Command()
        {
            //given 
            string Command = "   lindex       List1       0       ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", 0, EnumCommandType.LINDEX, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }
        #endregion


        #region IncrementCommand
        [TestMethod]
        public void Should_Build_Correct_Incr_Command()
        {
            //given 
            string Command = "   incr       Item1         2     ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("Item1", 2, EnumCommandType.INCR, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        [TestMethod]
        public void Should_Build_Correct_Decr_Command()
        {
            //given 
            string Command = "   decr       Item1              ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("Item1", 1, EnumCommandType.DECR, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        #endregion

        #region ExpireCommand
        [TestMethod]
        public void Should_Build_Correct_Expire_Command()
        {
            //given 
            string Command = "   expire       List1       0       ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", 0, EnumCommandType.EXPIRE, EnumVariableType.INT);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        #endregion

        #region GetCommand
        [TestMethod]
        public void Should_Build_Correct_Get_Command()
        {
            //given 
            string Command = "   get       List1              ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("List1", EnumCommandType.GET);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        #endregion

        #region SetCommand
        [TestMethod]
        public void Should_Build_Correct_Set_Command()
        {
            //given 
            string Command = "   set       Item1              Value1    ";

            //when
            RedisCommand ActualRedisCommand = RedisCommandParser.Parse(Command);

            //then   
            RedisCommand ExpectedRedisCommand = BuildRedisCommand("Item1","Value1", EnumCommandType.SET,EnumVariableType.STRING);
            Assert.AreEqual(ExpectedRedisCommand, ActualRedisCommand);
        }

        #endregion
    }
}

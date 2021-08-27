using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Model.RedisModel;
using RedisServer.RedisOperations;
using static RedisCloneTests.Utils.RedisCommandUtils;
using static RedisCloneTests.Utils.RedisItemUtils;
using static RedisServer.Model.CommandModel.CommandType;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisCloneTests.RedisOperation
{
    [TestClass]
    public class RedisOperationLauncherTest
    {
        private RedisOperationLauncher _RedisOperationLauncher;
        private Mock<IRedisLogger> _Logger; 
        private Mock<IRedisGetOperation> _RedisGetOperation;
        private Mock<IRedisContainerOperation> _RedisContainerOperation;
        private Mock<IRedisExpireOperation> _RedisExpireOperation;
        private Mock<IRedisIncrementOperation> _RedisIncrementOperation;
        private Mock<IRedisSetOperation> _RedisSetOperation;

        [TestInitialize]
        public void Setup()
        {
            _Logger = new Mock<IRedisLogger>();
            _RedisGetOperation = new Mock<IRedisGetOperation>();
            _RedisContainerOperation = new Mock<IRedisContainerOperation>();
            _RedisExpireOperation = new Mock<IRedisExpireOperation>();
            _RedisIncrementOperation = new Mock<IRedisIncrementOperation>();
            _RedisSetOperation = new Mock<IRedisSetOperation>();
            _RedisOperationLauncher = new RedisOperationLauncher(_Logger.Object, _RedisGetOperation.Object, _RedisContainerOperation.Object, _RedisExpireOperation.Object, _RedisIncrementOperation.Object, _RedisSetOperation.Object);
        }

        #region GetLauncher 
        [TestMethod]
        public void Should_Return_Nil_If_Returns_Null_On_Get()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", EnumCommandType.GET);
            object ExistingItem = null;
            _RedisGetOperation.Setup(x => x.Get(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "(nil)";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisGetOperation.Verify(x => x.Get(Model));
        }

        [TestMethod]
        public void Should_Return_Object_On_Get()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", EnumCommandType.GET);
            int ExistingItem = 2;
            _RedisGetOperation.Setup(x => x.Get(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "2";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisGetOperation.Verify(x => x.Get(Model));
        }
        #endregion

        #region SetLauncher 
        [TestMethod]
        public void Should_Return_Ok_On_Set()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "2", EnumCommandType.SET, EnumVariableType.INT);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "OK";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisSetOperation.Verify(x => x.Set(Model));
        }
        #endregion

        #region IncrementLauncher 
        [TestMethod]
        public void Should_Return_Correct_Result_On_Increment()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "2", EnumCommandType.INCR, EnumVariableType.INT);
            IRedisItem ExistingItem = BuildRedisIntItem(2);
            _RedisIncrementOperation.Setup(x => x.Increment(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "(integer) 2";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisIncrementOperation.Verify(x => x.Increment(Model));
        }

        [TestMethod]
        public void Should_Return_Correct_Result_On_Decrement()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "2", EnumCommandType.DECR, EnumVariableType.INT);
            IRedisItem ExistingItem = BuildRedisIntItem(-2);
            _RedisIncrementOperation.Setup(x => x.Decrement(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "(integer) -2";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisIncrementOperation.Verify(x => x.Decrement(Model));
        }
        #endregion

        #region ExpireLauncher 
        [TestMethod]
        public void Should_Return_0_If_Expire_Returns_Null()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "2", EnumCommandType.EXPIRE, EnumVariableType.INT);
            IRedisItem ExistingItem = null;
            _RedisExpireOperation.Setup(x => x.Expire(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "0";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisExpireOperation.Verify(x => x.Expire(Model));
        }

        [TestMethod]
        public void Should_Return_1_If_Expire_Returns_Item()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "2", EnumCommandType.EXPIRE, EnumVariableType.INT);
            IRedisItem ExistingItem = BuildRedisIntItem(2);
            _RedisExpireOperation.Setup(x => x.Expire(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "1";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisExpireOperation.Verify(x => x.Expire(Model));
        }
        #endregion

        #region ContainerLauncher 
        [TestMethod]
        public void Should_Return_Nil_If_Lindex_Returns_Null()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("List1", "2", EnumCommandType.LINDEX, EnumVariableType.INT);
            IRedisItem ExistingItem = null;
            _RedisContainerOperation.Setup(x => x.LIndex(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "(nil)";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisContainerOperation.Verify(x => x.LIndex(Model));
        }

        [TestMethod]
        public void Should_Return_Value_On_LIndex()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("List1", "2", EnumCommandType.LINDEX, EnumVariableType.INT);
            IRedisItem ExistingItem = BuildRedisStringItem("Value0");
            _RedisContainerOperation.Setup(x => x.LIndex(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "Value0";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisContainerOperation.Verify(x => x.LIndex(Model));
        }

        [TestMethod]
        public void Should_Return_Nil_If_LPop_Returns_Null()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("List1", EnumCommandType.LPOP);
            IRedisItem ExistingItem = null;
            _RedisContainerOperation.Setup(x => x.PopLeft(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "(nil)";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisContainerOperation.Verify(x => x.PopLeft(Model));
        }

        [TestMethod]
        public void Should_Return_Value_On_LPop()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("List1", EnumCommandType.LPOP);
            IRedisItem ExistingItem = BuildRedisStringItem("Value0");
            _RedisContainerOperation.Setup(x => x.PopLeft(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "Value0";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisContainerOperation.Verify(x => x.PopLeft(Model));
        }

        [TestMethod]
        public void Should_Return_Nil_If_RPop_Returns_Null()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("List1", EnumCommandType.RPOP);
            IRedisItem ExistingItem = null;
            _RedisContainerOperation.Setup(x => x.PopRight(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "(nil)";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisContainerOperation.Verify(x => x.PopRight(Model));
        }

        [TestMethod]
        public void Should_Return_Value_On_RPop()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("List1", EnumCommandType.RPOP);
            IRedisItem ExistingItem = BuildRedisStringItem("Value0");
            _RedisContainerOperation.Setup(x => x.PopRight(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "Value0";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisContainerOperation.Verify(x => x.PopRight(Model));
        }

        [TestMethod]
        public void Should_Return_Value_On_RPush()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("List1","Value0", EnumCommandType.RPUSH, EnumVariableType.STRING);
            IRedisItem ExistingItem = BuildRedisListItem("Value1", "Value2", "Value0");
            _RedisContainerOperation.Setup(x => x.PushRight(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "(integer) 3";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisContainerOperation.Verify(x => x.PushRight(Model));
        }

        [TestMethod]
        public void Should_Return_Value_On_LPush()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("List1", "Value0", EnumCommandType.LPUSH, EnumVariableType.STRING);
            IRedisItem ExistingItem = BuildRedisListItem("Value0", "Value1");
            _RedisContainerOperation.Setup(x => x.PushLeft(Model)).Returns(ExistingItem);

            //when
            string ActualResponse = _RedisOperationLauncher.Launch(Model);

            //then   
            string ExpectedResponse = "(integer) 2";
            Assert.AreEqual(ExpectedResponse, ActualResponse);
            _RedisContainerOperation.Verify(x => x.PushLeft(Model));
        }
        #endregion
    }
}
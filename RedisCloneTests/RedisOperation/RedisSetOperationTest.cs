using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Model.RedisModel;
using RedisServer.RedisOperations;
using RedisServer.Repository;
using System;
using static RedisCloneTests.Utils.AssertionUtils;
using static RedisCloneTests.Utils.RedisCommandUtils;
using static RedisCloneTests.Utils.RedisItemUtils;
using static RedisServer.Model.CommandModel.CommandType;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisCloneTests.RedisOperation
{
    [TestClass]
    public class RedisSetOperationTest
    { 
        private RedisSetOperation _RedisSetOperation;
        private Mock<IRepository> _Repository;
        private Mock<IRedisLogger> _Logger;

        [TestInitialize]
        public void Setup()
        {
            _Repository = new Mock<IRepository>();
            _Logger = new Mock<IRedisLogger>();
            _RedisSetOperation = new RedisSetOperation(_Repository.Object, _Logger.Object);
        }

        #region Set 
        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Settable_Item()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value1", EnumCommandType.SET, EnumVariableType.LIST);
            IRedisItem ExistingItem = null;
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisSetOperation.Set(Model);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod]
        public void Should_Create_Item_On_Set()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value0", EnumCommandType.SET, EnumVariableType.STRING);
            IRedisItem ExistingItem = null;
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualItem = _RedisSetOperation.Set(Model);

            //then   
            RedisStringItem ExpectedItem = BuildRedisStringItem("Value0");
            Assert.AreEqual(ExpectedItem, ActualItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));
        } 

        [TestMethod]
        public void Should_Override_If_Item_Exists()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value1", EnumCommandType.SET, EnumVariableType.STRING);
            IRedisItem ExistingItem = BuildRedisListItem("Value0","Value1");
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when 
            IRedisItem ActualItem = _RedisSetOperation.Set(Model);

            //then   
            RedisStringItem ExpectedItem = BuildRedisStringItem("Value1");
            Assert.AreEqual(ExpectedItem, ActualItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));

        } 
        #endregion 
    }
}
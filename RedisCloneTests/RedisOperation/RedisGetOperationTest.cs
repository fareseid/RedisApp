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
    public class RedisGetOperationTest
    {
        private RedisGetOperation _RedisGetOperation;
        private Mock<IRepository> _Repository;
        private Mock<IRedisLogger> _Logger;

        [TestInitialize]
        public void Setup()
        {
            _Repository = new Mock<IRepository>();
            _Logger = new Mock<IRedisLogger>();
            _RedisGetOperation = new RedisGetOperation(_Repository.Object, _Logger.Object);
        }

        #region Get 
        [TestMethod]
        public void Should_Return_Null_If_Key_No_Found_OnGet()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.GET, EnumVariableType.NA);
            RedisListItem ExistingItem = null;
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            object ActualItem = _RedisGetOperation.Get(Model);

            //then    
            Assert.IsNull(ActualItem);
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Get_Item_On_Get()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.GET, EnumVariableType.NA);
            IRedisItem ExistingItem = new RedisListItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisGetOperation.Get(Model); 
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod]
        public void Should_Return_Object_Value_If_Exists_On_Get()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.GET, EnumVariableType.NA);
            IRedisItem ExistingItem = BuildRedisStringItem("Value1");
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            object ActualItem = _RedisGetOperation.Get(Model);

            //then   
            string ExpectedItem = "Value1";
            Assert.AreEqual(ExpectedItem, ActualItem); 
        } 
        #endregion 
    }
}


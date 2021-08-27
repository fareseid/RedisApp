using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RedisServer.Logging;
using RedisServer.Model.CommandModel;
using RedisServer.Model.RedisModel;
using RedisServer.RedisOperations;
using RedisServer.Repository;
using System;
using System.Threading;
using static RedisCloneTests.Utils.AssertionUtils;
using static RedisCloneTests.Utils.RedisCommandUtils;
using static RedisCloneTests.Utils.RedisItemUtils;
using static RedisServer.Model.CommandModel.CommandType;
using static RedisServer.Model.CommandModel.VariableType;

namespace RedisCloneTests.RedisOperation
{
    [TestClass]
    public class RedisExpireOperationTest
    {
        private RedisExpireOperation _RedisExpireOperation; 
        private Mock<IRepository> _Repository;
        private Mock<IRedisLogger> _Logger;

        [TestInitialize]
        public void Setup()
        {
            _Repository = new Mock<IRepository>();
            _Logger = new Mock<IRedisLogger>();
            _RedisExpireOperation = new RedisExpireOperation(_Repository.Object, _Logger.Object); 
        }
         
        #region Expire 
        [TestMethod]
        public void Should_Return_Null_If_Key_Not_Found()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 0, EnumCommandType.EXPIRE, EnumVariableType.INT);
            IRedisItem ExistingItem = null;
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualListItem = _RedisExpireOperation.Expire(Model);

            //then    
            Assert.IsNull(ActualListItem);
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Expire_Item_On_Expire()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 0, EnumCommandType.EXPIRE, EnumVariableType.INT);
            IRedisItem ExistingItem = new NonExpirableRedisItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisExpireOperation.Expire(Model); 
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod] 
        public void Should_Remove_Item_On_Expire()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 0, EnumCommandType.EXPIRE, EnumVariableType.INT);
            IRedisItem ExistingItem = BuildRedisStringItem("Value1");
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when 
            _RedisExpireOperation.Expire(Model);
            Thread.Sleep(2);

            //then 
            _Repository.Verify(x => x.Remove(Model.Key));  
        }
        #endregion

        private class NonExpirableRedisItem : IRedisItem { 
        
        }
    }
}

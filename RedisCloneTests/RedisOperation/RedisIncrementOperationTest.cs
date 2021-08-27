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
    public class RedisIncrementOperationTest
    {
        private RedisIncrementOperation _RedisIncrementOperation;
        private Mock<IRepository> _Repository;
        private Mock<IRedisLogger> _Logger;

        [TestInitialize]
        public void Setup()
        {
            _Repository = new Mock<IRepository>();
            _Logger = new Mock<IRedisLogger>();
            _RedisIncrementOperation = new RedisIncrementOperation(_Repository.Object, _Logger.Object);
        }

        #region Increment 
        [TestMethod]
        public void Should_Create_New_Item_If_Key_Not_Found_On_Increment()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 5, EnumCommandType.INCR, EnumVariableType.INT);
            IRedisItem ExistingItem = null;
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualItem = _RedisIncrementOperation.Increment(Model);

            //then   
            RedisIntItem ExpectedItem = BuildRedisIntItem(5);
            Assert.AreEqual(ExpectedItem, ActualItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Incrementable_Item_On_Increment()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 1, EnumCommandType.INCR, EnumVariableType.INT);
            IRedisItem ExistingItem = new RedisStringItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisIncrementOperation.Increment(Model);
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod]
        public void Should_Increment_Int_OnIncrement()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 2, EnumCommandType.INCR, EnumVariableType.INT);
            IRedisItem ExistingItem = BuildRedisIntItem(4);
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when 
            IRedisItem ActualItem = _RedisIncrementOperation.Increment(Model);

            //then   
            RedisIntItem ExpectedItem = BuildRedisIntItem(6);
            Assert.AreEqual(ExpectedItem, ActualItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));

        } 
        #endregion 

        #region Decrement 
        [TestMethod]
        public void Should_Create_New_Item_If_Key_Not_Found_On_Decrement()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 5, EnumCommandType.DECR, EnumVariableType.INT);
            IRedisItem ExistingItem = null;
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualItem = _RedisIncrementOperation.Decrement(Model);

            //then   
            RedisIntItem ExpectedItem = BuildRedisIntItem(-5);
            Assert.AreEqual(ExpectedItem, ActualItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Decrementable_Item_On_Decrement()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 5, EnumCommandType.DECR, EnumVariableType.INT);
            IRedisItem ExistingItem = new RedisStringItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisIncrementOperation.Decrement(Model);

            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod]
        public void Should_Decrement_Int_OnDecrement()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 2, EnumCommandType.DECR, EnumVariableType.INT);
            IRedisItem ExistingItem = BuildRedisIntItem(4);
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when 
            IRedisItem ActualItem = _RedisIncrementOperation.Decrement(Model);

            //then   
            RedisIntItem ExpectedItem = BuildRedisIntItem(2);
            Assert.AreEqual(ExpectedItem, ActualItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));

        } 
        #endregion 
    }
}
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
    public class RedisContainerOperationTest
    {
        private RedisContainerOperation _RedisContainerOperation;
        private Mock<IRepository> _Repository;
        private Mock<IRedisLogger> _Logger;

        [TestInitialize]
        public void Setup()
        {
            _Repository = new Mock<IRepository>();
            _Logger = new Mock<IRedisLogger>();
            _RedisContainerOperation = new RedisContainerOperation(_Repository.Object, _Logger.Object);
        }

        #region RPush

        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Container_Item_On_RPush()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value1", EnumCommandType.RPUSH, EnumVariableType.STRING);
            IRedisItem ExistingItem = new RedisStringItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisContainerOperation.PushRight(Model);

            } catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            } 
        }

        [TestMethod]
        public void Should_Create_New_Item_If_Key_Not_Found_On_RPush()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value1", EnumCommandType.RPUSH, EnumVariableType.STRING);
            IRedisItem ExistingItem = null;
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualListItem = _RedisContainerOperation.PushRight(Model);

            //then   
            RedisListItem ExpectedItem = BuildRedisListItem("Value1");
            Assert.AreEqual(ExpectedItem,ActualListItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));
        }

        [TestMethod]
        public void Should_Append_To_Existing_Item_To_The_Right_On_RPush()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value1", EnumCommandType.RPUSH, EnumVariableType.STRING); 
            RedisListItem ExistingItem = BuildRedisListItem("Value0");  
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualListItem = _RedisContainerOperation.PushRight(Model);

            //then   
            RedisListItem ExpectedItem = BuildRedisListItem("Value0","Value1");   
            Assert.AreEqual(ExpectedItem, ActualListItem);
            Assert.AreEqual(BuildRedisStringItem("Value0"), ((RedisListItem)ActualListItem).LIndex(0));
            Assert.AreEqual(BuildRedisStringItem("Value1"), ((RedisListItem)ActualListItem).LIndex(1));
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));
        }
        #endregion
        
        #region LPush 
        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Container_Item_On_LPush()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value1", EnumCommandType.LPUSH, EnumVariableType.STRING);
            IRedisItem ExistingItem = new RedisStringItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisContainerOperation.PushLeft(Model);

            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod]
        public void Should_Create_New_Item_If_Key_Not_Found_On_LPush()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value1", EnumCommandType.LPUSH, EnumVariableType.STRING);
            IRedisItem ExistingItem = null;
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualListItem = _RedisContainerOperation.PushLeft(Model);

            //then   
            RedisListItem ExpectedItem = BuildRedisListItem("Value1");
            Assert.AreEqual(ExpectedItem, ActualListItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));
        }

        [TestMethod]
        public void Should_Append_To_Existing_Item_To_The_Left_On_LPush()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", "Value1", EnumCommandType.LPUSH, EnumVariableType.STRING);
            RedisListItem ExistingItem = BuildRedisListItem("Value0");
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualListItem = _RedisContainerOperation.PushLeft(Model);

            //then   
            RedisListItem ExpectedItem = BuildRedisListItem("Value1", "Value0");
            Assert.AreEqual(ExpectedItem, ActualListItem);
            Assert.AreEqual(BuildRedisStringItem("Value1"), ((RedisListItem)ActualListItem).LIndex(0));
            Assert.AreEqual(BuildRedisStringItem("Value0"), ((RedisListItem)ActualListItem).LIndex(1));
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedItem));
        }
        #endregion
         
        #region LPop  
        [TestMethod]
        public void Should_Return_Null_If_Container_Empty_OnLPop()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.LPOP, EnumVariableType.NA);
            RedisListItem ExistingItem = new RedisListItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualListItem = _RedisContainerOperation.PopLeft(Model);

            //then   
            Assert.IsNull(ActualListItem);
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Container_Item_On_LPop()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.LPOP, EnumVariableType.NA);
            IRedisItem ExistingItem = new RedisStringItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisContainerOperation.PopLeft(Model); 
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod]
        public void Should_Append_To_Existing_Item_To_The_Left_On_LPop()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.LPOP, EnumVariableType.NA);
            RedisListItem ExistingItem = BuildRedisListItem("Value0","Value1","Value2");
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualPoppedItem = _RedisContainerOperation.PopLeft(Model);

            //then   
            RedisListItem ExpectedListItem = BuildRedisListItem("Value1", "Value2");
            Assert.AreEqual(BuildRedisStringItem("Value1"), ExistingItem.LIndex(0));
            Assert.AreEqual(BuildRedisStringItem("Value2"), ExistingItem.LIndex(1));
            Assert.AreEqual(BuildRedisStringItem("Value0"), ActualPoppedItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedListItem));
        }
        #endregion
         
        #region RPop 
        [TestMethod]
        public void Should_Return_Null_If_Container_Empty_OnRPop()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.RPOP, EnumVariableType.NA);
            RedisListItem ExistingItem = new RedisListItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualListItem = _RedisContainerOperation.PopRight(Model);

            //then   
            Assert.IsNull(ActualListItem);
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Container_Item_On_RPop()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.RPOP, EnumVariableType.NA);
            IRedisItem ExistingItem = new RedisStringItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisContainerOperation.PopRight(Model); 
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod]
        public void Should_Append_To_Existing_Item_To_The_Left_On_RPop()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", null, EnumCommandType.RPOP, EnumVariableType.NA);
            RedisListItem ExistingItem = BuildRedisListItem("Value0", "Value1", "Value2");
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualPoppedItem = _RedisContainerOperation.PopRight(Model);

            //then   
            RedisListItem ExpectedListItem = BuildRedisListItem("Value0", "Value1");
            Assert.AreEqual(BuildRedisStringItem("Value0"), ExistingItem.LIndex(0));
            Assert.AreEqual(BuildRedisStringItem("Value1"), ExistingItem.LIndex(1));
            Assert.AreEqual(BuildRedisStringItem("Value2"),ActualPoppedItem);
            _Repository.Verify(x => x.Insert(Model.Key, ExpectedListItem));
        }
        #endregion
         
        #region LIndex 
        [TestMethod]
        public void Should_Return_Null_If_Lindex_Not_Available()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 1, EnumCommandType.LINDEX, EnumVariableType.INT);
            RedisListItem ExistingItem = new RedisListItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualListItem = _RedisContainerOperation.LIndex(Model);

            //then   
            Assert.IsNull(ActualListItem);
        }

        [TestMethod]
        public void Should_Throw_Exception_If_Key_Points_To_Non_Container_Item_On_LIndex()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 1, EnumCommandType.LINDEX, EnumVariableType.INT);
            IRedisItem ExistingItem = new RedisStringItem();
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            try
            {
                //when
                _RedisContainerOperation.LIndex(Model); 
            }
            catch (Exception Ex)
            {
                //then   
                AssertOnAppException(Ex, "Method Not Allowed");
            }
        }

        [TestMethod]
        public void Should_Return_Item_On_LIndex()
        {
            //given 
            RedisCommand Model = BuildRedisCommand("Key1", 2, EnumCommandType.LINDEX, EnumVariableType.INT);
            RedisListItem ExistingItem = BuildRedisListItem("Value0", "Value1", "Value2");
            _Repository.Setup(x => x.Get(Model.Key)).Returns(ExistingItem);

            //when
            IRedisItem ActualItem = _RedisContainerOperation.LIndex(Model);

            //then    
            Assert.AreEqual(BuildRedisStringItem("Value2"), ActualItem); 
        }
        #endregion
    }
}
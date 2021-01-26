using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class ListTNullOrEmptyAttributeTests
    {
        #region GetData
        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetData_Test()
        {
            // Arrange
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(typeof(int));
            MethodInfo methodInfo = null;

            // Act
            var result = listTNullOrEmptyAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetData_TypeAlreadyAList_Test()
        {
            // Arrange
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(typeof(List<int>));
            MethodInfo methodInfo = null;

            // Act
            var result = listTNullOrEmptyAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetData_TypeNonGenericChildOfList_Test()
        {
            // Arrange
            var type = typeof(ListChild);
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(type);
            MethodInfo methodInfo = null;

            // Act
            var result = listTNullOrEmptyAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.IsNull(result[0][0]);
            Assert.AreEqual(type, result[1][0].GetType());
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetData_TypeGenericChildOfList_Test()
        {
            // Arrange
            var type = typeof(ListChild<int>);
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(type);
            MethodInfo methodInfo = null;

            // Act
            var result = listTNullOrEmptyAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.IsNull(result[0][0]);
            Assert.AreEqual(type, result[1][0].GetType());
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetData_TypeListOfLists_Test()
        {
            // Arrange
            var type = typeof(List<List<int>>);
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(type);
            MethodInfo methodInfo = null;

            // Act
            var result = listTNullOrEmptyAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.IsNull(result[0][0]);
            Assert.AreEqual(type, result[1][0].GetType());
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetData_TypeIsCustomIListTImplementation_Test()
        {
            // Arrange
            var type = typeof(CustomList<int>);
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(type);
            MethodInfo methodInfo = null;

            // Act
            var result = listTNullOrEmptyAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.IsNull(result[0][0]);
            Assert.AreEqual(type, result[1][0].GetType());
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetData_TypeIsCustomIListImplementation_Test()
        {
            // Arrange
            var type = typeof(CustomList);
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(type);
            MethodInfo methodInfo = null;

            // Act
            var result = listTNullOrEmptyAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.IsNull(result[0][0]);
            Assert.AreEqual(type, result[1][0].GetType());
            Assert.AreEqual(2, result.Count);
        }
        #endregion

        #region GetDisplayName
        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetDisplayName_Null_Test()
        {
            // Arrange
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(typeof(int));
            MethodInfo methodInfo = null;
            object[] data = new object[] { null };

            // Act
            var result = listTNullOrEmptyAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("Null", result);
        }
        [TestMethod]
        public void ListTNullOrEmptyAttribute_GetDisplayName_EmptyObject_Test()
        {
            // Arrange
            var listTNullOrEmptyAttribute = new ListTNullOrEmptyAttribute(typeof(int));
            MethodInfo methodInfo = null;
            object[] data = new object[] { new List<int>() };

            // Act
            var result = listTNullOrEmptyAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("Empty List", result);
        }
        #endregion
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class ListTNullOrEmptyAttributeTests
    {
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
    }
}

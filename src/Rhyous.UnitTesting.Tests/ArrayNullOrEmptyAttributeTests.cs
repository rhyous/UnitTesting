using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class ArrayNullOrEmptyAttributeTests
    {
        [TestMethod]
        public void ArrayNullOrEmptyAttribute_Constructor_TypeNull_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new ArrayNullOrEmptyAttribute(null);
            });
        }

        [TestMethod]
        public void ArrayNullOrEmptyAttribute_Constructor_TypeNotArrayMakeArray_Test()
        {
            var attribute = new ArrayNullOrEmptyAttribute(typeof(int));
            Assert.AreEqual(typeof(int[]), attribute.ArrayType);
        }

        [TestMethod]
        public void ArrayNullOrEmptyAttribute_GetData_Null_Test()
        {
            var attribute = new ArrayNullOrEmptyAttribute(typeof(int[]));
            var actual = attribute.GetDisplayName(null, new object[] { null });
            Assert.AreEqual("Null", actual);
        }

        [TestMethod]
        public void ArrayNullOrEmptyAttribute_GetDisplayName_Null_Test()
        {
            // Arrange
            var arrayNullOrEmptyAttribute = new ArrayNullOrEmptyAttribute(typeof(int));
            MethodInfo methodInfo = null;
            object[] data = new object[] { null };

            // Act
            var result = arrayNullOrEmptyAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("Null", result);
        }

        [TestMethod]
        public void ArrayNullOrEmptyAttribute_GetDisplayName_EmptyObject_Test()
        {
            // Arrange
            var arrayNullOrEmptyAttribute = new ArrayNullOrEmptyAttribute(typeof(int));
            MethodInfo methodInfo = null;
            object[] data = new object[] { Array.Empty<int>() };

            // Act
            var result = arrayNullOrEmptyAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("Empty Array", result);
        }
    }
}
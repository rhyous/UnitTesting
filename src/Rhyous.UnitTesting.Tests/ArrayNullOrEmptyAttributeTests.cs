using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        public void ArrayNullOrEmptyAttribute_Constructor_TypeNotArray_Test()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new ArrayNullOrEmptyAttribute(typeof(int));
            });
        }

        [TestMethod]
        public void ArrayNullOrEmptyAttribute_GetData_Null_Test()
        {
            var attribute = new ArrayNullOrEmptyAttribute(typeof(int[]));
            var actual = attribute.GetDisplayName(null, new object[] { null });
            Assert.AreEqual("Null", actual);
        }
    }
}
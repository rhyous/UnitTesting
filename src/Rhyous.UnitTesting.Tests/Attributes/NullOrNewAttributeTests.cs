using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class ObjectNullOrNewAttributeTests
    {
        [TestMethod]
        public void ObjectNullOrNewAttribute_Constructor_TypeNull_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new ObjectNullOrNewAttribute(null);
            });
        }

        [TestMethod]
        public void ObjectNullOrNewAttribute_Constructor_TypeNotNullable_Test()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var ObjectNullOrNewAttribute = new ObjectNullOrNewAttribute(typeof(int)); ;
            });
        }

        [TestMethod]
        public void ObjectNullOrNewAttribute_GetData_Null_Test()
        {
            var attribute = new ObjectNullOrNewAttribute(typeof(int[]));
            var actual = attribute.GetDisplayName(null, new object[] { null });
            Assert.AreEqual("Null Int32[]", actual);
        }

        [TestMethod]
        public void ObjectNullOrNewAttribute_GetDisplayName_Null_Test()
        {
            // Arrange
            var ObjectNullOrNewAttribute = new ObjectNullOrNewAttribute(typeof(int?));
            MethodInfo methodInfo = null;
            object[] data = new object[] { null };

            // Act
            var result = ObjectNullOrNewAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("Null Int32?", result);
        }

        [TestMethod]
        public void ObjectNullOrNewAttribute_GetDisplayName_EmptyObject_Test()
        {
            // Arrange
            var ObjectNullOrNewAttribute = new ObjectNullOrNewAttribute(typeof(object));
            MethodInfo methodInfo = null;
            object[] data = new object[] {new object() };

            // Act
            var result = ObjectNullOrNewAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("New Object", result);
        }

        [TestMethod]
        public void ObjectNullOrNewAttribute_GetDisplayName_Empty_SomeClassA_Test()
        {
            // Arrange
            var ObjectNullOrNewAttribute = new ObjectNullOrNewAttribute(typeof(SomeClassA));
            MethodInfo methodInfo = null;
            object[] data = new object[] { new SomeClassA() };

            // Act
            var result = ObjectNullOrNewAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("New SomeClassA", result);
        } public class SomeClassA { };
    }
}
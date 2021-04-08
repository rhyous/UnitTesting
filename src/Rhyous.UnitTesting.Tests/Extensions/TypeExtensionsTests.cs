using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;
using System;

namespace Rhyous.UnitTesting.Tests.Extensions
{

    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void TypeExtensions_IsList_NotGenericChild_True()
        {
            // Arrange
            var listChild = new ListChild();

            // Act
            var result = listChild.GetType().IsIList();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TypeExtensions_IsList_GenericChild_True()
        {
            // Arrange
            var listChild = new ListChild<string>();

            // Act
            var result = listChild.GetType().IsIList();

            // Assert
            Assert.IsTrue(result);
        }

    }
}

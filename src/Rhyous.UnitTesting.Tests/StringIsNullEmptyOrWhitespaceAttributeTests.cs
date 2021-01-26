using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class StringIsNullEmptyOrWhitespaceAttributeTests
    {
        [TestMethod]
        public void StringIsNullEmptyOrWhitespaceAttribute_GetData()
        {
            // Arrange
            var attrib = new StringIsNullEmptyOrWhitespaceAttribute();
            MethodInfo methodInfo = null;

            // Act
            var result = attrib.GetData(methodInfo);

            // Assert
            Assert.AreEqual(6, result.Count());
            foreach (var item in result)
                Assert.AreEqual(1, item.Length);
        }
    }
}
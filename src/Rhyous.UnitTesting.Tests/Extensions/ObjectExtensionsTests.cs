using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.UnitTesting.Tests.Extensions
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        [DataRow(11, "Simple Model", "11", "Id")]
        [DataRow(11, "Simple Model", "Simple Model", "Name")]
        [DataRow(11, "Simple Model", "11|Simple Model", "Id,Name")]
        [DataRow(11, "Simple Model", "Simple Model|11", "Name,Id")]
        public void ObjectExtensions_GetDisplayName_Test(int id, string name, string expected, string property)
        {
            // Arrange
            var testObject = new object[] { new SimpleModel { Id = id, Name = name } };

            // Act
            var actual = testObject.GetDisplayName(property);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(11, "Simple Model", "11", "Id")]
        [DataRow(11, "Simple Model", "Simple Model", "Name")]
        public void ObjectExtensions_GetPropertyValue_Test(int id, string name, string expected, string property)
        {
            // Arrange
            var testObject = new object[] { new SimpleModel { Id = id, Name = name } };

            // Act
            var actual = testObject.GetPropertyValue(property);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ObjectExtensions_GetPropertyValue_NullPropertyValue_ReturnsNull()
        {
            // Arrange
            var testObject = new object[] { new SimpleModel { Id = 11, Name = null } };

            // Act
            var actual = testObject.GetPropertyValue("Name");

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ObjectExtensions_GetPropertyValue_NonExistentProperty_ReturnsNull()
        {
            // Arrange
            var testObject = new object[] { new SimpleModel { Id = 11, Name = "Test" } };

            // Act
            var actual = testObject.GetPropertyValue("NonExistentProperty");

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ObjectExtensions_GetPropertyValue_NullData_ReturnsNull()
        {
            // Arrange
            object[] testObject = null;

            // Act
            var actual = testObject.GetPropertyValue("Name");

            // Assert
            Assert.IsNull(actual);
        }
    }
}

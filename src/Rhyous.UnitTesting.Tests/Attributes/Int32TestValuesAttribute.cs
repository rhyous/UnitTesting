using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class Int32TestValuesAttributeTests
    {
        [TestMethod]
        public void Int32TestValuesAttribute_GetData_Default_Test()
        {
            // Arrange
            var int32TestValuesAttribute = new Int32TestValuesAttribute();
            MethodInfo methodInfo = null;
            var expectedArray = Int32TestValuesAttribute.CommonIntValues;

            // Act
            var result = int32TestValuesAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.AreEqual(9, result.Count);
            int i = 0;
            foreach (var d in result)
                Assert.AreEqual(expectedArray[i++], d[0]);
        }

        [TestMethod]
        public void Int32TestValuesAttribute_GetData_IntArray_Test()
        {
            // Arrange
            var ints = new int[] { 1, 2, 3, 4, 5 };
            var int32TestValuesAttribute = new Int32TestValuesAttribute(ints); // as int[] array
            MethodInfo methodInfo = null;
            var expectedArray = Int32TestValuesAttribute.CommonIntValues.Concat(ints).Distinct().ToArray();

            // Act
            var result = int32TestValuesAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.AreEqual(13, result.Count);
            int i = 0;
            foreach (var d in result)
                Assert.AreEqual(expectedArray[i++], d[0]);
        }

        [TestMethod]
        public void Int32TestValuesAttribute_GetData_Params_Test()
        {
            // Arrange
            var int32TestValuesAttribute = new Int32TestValuesAttribute(1, 2, 3, 4, 5); // As params
            MethodInfo methodInfo = null;
            var ints = new int[] { 1, 2, 3, 4, 5 };
            var expectedArray = Int32TestValuesAttribute.CommonIntValues.Concat(ints).Distinct().ToArray();

            // Act
            var result = int32TestValuesAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.AreEqual(13, result.Count);
            int i = 0;
            foreach (var d in result)
                Assert.AreEqual(expectedArray[i++], d[0]);
        }

        [TestMethod]
        public void Int32TestValuesAttribute_GetDisplayName_Test()
        {
            // Arrange
            var ints = new int[] { 1, 2, 3, 4, 5 };
            var int32TestValuesAttribute = new Int32TestValuesAttribute(ints);
            MethodInfo methodInfo = null;
            var data = int32TestValuesAttribute.GetData(methodInfo);

            foreach (var d in data)
            {
                // Act
                var result = int32TestValuesAttribute.GetDisplayName(methodInfo, d);

                // Assert
                Assert.AreEqual(d[0].ToString(), result);
            }
        }
    }
}
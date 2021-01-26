using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class PrimitiveListAttributeTests
    {
        [TestMethod]
        public void PrimitiveListAttribute_GetData_ObjArray_Test()
        {
            // Arrange
            var ints = new object[] { 1, 2, 3, 4, 5 };
            var primitiveListAttribute = new PrimitiveListAttribute(ints);
            MethodInfo methodInfo = null;

            // Act
            var result = primitiveListAttribute.GetData(methodInfo);

            // Assert
            int i = 0;
            foreach (var d in result)
                Assert.AreEqual(ints[i++], d[0]);
        }

        [TestMethod]
        public void PrimitiveListAttribute_GetData_Params_Test()
        {
            // Arrange
            var primitiveListAttribute = new PrimitiveListAttribute(1, 2, 3, 4, 5);
            MethodInfo methodInfo = null;

            // Act
            var result = primitiveListAttribute.GetData(methodInfo);

            // Assert
            int i = 0;
            foreach (var d in result)
                Assert.AreEqual(++i, d[0]);
        }

        [TestMethod]
        public void PrimitiveListAttribute_GetDisplayName_Test()
        {
            // Arrange
            var ints = new object[] { 1, 2, 3, 4, 5 };
            var primitiveListAttribute = new PrimitiveListAttribute(ints);
            MethodInfo methodInfo = null;
            var data = primitiveListAttribute.GetData(methodInfo);

            foreach (var d in data)
            {
                // Act
                var result = primitiveListAttribute.GetDisplayName(methodInfo, d);

                // Assert
                Assert.AreEqual(d[0].ToString(), result);
            }
        }
    }
}

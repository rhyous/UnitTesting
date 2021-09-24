using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests.JsonTestDataSource
{ 
    public class TestDataModel : ITestName
    {
        public string TestName { get; set; }
    }

    public class TestDataModelNoName
    {
        public string SomeTestValue { get; set; }
    }

    [TestClass]
    public class JsonTestDataSourceAttributeTests
    {
        #region GetData
        [TestMethod]
        public void JsonTestDataSourceAttribute_GetData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var jsonTestDataSourceAttribute = new JsonTestDataSourceAttribute(typeof(List<TestDataModel>), @"c:\fake\file");
            var funcWasCalled = false;
            Func<string, string> func = (string input) => 
            {
                funcWasCalled = true;
                return "[{ \"TestName\":\"SomeName1\"},{ \"TestName\":\"SomeName2\"}]"; 
            };
            jsonTestDataSourceAttribute.FileReadAllTextMethod = func;
            MethodInfo methodInfo = null;

            // Act
            var result = jsonTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.IsTrue(funcWasCalled);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("SomeName1", (result[0][0] as TestDataModel).TestName);
            Assert.AreEqual("SomeName2", (result[1][0] as TestDataModel).TestName);
        }
        #endregion

        #region GetDisplayName
        [TestMethod]
        public void JsonTestDataSourceAttribute_GetDisplayName_ITestNameImplemented_Test()
        {
            // Arrange
            var jsonTestDataSourceAttribute = new JsonTestDataSourceAttribute(typeof(List<TestDataModel>), @"c:\fake\file");
            MethodInfo methodInfo = null;
            var testName = "Test A";
            object[] data = new[] { new TestDataModel { TestName = testName } };

            // Act
            var result = jsonTestDataSourceAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual(testName, result);
        }


        [TestMethod]
        public void JsonTestDataSourceAttribute_GetDisplayName_ITestNameNotImplemented_Test()
        {
            // Arrange
            var jsonTestDataSourceAttribute = new JsonTestDataSourceAttribute(typeof(TestDataModelNoName), @"c:\fake\file");
            MethodInfo methodInfo = null;
            var testName = "Test A";
            object[] data = new[] { new TestDataModelNoName {  } };

            // Act
            var result = jsonTestDataSourceAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.IsNull(result);
        }
        #endregion
    }
}

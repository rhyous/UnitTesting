using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Collections;
using Rhyous.UnitTesting.Tests.TestModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests
{
    [TestClass]
    public class JsonTestDataSourceAttributeTests
    {
        #region GetData
        [TestMethod]
        public void JsonTestDataSourceAttribute_GetData_FileHas3RowsOfData()
        {
            // Arrange
            var jsonTestDataSourceAttribute = new JsonTestDataSourceAttribute(typeof(List<TestDataModel>), @"c:\fake\file");
            var funcWasCalled = false;
            jsonTestDataSourceAttribute.FileExists = (string input) => { return true; };
            jsonTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            jsonTestDataSourceAttribute.FileReadAllTextMethod = (string input) =>
            {
                funcWasCalled = true;
                return "[{ \"TestName\":\"SomeName1\"},{ \"TestName\":\"SomeName2\"},{ \"TestName\":\"AnotherName3\"}]";
            };
            MethodInfo methodInfo = null;

            // Act
            var result = jsonTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.IsTrue(funcWasCalled);
            Assert.HasCount(3, result);
            Assert.AreEqual("SomeName1", (result[0][0] as TestDataModel).TestName);
            Assert.AreEqual("SomeName2", (result[1][0] as TestDataModel).TestName);
            Assert.AreEqual("AnotherName3", (result[2][0] as TestDataModel).TestName);
        }

        [TestMethod]
        public void JsonTestDataSourceAttribute_GetData_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange
            var jsonTestDataSourceAttribute = new JsonTestDataSourceAttribute(typeof(List<TestDataModel>), @"c:\nonexistent\file.json");
            jsonTestDataSourceAttribute.FileExists = (string input) => { return false; };
            jsonTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            MethodInfo methodInfo = null;

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
            {
                var result = jsonTestDataSourceAttribute.GetData(methodInfo).ToList();
            });
        }

        [TestMethod]
        public void JsonTestDataSourceAttribute_GetData_FileNotFoundInCurrentDirectory_FindsInFullPath()
        {
            // Arrange
            var jsonTestDataSourceAttribute = new JsonTestDataSourceAttribute(typeof(List<TestDataModel>), @"relative\file.json");
            var fileExistsCallCount = 0;
            jsonTestDataSourceAttribute.FileExists = (string input) =>
            {
                fileExistsCallCount++;
                // First call: relative path doesn't exist
                // Second call: full path exists
                return input.Contains(@"c:\FakeRootFolder");
            };
            jsonTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            jsonTestDataSourceAttribute.FileReadAllTextMethod = (string input) =>
            {
                return "[{ \"TestName\":\"SomeName1\"}]";
            };
            MethodInfo methodInfo = null;

            // Act
            var result = jsonTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.AreEqual(2, fileExistsCallCount); // Called twice: relative path, then full path
            Assert.HasCount(1, result);
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
            object[] data = [new TestDataModel { TestName = testName }];

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
            object[] data = [new TestDataModelNoName { }];

            // Act
            var result = jsonTestDataSourceAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void JsonTestDataSourceAttribute_GetDisplayName_PropertyNameProvided_ReturnsPropertyValue()
        {
            // Arrange
            var jsonTestDataSourceAttribute = new JsonTestDataSourceAttribute(typeof(List<TestDataModel>), @"c:\fake\file", "SomeTestValue");
            MethodInfo methodInfo = null;
            object[] data = [new TestDataModelNoName { SomeTestValue = "Custom Value" }];

            // Act
            var result = jsonTestDataSourceAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("Custom Value", result);
        }

        [TestMethod]
        public void JsonTestDataSourceAttribute_GetDisplayName_MultiplePropertiesProvided_ReturnsPipeSeparatedValues()
        {
            // Arrange
            var jsonTestDataSourceAttribute = new JsonTestDataSourceAttribute(typeof(List<TestDataModel>), @"c:\fake\file", "Id,TestName");
            MethodInfo methodInfo = null;
            var testModel = new TestDataModelWithId { Id = 42, TestName = "Test 42" };
            object[] data = [testModel];

            // Act
            var result = jsonTestDataSourceAttribute.GetDisplayName(methodInfo, data);

            // Assert
            Assert.AreEqual("42|Test 42", result);
        }
        #endregion
    }

    public class TestDataModelWithId : ITestName
    {
        public int Id { get; set; }
        public string TestName { get; set; }
    }
}

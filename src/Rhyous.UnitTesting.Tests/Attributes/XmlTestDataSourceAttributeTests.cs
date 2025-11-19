using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests.Attributes
{
    [TestClass]
    public class XmlTestDataSourceAttributeTests
    {
        [TestMethod]
        public void XmlTestDataSourceAttribute_GetData_XmlHasTwoTstRows()
        {
            // Arrange
            var xmlTestDataSourceAttribute = new XmlTestDataSourceAttribute(typeof(List<SimpleModel>), @"fakedir\file.txt", null);
            MethodInfo methodInfo = null;
            xmlTestDataSourceAttribute.FileExists = (string file) => { return true; };
            xmlTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            var simpleModel11 = new SimpleModel { Id = 11, Name = "Model 11" };
            var simpleModel12 = new SimpleModel { Id = 12, Name = "Model 12" };
            xmlTestDataSourceAttribute.DeserializeFromXml = (string xml, Type type) =>
            {
                return new List<SimpleModel>
                {
                    simpleModel11,
                    simpleModel12
                };
            };

            // Act
            var result = xmlTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Assert
            Assert.AreEqual(simpleModel11, result[0][0]);
            Assert.AreEqual(simpleModel12, result[1][0]);
        }

        [TestMethod]
        public void XmlTestDataSourceAttribute_GetDisplayName_PropertyProvided_ReturnsValueForThatProperty()
        {
            // Arrange
            var xmlTestDataSourceAttribute = new XmlTestDataSourceAttribute(typeof(List<SimpleModel>), @"fakedir\file.txt", nameof(SimpleModel.Name));
            MethodInfo methodInfo = null;
            xmlTestDataSourceAttribute.FileExists = (string file) => { return true; };
            xmlTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            var simpleModel11 = new SimpleModel { Id = 11, Name = "Model 11" };
            var simpleModel12 = new SimpleModel { Id = 12, Name = "Model 12" };
            xmlTestDataSourceAttribute.DeserializeFromXml = (string xml, Type type) =>
            {
                return new List<SimpleModel>
                {
                    simpleModel11,
                    simpleModel12
                };
            };
            var data = xmlTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Act
            var result1 = xmlTestDataSourceAttribute.GetDisplayName(methodInfo, data[0]);
            var result2 = xmlTestDataSourceAttribute.GetDisplayName(methodInfo, data[1]);

            // Assert
            Assert.AreEqual(simpleModel11.Name, result1);
            Assert.AreEqual(simpleModel12.Name, result2);
        }

        [TestMethod]
        public void XmlTestDataSourceAttribute_GetDisplayName_PropertyNotProvided_HasTestNameProperty_ReturnsValueForThatProperty()
        {
            // Arrange
            var xmlTestDataSourceAttribute = new XmlTestDataSourceAttribute(typeof(List<TestModel>), @"fakedir\file.txt");
            MethodInfo methodInfo = null;
            xmlTestDataSourceAttribute.FileExists = (string file) => { return true; };
            xmlTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            var simpleModel11 = new TestModel { TestName = "Test 11", Id = 11, Name = "Model 11" };
            var simpleModel12 = new TestModel { TestName = "Test 12", Id = 12, Name = "Model 12" };
            xmlTestDataSourceAttribute.DeserializeFromXml = (string xml, Type type) =>
            {
                return new List<TestModel>
                {
                    simpleModel11,
                    simpleModel12
                };
            };
            var data = xmlTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Act
            var result1 = xmlTestDataSourceAttribute.GetDisplayName(methodInfo, data[0]);
            var result2 = xmlTestDataSourceAttribute.GetDisplayName(methodInfo, data[1]);

            // Assert
            Assert.AreEqual(simpleModel11.TestName, result1);
            Assert.AreEqual(simpleModel12.TestName, result2);
        }
    }
}

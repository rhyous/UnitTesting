using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EasyCsv;
using Rhyous.EasyCsv.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting.Tests.Attributes
{
    [TestClass]
    public class CsvTestDataSourceAttributeTests
    {
        [TestMethod]
        public void CsvTestDataSource_GetData_ValidCsvWith2Rows_Returns2Tests()
        {
            // Arrange
            var csvTestDataSourceAttribute = new CsvTestDataSourceAttribute(@"fakedir\file.csv", null);
            MethodInfo methodInfo = null;
            csvTestDataSourceAttribute.FileExists = (string file) => { return true; };
            csvTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            var row1 = new List<string> { "Test1", "Value1" };
            var row2 = new List<string> { "Test2", "Value2" };
            csvTestDataSourceAttribute.CreateCsv = (string file) =>
            {
                var csv = new Csv();
                csv.Headers.Add("TestName");
                csv.Headers.Add("Value");
                csv.Rows.Add(row1);
                csv.Rows.Add(row2);
                return csv;
            };

            // Act
            var result = csvTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Assert
            CollectionAssert.AreEqual(row1, result[0][0] as Row<string>);
            CollectionAssert.AreEqual(row2, result[1][0] as Row<string>);
        }

        [TestMethod]
        public void CsvTestDataSource_GetDisplayName_NullProperty_TestNameColumnExists_ReturnsTestNameColumnValue()
        {
            // Arrange
            var csvTestDataSourceAttribute = new CsvTestDataSourceAttribute(@"fakedir\file.csv", null);
            MethodInfo methodInfo = null;
            csvTestDataSourceAttribute.FileExists = (string file) => { return true; };
            csvTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            var row1 = new List<string> { "Test1", "Value1" };
            var row2 = new List<string> { "Test2", "Value2" };
            csvTestDataSourceAttribute.CreateCsv = (string file) =>
            {
                var csv = new Csv(true);
                csv.Headers.Add("TestName");
                csv.Headers.Add("Value");
                csv.Rows.Add(row1);
                csv.Rows.Add(row2);
                return csv;
            };
            var result = csvTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Act
            var firstRowName = csvTestDataSourceAttribute.GetDisplayName(methodInfo, result[0]);
            var secondRowName = csvTestDataSourceAttribute.GetDisplayName(methodInfo, result[1]);

            // Assert
            Assert.AreEqual(row1[0], firstRowName);
            Assert.AreEqual(row2[0], secondRowName);
        }


        [TestMethod]
        public void CsvTestDataSource_GetDisplayName_TestNameColumnDefined_TestNameColumnExists_ReturnsTestNameColumnValue()
        {
            // Arrange
            var csvTestDataSourceAttribute = new CsvTestDataSourceAttribute(@"fakedir\file.csv", "MyTestNameColumn");
            MethodInfo methodInfo = null;
            csvTestDataSourceAttribute.FileExists = (string file) => { return true; };
            csvTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            var row1 = new List<string> { "Test1", "Value1" };
            var row2 = new List<string> { "Test2", "Value2" };
            csvTestDataSourceAttribute.CreateCsv = (string file) =>
            {
                var csv = new Csv(true);
                csv.Headers.Add("MyTestNameColumn");
                csv.Headers.Add("Value");
                csv.Rows.Add(row1);
                csv.Rows.Add(row2);
                return csv;
            };
            var result = csvTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Act
            var firstRowName = csvTestDataSourceAttribute.GetDisplayName(methodInfo, result[0]);
            var secondRowName = csvTestDataSourceAttribute.GetDisplayName(methodInfo, result[1]);

            // Assert
            Assert.AreEqual(row1[0], firstRowName);
            Assert.AreEqual(row2[0], secondRowName);
        }


        [TestMethod]
        public void CsvTestDataSource_GetDisplayName_TestNameColumnDefined_TestNameColumnDoesNotExists_ReturnsTestNameColumnValue()
        {
            // Arrange
            var csvTestDataSourceAttribute = new CsvTestDataSourceAttribute(@"fakedir\file.csv", "MyTestNameColumn");
            MethodInfo methodInfo = null;
            csvTestDataSourceAttribute.FileExists = (string file) => { return true; };
            csvTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            var row1 = new List<string> { "Test1", "Value1" };
            var row2 = new List<string> { "Test2", "Value2" };
            csvTestDataSourceAttribute.CreateCsv = (string file) =>
            {
                var csv = new Csv(true);
                csv.Headers.Add("SomeOtherColumnName");
                csv.Headers.Add("Value");
                csv.Rows.Add(row1);
                csv.Rows.Add(row2);
                return csv;
            };
            var result = csvTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Act
            var firstRowName = csvTestDataSourceAttribute.GetDisplayName(methodInfo, result[0]);
            var secondRowName = csvTestDataSourceAttribute.GetDisplayName(methodInfo, result[1]);

            // Assert
            Assert.AreEqual($"1|{row1[0]}", firstRowName);
            Assert.AreEqual($"2|{row2[0]}", secondRowName);
        }

        [TestMethod]
        public void CsvTestDataSource_GetDisplayName_TwoTestNameColumnsDefined_ReturnsTestNameColumnsValuesPipeSeparated()
        {
            // Arrange
            var csvTestDataSourceAttribute = new CsvTestDataSourceAttribute(@"fakedir\file.csv", "MyTestNameColumn1,MyTestNameColumn2");
            MethodInfo methodInfo = null;
            csvTestDataSourceAttribute.FileExists = (string file) => { return true; };
            csvTestDataSourceAttribute.GetCurrentDirectory = () => { return @"c:\FakeRootFolder"; };
            var row1 = new List<string> { "Test1", "A1", "Value1" };
            var row2 = new List<string> { "Test2", "B2", "Value2" };
            csvTestDataSourceAttribute.CreateCsv = (string file) =>
            {
                var csv = new Csv(true);
                csv.Headers.Add("MyTestNameColumn1");
                csv.Headers.Add("MyTestNameColumn2");
                csv.Headers.Add("Value");
                csv.Rows.Add(row1);
                csv.Rows.Add(row2);
                return csv;
            };
            var result = csvTestDataSourceAttribute.GetData(methodInfo).ToList();

            // Act
            var firstRowName = csvTestDataSourceAttribute.GetDisplayName(methodInfo, result[0]);
            var secondRowName = csvTestDataSourceAttribute.GetDisplayName(methodInfo, result[1]);

            // Assert
            Assert.AreEqual($"{row1[0]}|{row1[1]}", firstRowName);
            Assert.AreEqual($"{row2[0]}|{row2[1]}", secondRowName);
        }
    }
}

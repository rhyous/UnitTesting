using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Rhyous.UnitTesting
{
    /// <summary>
    /// An attribute used to decorate Unit Test methods to provide a list of test data and
    /// run the test for each test data in the list.
    /// </summary>
    public class JsonTestDataSourceAttribute : Attribute, ITestDataSource
    {
        private readonly Type _Type;
        private readonly string _File;
        private readonly string _PropertyName;
        private int _RowTestId = 0;

        /// <summary>A Func which allows for mocking File.Exists in Unit Tests</summary>
        internal Func<string, bool> FileExists = File.Exists;
        /// <summary>A Func which allows for mocking Directory.GetCurrentDirectory in Unit Tests</summary>
        internal Func<string> GetCurrentDirectory = Directory.GetCurrentDirectory;
        /// <summary>A Func which allows for mocking File.ReadAllText in Unit Tests</summary>
        internal Func<string, string> FileReadAllTextMethod = File.ReadAllText;

        /// <summary>The Attribute constructor</summary>
        /// <param name="type">The type</param>
        /// <param name="file">The file path</param>
        /// <param name="testNameProperty">The property name to use as the test display name. Supports multiple properties, comma separated.</param>
        public JsonTestDataSourceAttribute(Type type, string file, string testNameProperty = null)
        {
            _Type = type;
            _File = file;
            _PropertyName = testNameProperty;
        }

        /// <summary>Gets the data. This is called by test methods.</summary>
        /// <param name="methodInfo">The test method passed in by test.</param>
        /// <returns></returns>
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var file = _File;
            if (!FileExists(file))
            {
                // Try full path
                file = Path.Combine(GetCurrentDirectory(), file);
                if (!FileExists(file))
                    throw new FileNotFoundException($"Could not find test data file. Searched:{Environment.NewLine}{_File}{Environment.NewLine}{file}");
            }
            var json = FileReadAllTextMethod(file);
            var testDataSet = JsonConvert.DeserializeObject(json, _Type);
            IEnumerable rows;
            rows = (testDataSet is IEnumerable<ITestRunOrder> orderedRows)
                 ? orderedRows.OrderBy(o => o.RunOrder)
                 : testDataSet as IEnumerable;

            foreach (var row in rows)
                yield return new object[] { row };
        }

        /// <summary>
        /// Returns the name of the test.
        /// </summary>
        /// <param name="methodInfo">The test method</param>
        /// <param name="data">The data passed into the test.</param>
        /// <returns>The name of the test being run.</returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            var name = data.GetDisplayName(_PropertyName);
            return string.IsNullOrWhiteSpace(name)
                 ? $"{++_RowTestId}"
                 : name;
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
        /// <summary>A Func which allows for mocking File.ReadAllText in Unit Tests</summary>
        internal Func<string, string> FileReadAllTextMethod = File.ReadAllText;

        /// <summary>The Attribute constructor</summary>
        /// <param name="type">The type</param>
        /// <param name="file">The file path</param>
        public JsonTestDataSourceAttribute(Type type, string file)
        {
            _Type = type;
            _File = file;
        }

        /// <summary>Gets the data. This is called by test methods.</summary>
        /// <param name="methodInfo">The test method passed in by test.</param>
        /// <returns></returns>
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var json = FileReadAllTextMethod(_File);
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
        /// If your data model implements both ITestRunOrder and ITestName, the format is "{RunOrder}:{TestName}".
        /// If you data model implements only ITestRunOrder, the name is the format is "{RunOrder}". 
        /// If you data model implements only ITestName, the name is the format is "{TestName}".
        /// If neither are implemented, the test name returns null.
        /// </summary>
        /// <param name="methodInfo">The test method</param>
        /// <param name="data">The data passed into the test.</param>
        /// <returns>The name of the test being run.</returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            string name = null;
            if (data[0] is ITestRunOrder testOrder)
                name += $"{testOrder.RunOrder}";
            if (data[0] is ITestName testName)
                name += string.IsNullOrWhiteSpace(name) ? testName.TestName : $":{testName.TestName}";
            return name;
        }
    }
}
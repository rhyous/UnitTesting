using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        /// <summary>
        /// A Func which allows for mocking File.ReadAllText in Unit Tests
        /// </summary>
        internal Func<string, string> FileReadAllTextMethod = File.ReadAllText;

        /// <summary>
        /// The Attribute constructor
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="file">The file path</param>
        public JsonTestDataSourceAttribute(Type type, string file)
        {
            _Type = type;
            _File = file;
        }

        /// <summary>
        /// Gets the data. This is called by test methods.
        /// </summary>
        /// <param name="methodInfo">The test method passed in by test.</param>
        /// <returns></returns>
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var json = FileReadAllTextMethod(_File);
            var obj = JsonConvert.DeserializeObject(json, _Type);
            var rows = obj as IEnumerable;
            foreach (var row in rows)
                yield return new object[] { row };
        }

        /// <summary>
        /// Returns the name of the test if your data model implements ITestName
        /// </summary>
        /// <param name="methodInfo">The test method</param>
        /// <param name="data">The data passed into the test.</param>
        /// <returns></returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return (data[0] as ITestName)?.TestName;
        }
    }
}
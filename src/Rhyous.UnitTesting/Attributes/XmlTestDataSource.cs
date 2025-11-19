using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Rhyous.UnitTesting
{
    /// <summary>
    /// An attribute used to decorate Unit Test methods to provide a list of test data and
    /// run the test for each test data in the list.
    /// </summary>
    /// <remarks>This was written to replace the old unit test DataSource code. This isn't as straightforward as the CsvTestDataSourceAttribute.
    /// 1. Update the attribute but keep the file path
    /// 2. Create a model and a model list. See the bottom of this unit test for examples: https://github.com/rhyous/StringLibrary/blob/master/src/Rhyous.StringLibrary.Tests/Expression/PropertyNameLambdaExtensionsTests.cs
    /// 3. Update the Xml
    ///    - Delete the schema section of the Xml
    ///    - Remove the x: part at the top
    ///    - Find and replace x: with nothing
    /// 4. Update the test method to take in the model you created.
    /// 5. Remove and replace all the calls to TestContext.
    /// </remarks>
    public class XmlTestDataSourceAttribute : Attribute, ITestDataSource
    {
        private readonly Type _Type;
        private readonly string _File;
        private readonly string _TestNameProperty;
        private int _RowTestId = 0;

        /// <summary>A Func which allows for mocking File.Exists in Unit Tests</summary>
        internal Func<string, bool> FileExists = File.Exists;
        /// <summary>A Func which allows for mocking Directory.GetCurrentDirectory in Unit Tests</summary>
        internal Func<string> GetCurrentDirectory = Directory.GetCurrentDirectory;
        /// <summary>A Func which allows for mocking XML deserialization in Unit Tests</summary>
        internal Func<string, Type, IEnumerable> DeserializeFromXml = Deserialize;

        /// <summary>The Attribute constructor</summary>
        /// <param name="type">The <see cref="IEnumerable"/> type containing the data instances for each unit test.</param>
        /// <param name="file">The file path</param>
        /// <param name="testNameProperty">The property name to use as the test display name. Supports multiple properties, comma separated.</param>
        public XmlTestDataSourceAttribute(Type type, string file, string testNameProperty = null)
        {
            _Type = type;
            _File = file;
            _TestNameProperty = testNameProperty;
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
            if (!typeof(IEnumerable).IsAssignableFrom(_Type))
                throw new ArgumentException($"The type {_Type} must implement IEnumerable.");

            IEnumerable result = DeserializeFromXml(file, _Type);

            foreach (var row in result)
                yield return new object[] { row };
        }

        internal static IEnumerable Deserialize(string file, Type type)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            using (TextReader textReader = new StreamReader(file))
            using (XmlTextReader xmlTextReader = new XmlTextReader(textReader))
            {
                xmlTextReader.Read();
                var result = xmlSerializer.Deserialize(xmlTextReader) as IEnumerable;
                return result;
            }
        }

        /// <summary>Returns the name of the test.</summary>
        /// <param name="methodInfo">The test method</param>
        /// <param name="data">The data passed into the test.</param>
        /// <returns>The name of the test being run.</returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            var name = data.GetDisplayName(_TestNameProperty);
            return string.IsNullOrWhiteSpace(name)
                 ? $"{++_RowTestId}|{data[0]}"
                 : name;
        }
    }
}
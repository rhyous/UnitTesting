using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EasyCsv;
using System;
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
    /// <remarks>This was written to replace the old unit test DataSource code.
    /// 1. Update the attribute but keep the file path.
    /// 2. Update the test method to take in a Row{string} parameter.
    /// 3. Remove and replace all the calls to TestContext.
    /// </remarks>
    public class CsvTestDataSourceAttribute : Attribute, ITestDataSource
    {
        private readonly string _File;
        private readonly string _TestNameColumn;
        private List<int> _HeaderColumnIndexes = new List<int>();
        private int _RowTestId = 0;

        /// <summary>A Func which allows for mocking File.Exists in Unit Tests</summary>
        internal Func<string, bool> FileExists = File.Exists;
        /// <summary>A Func which allows for mocking Directory.GetCurrentDirectory in Unit Tests</summary>
        internal Func<string> GetCurrentDirectory = Directory.GetCurrentDirectory;
        /// <summary>A Func which allows for mocking the creation of the ICsv instance in Unit Tests</summary>
        internal Func<string, ICsv> CreateCsv = (string file) => { return new Csv(file); };

        /// <summary>The Attribute constructor</summary>
        /// <param name="file">The file path</param>
        /// <param name="testNameColumn">The property name to use as the test display name. Supports multiple columns, comma separated.</param>
        public CsvTestDataSourceAttribute(string file, string testNameColumn = null)
        {
            _File = file;
            _TestNameColumn = string.IsNullOrWhiteSpace(testNameColumn) ? nameof(ITestName.TestName) : testNameColumn;
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

            var csv = CreateCsv(file);
            _HeaderColumnIndexes.Clear();
            csv.GetHeaderColumnIndexes(_TestNameColumn, _HeaderColumnIndexes);
            _RowTestId = 0;
            foreach (var row in csv.Rows)
                yield return new object[] { row };
        }

        /// <summary>Returns the name of the test.</summary>
        /// <param name="methodInfo">The test method</param>
        /// <param name="data">The data passed into the test.</param>
        /// <returns>The name of the test being run.</returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            var row = data[0] as Row<string>;
            if (_HeaderColumnIndexes.Any())
                return string.Join("|", _HeaderColumnIndexes.Select(index => row[index]));
            var name = data.GetDisplayName(_TestNameColumn);
            return string.IsNullOrWhiteSpace(name)
                 ? $"{++_RowTestId}|{row[0]}"
                 : name;
        }
    }
}
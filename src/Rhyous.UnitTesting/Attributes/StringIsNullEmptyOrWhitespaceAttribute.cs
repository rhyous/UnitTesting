using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    /// <summary>An attribute to decorate a unit test so it inputs a list of null, empty, or whitespace string values. </summary>
    public class StringIsNullEmptyOrWhitespaceAttribute : Attribute, ITestDataSource
    {
        private readonly Dictionary<string, string> NameMap = new Dictionary<string, string>(3)
        {
            { "null", "Null" },
            { "" , "Empty" },
            { " ", "Whitespace - one space" },
            { "    ", "Whitespace - multiple spaces" },
            { " \t ", "Whitespace - with tab" },
            { " \r\n ", "Whitespace - with carriage return" }
        };

        /// <summary>Returns the data as a list of null, empty, or whitespace string values. One object array per unit test.</summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <returns>The data as a list of null, empty, or whitespace string values</returns>
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            return new[]
            {
                    new string[] { null }, // null
                    new string[] { "" },  // Empty
                    new string[] { " " }, // Whitespace - one space
                    new string[] { "    " }, // Whitespace - multiple spaces
                    new string[] { " \t " }, // Whitespace - with tab
                    new string[] { " \r\n " }, // Whitespace - with carriage return
            };
        }

        /// <summary>Returns the display name for the unit test.</summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <param name="data">The data passed into the Unit Test. One of the GetData arrays.</param>
        /// <returns>The name of the null, empty, or whitespace string values. <see cref="NameMap"/></returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return NameMap[data[0]?.ToString() ?? "null"];
        }
    }
}
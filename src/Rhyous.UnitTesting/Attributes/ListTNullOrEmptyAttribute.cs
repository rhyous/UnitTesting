using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    /// <summary>An attribute to decorate a unit test so it inputs null and an empty list.</summary>
    /// <remarks>Can be used for IEnumerable or IEnumerable{T} tests.</remarks>
    public class ListTNullOrEmptyAttribute : Attribute, ITestDataSource
    {
        /// <summary>The constructor.</summary>
        /// <param name="listType">The type of T in List{T}.</param>
        public ListTNullOrEmptyAttribute(Type listType)
        {
            ListType = listType ?? throw new ArgumentNullException(nameof(listType));
        }

        /// <summary>The type of T in List{T}.</summary>
        public Type ListType { get; }

        /// <summary>Returns the data, one object array per unit test.</summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <returns>A null and empty list.</returns>
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var listType = ListType.IsIList()
                         ? ListType 
                         : typeof(List<>).MakeGenericType(ListType);
            var emptylist = Activator.CreateInstance(listType);
            return new []
            {
                 new object[] { null },       // null
                 new object[] { emptylist },  // Empty
            };
        }

        /// <summary>Returns the display name for the unit test.</summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <param name="data">The data passed into the Unit Test. One of the GetData arrays.</param>
        /// <returns>'Null' if null or 'Empty List' if an empty list.</returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return data[0] == null ? "Null" : "Empty List";
        }
    }
}
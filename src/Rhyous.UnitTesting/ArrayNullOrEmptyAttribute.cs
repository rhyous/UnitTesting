using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    /// <summary>
    /// An attribute to decorate a unit test so it inputs null and an empty array 
    /// </summary>
    public class ArrayNullOrEmptyAttribute : Attribute, ITestDataSource
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="arrayType">The array type. If you put in a type that isn't an array, such as int,
        /// it will change it to an int[] type for you.</param>
        public ArrayNullOrEmptyAttribute(Type arrayType)
        {
            if (arrayType is null)
                throw new ArgumentNullException(nameof(arrayType));

            ArrayType = arrayType.IsArray
                      ? arrayType
                      : arrayType.MakeArrayType();
        }

        /// <summary>
        /// The array type.
        /// </summary>
        public Type ArrayType { get; }

        /// <summary>
        /// Returns the data, one object array per unit test.
        /// </summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <returns>A null and empty array.</returns>
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var emptyArray = Activator.CreateInstance(ArrayType, 0);
            return new[]
            {
                 new object[] { null },       // null
                 new object[] { emptyArray },  // Empty
            };
        }

        /// <summary>
        /// Returns the display name for the unit test.
        /// </summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <param name="data">The data passed into the Unit Test. One of the GetData arrays.</param>
        /// <returns>'Null' if null or 'Empty Array' if an empty array.</returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return data[0] == null ? "Null" : "Empty Array";
        }
    }
}
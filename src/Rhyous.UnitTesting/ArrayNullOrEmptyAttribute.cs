using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    public class ArrayNullOrEmptyAttribute : Attribute, ITestDataSource
    {
        public ArrayNullOrEmptyAttribute(Type arrayType)
        {
            ArrayType = arrayType;
        }

        public Type ArrayType { get; }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var emptyArray = Activator.CreateInstance(ArrayType, 0);
            return new[]
            {
                 new object[] { null, "Null" },       // null
                 new object[] { emptyArray, "Empty"},  // Empty
            };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return (string)data[1];
        }
    }
}
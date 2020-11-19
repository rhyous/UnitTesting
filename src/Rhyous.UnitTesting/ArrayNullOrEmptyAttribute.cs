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
            if (arrayType is null)
                throw new ArgumentNullException(nameof(arrayType));

            if (!arrayType.IsArray)
                throw new ArgumentException("The type must be an array", nameof(arrayType));
            ArrayType = arrayType;
        }

        public Type ArrayType { get; }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var emptyArray = Activator.CreateInstance(ArrayType, 0);
            return new[]
            {
                 new object[] { null },       // null
                 new object[] { emptyArray },  // Empty
            };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return data[0] == null ? "Null" : "Empty";
        }
    }
}
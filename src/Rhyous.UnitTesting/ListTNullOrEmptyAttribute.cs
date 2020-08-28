using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    public class ListTNullOrEmptyAttribute : Attribute, ITestDataSource
    {
        public ListTNullOrEmptyAttribute(Type listType)
        {
            ListType = listType;
        }

        public Type ListType { get; }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var listType = typeof(List<>).MakeGenericType(ListType);
            var emptylist = Activator.CreateInstance(listType);
            return new []
            {
                 new object[] { null, "Null" },       // null
                 new object[] { emptylist, "Empty"},  // Empty
            };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return (string)data[1];
        }
    }
}
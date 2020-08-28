using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    public class PrimitiveListAttribute : Attribute, ITestDataSource
    {
        public PrimitiveListAttribute(params object[] primitives)
        {
            Primitives = primitives;
        }

        public object[] Primitives { get; }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            return Primitives.Select(p => new object[] { p }).ToArray();
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return data[0]?.ToString();
        }
    }
}
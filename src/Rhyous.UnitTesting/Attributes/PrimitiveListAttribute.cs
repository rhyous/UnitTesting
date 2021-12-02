using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    /// <summary>An attribute to decorate a unit test so it inputs any of the input primitives </summary>
    public class PrimitiveListAttribute : Attribute, ITestDataSource
    {
        /// <summary>The constructor</summary>
        /// <param name="primitives">Primitives that will be input separately into unit tests.</param>
        public PrimitiveListAttribute(params object[] primitives)
        {
            Primitives = primitives;
        }

        /// <summary>An array of the primitives.</summary>
        public object[] Primitives { get; protected set; }

        /// <summary>Returns the data, one object array per unit test.</summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <returns>This primitives.</returns>
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            return Primitives.Select(p => new object[] { p });
        }

        /// <summary>Returns the display name for the unit test.</summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <param name="data">The data passed into the Unit Test. One of the GetData arrays.</param>
        /// <returns>A string created by calling the ToString() on the primitive.</returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return data[0]?.ToString();
        }
    }
}
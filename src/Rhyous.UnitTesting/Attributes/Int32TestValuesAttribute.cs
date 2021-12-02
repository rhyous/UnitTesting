using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    /// <summary>An attribute to decorate a unit test so it inputs important Int32 values to test.</summary>
    public class Int32TestValuesAttribute : Attribute, ITestDataSource
    {
        /// <summary>
        /// A set of common int values. See table below:
        /// 0 = Zero
        /// 1 = a positive number
        /// 227 = a positive prime number much greater than 1
        /// 2468 = An positive even number much greater than 1
        /// int.MaxValue = the largest positive number an int can be
        /// -1 = a negative number
        /// -227 = a negative prime number less 
        /// -2468 = An negative even number much less than -1
        /// int.MinValue = the smallest negative number an int can be
        /// </summary>
        public static int[] CommonIntValues => new[] { 0, 1, 227, 2468, int.MaxValue, -1, -227, -2468, int.MinValue };

        /// <summary>The constructor</summary>
        /// <param name="additionalInts">Any additional integers to test. Only distinct integers will be tested.</param>
        public Int32TestValuesAttribute(params int[] additionalInts)
        {
            Primitives = (additionalInts == null && !additionalInts.Any())
                       ? CommonIntValues
                       : CommonIntValues.Concat(additionalInts).Distinct().ToArray();
        }

        /// <summary>An array ints.</summary>
        public int[] Primitives { get; protected set; }

        /// <summary>Returns the data, one Int32 per unit test.</summary>
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
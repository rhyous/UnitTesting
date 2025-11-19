using System;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    internal static class ObjectExtensions
    {
        internal static string GetDisplayName(this object[] data)
        {
            string name = null;
            if (data[0] is ITestRunOrder testOrder)
                name += $"{testOrder.RunOrder}";
            if (data[0] is ITestName testName)
                name += string.IsNullOrWhiteSpace(name) ? testName.TestName : $":{testName.TestName}";
            return name;
        }

        /// <summary>Returns the name of the test.</summary>
        /// <param name="data">The data passed into the test.</param>
        /// <param name="property">The property name to use as the test display name. Supports multiple properties, comma separated.</param>
        /// <returns>The name of the test being run.</returns>
        internal static string GetDisplayName(this object[] data, string property)
        {
            if (data == null || !data.Any())
                return null;
            if (string.IsNullOrWhiteSpace(property))
            {
                var testName = data.GetPropertyValue(nameof(ITestName.TestName));
                return string.IsNullOrWhiteSpace(testName) ? null : testName;
            }

            if (property.Contains(","))
            {
                var values = property.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(p => data.GetPropertyValue(p))
                                     .Where(v => !string.IsNullOrEmpty(v));
                return string.Join("|", values);
            }
            else
            {
                return data.GetPropertyValue(property);
            }
        }

        internal static string GetPropertyValue(this object[] data, string property)
        {
            if (data == null || !data.Any() || string.IsNullOrWhiteSpace(property))
                return null;
            var propInfo = data[0].GetType().GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (propInfo is null)
                return null;
            var value = propInfo.GetValue(data[0], null);
            return value?.ToString();
        }
    }
}

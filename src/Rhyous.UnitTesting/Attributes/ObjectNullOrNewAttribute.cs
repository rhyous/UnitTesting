using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    /// <summary>An attribute to decorate a unit test so it inputs null and an empty object</summary>
    public class ObjectNullOrNewAttribute : Attribute, ITestDataSource
    {
        private readonly Type _ObjType;
        private readonly object[] _ConstructorParams;

        /// <summary>The constructor.</summary>
        /// <param name="objType">The type.</param>
        /// <param name="constructorParams">The constructor parameters.</param>
        /// <remarks>Usually only constant constructor parameters are allowed as you can only put constants in an Attribute.</remarks>
        public ObjectNullOrNewAttribute(Type objType, params object[] constructorParams)
        {
            if (objType is null)
            {
                throw new ArgumentNullException(nameof(objType));
            }
            if (!CanBeAssignedNull(objType))
            {
                throw new InvalidOperationException($"The provided type '{nameof(objType)}' cannot be null.");
            }

            _ObjType = objType;
            _ConstructorParams = constructorParams;
        }

        /// <summary>Returns the data, one object array per unit test.</summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <returns>A null and new object.</returns>
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var createdObject = _ConstructorParams == null || !_ConstructorParams.Any()
                           ? Activator.CreateInstance(_ObjType)
                           : Activator.CreateInstance(_ObjType, _ConstructorParams);
            return new[]
            {
                 new object[] { null },           // null
                 new object[] { createdObject },  // New
            };
        }

        /// <summary>Returns the display name for the unit test.</summary>
        /// <param name="methodInfo">The test method. This is unused.</param>
        /// <param name="data">The data passed into the Unit Test. One of the GetData f.</param>
        /// <returns>'Null' if null or 'new' if an instantiated object.</returns>
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            var typeName = IsNullableType(_ObjType) ? $"{Nullable.GetUnderlyingType(_ObjType).Name}?" : _ObjType.Name;
            return data[0] == null ? $"Null {typeName}" : $"New {typeName}";
        }

        private static bool CanBeAssignedNull(Type objType)
        {
            return !objType.IsValueType || IsNullableType(objType);
        }

        private static bool IsNullableType(Type objType)
        {
            return Nullable.GetUnderlyingType(objType) != null;
        }
    }
}
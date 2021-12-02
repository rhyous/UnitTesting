using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.UnitTesting
{
    /// <summary>Extensions for Type.</summary>
    public static class TypeExtensions
    {
        /// <summary>Chekcs if a Type is a type that implements IList</summary>
        /// <param name="t">The type</param>
        /// <returns>Try if the type inherits IList false otherwise.</returns>
        public static bool IsIList(this Type t)
        {
            return typeof(IList).IsAssignableFrom(t) 
                || t.GetInterfaces().Any(i => i.IsGenericIList());
        }

        /// <summary>Chekcs if a Type is a type that implements IList{}</summary>
        /// <param name="t">The type</param>
        /// <returns>Try if the type inherits IList{} false otherwise.</returns>
        public static bool IsGenericIList(this Type t)
        {
            return t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IList<>));
        }
    }
}
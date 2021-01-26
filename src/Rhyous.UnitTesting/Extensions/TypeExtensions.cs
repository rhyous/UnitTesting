using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.UnitTesting
{
    public static class TypeExtensions
    {
        public static bool InheritsIList(this Type t)
        {
            return typeof(IList).IsAssignableFrom(t) 
                || t.GetInterfaces().Any(i => i.IsGenericIList());
        }

        public static bool IsGenericIList(this Type t)
        {
            return t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IList<>));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Configgy
{
    internal static class TypeExtensions
    {
        public static IList<PropertyInfo> GetProperties(this Type type, bool requireWrite)
        {
            // get all the public readable properties on this configuration type
            return type
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .OfType<PropertyInfo>()
                .Where(p => p.CanRead)
                .Where(p => !requireWrite || p.CanWrite)
                .ToList();
        }
    }
}
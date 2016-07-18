using System;
using System.Reflection;

namespace Tests
{
    internal static class InternalUtils
    {
        internal static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetTypeInfo().GetProperty(name);
        }

        internal static MethodInfo GetMethod(this Type type, string name)
        {
            return type.GetTypeInfo().GetMethod(name);
        }

        internal static PropertyInfo[] GetProperties(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().GetProperties(flags);
        }

        internal static PropertyInfo[] GetProperties(this Type type)
        {
            return type.GetTypeInfo().GetProperties();
        }

        internal static Type[] GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().GetInterfaces();
        }

        internal static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        internal static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        internal static string ToShortDateString(this DateTime input)
        {
            return input.ToString("d");
        }
    }
}

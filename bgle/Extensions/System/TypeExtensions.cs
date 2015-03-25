using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        
        public static Type[] GetAllInterfaces(this Type type)
        {
            var current = type.GetInterfaces();
            var childs = current.SelectMany(i => i.GetAllInterfaces()).ToList();
            var result = current.Union(childs).Distinct().ToArray();
            return result;
        }

        /// <summary>
        ///     Find all derived types from assembly.
        ///     If assembly is not given, given type assembly is used.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetDerivedTypes(this Type type, Assembly assembly = null)
        {
            return type.GetDerivedTypes(false);
        }

        /// <summary>
        ///     Find all derived types from assembly.
        ///     If assembly is not given, given type assembly is used.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeItself"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetDerivedTypes(this Type type, bool includeItself, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = type.Assembly;
            }

            return assembly.GetTypes().Where(t => (includeItself || t != type) && type.IsAssignableFrom(t)).ToArray();
        }

        public static Type GetEffectiveType(this Type type)
        {
            return !type.IsNullable() ? type : Nullable.GetUnderlyingType(type);
        }

        public static MethodInfo[] GetMethodsExtended(this Type type)
        {
            var current = type.GetMethods();
            var parentMethods = type.GetInterfaces().SelectMany(it => it.GetMethodsExtended()).ToArray();
            var result = current.Union(parentMethods).Distinct().ToArray();
            return result;
        }

        public static PropertyInfo[] GetPropertiesExtended(this Type type)
        {
            var current = type.GetProperties();
            var parentProperties = type.GetInterfaces().SelectMany(it => it.GetPropertiesExtended()).ToArray();
            var result = current.Union(parentProperties).Distinct().ToArray();
            return result;
        }

        public static bool IsNullable(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        ///     Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        ///     Boolean is not considered numeric.
        /// </remarks>
        public static bool IsNumericType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.IsEnum)
            {
                return false;
            }

            switch (Type.GetTypeCode(type.GetEffectiveType()))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }
    }
}
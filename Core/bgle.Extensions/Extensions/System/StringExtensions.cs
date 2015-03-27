using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System
{
    public static class StringExtensions
    {
        public static T ConvertTo<T>(this string s)
        {
            var targetType = typeof(T).GetEffectiveType();

            if (targetType == typeof(string))
            {
                return (T)((object)s);
            }
            if (s.IsNullOrEmpty())
            {
                return default(T);
            }

            if (targetType.IsEnum)
            {
                return (T)s.ToEnum(targetType, default(T));
            }

            return (T)Convert.ChangeType(s, targetType);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static string Join(this IEnumerable<string> s, string separator)
        {
            return string.Join(separator, s);
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            return ToEnum(value, default(T));
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                return defaultValue;
            }
            var names = Enum.GetNames(type);
            if (names.Contains(value))
            {
                return (T)Enum.Parse(type, value, true);
            }
            var res = Enum.GetValues(type).OfType<T>().FirstOrDefault(v => ((int)(object)v).ToString(CultureInfo.InvariantCulture) == value);

            return !Equals(res, default(T)) ? res : defaultValue;
        }

        public static object ToEnum(this string value, Type type, object defaultValue)
        {
            if (!type.IsEnum)
            {
                return defaultValue;
            }
            var names = Enum.GetNames(type);
            if (names.Contains(value))
            {
                return Enum.Parse(type, value, true);
            }
            foreach (var v in Enum.GetValues(type))
            {
                if (((int)v).ToString(CultureInfo.InvariantCulture) == value)
                {
                    return v;
                }
            }

            return defaultValue;
        }
    }
}
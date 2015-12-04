using System;

namespace Omu.ValueInjecter.Utils
{
    public static class StrUtil
    {
        public static string RemovePrefix(string o, string prefix, StringComparison comparison)
        {
            if (prefix == null) return o;
            return !o.StartsWith(prefix, comparison) ? o : o.Remove(0, prefix.Length);
        }

        public static string RemovePrefix(string o, string prefix)
        {
            return RemovePrefix(o, prefix, StringComparison.Ordinal);
        }

        public static string RemoveSuffix(string o, string suffix)
        {
            if (suffix == null) return o;
            return !o.EndsWith(suffix) ? o : o.Remove(o.Length - suffix.Length, suffix.Length);
        }
    }
}
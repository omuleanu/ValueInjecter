using System;

namespace Omu.ValueInjecter.Utils
{
    /// <summary>
    /// String utilities 
    /// </summary>
    public static class StrUtil
    {
        /// <summary>
        /// remove string prefix
        /// </summary>
        /// <param name="o"></param>
        /// <param name="prefix"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static string RemovePrefix(string o, string prefix, StringComparison comparison)
        {
            if (prefix == null) return o;
            return !o.StartsWith(prefix, comparison) ? o : o.Remove(0, prefix.Length);
        }

        /// <summary>
        /// remove string prefix
        /// </summary>
        /// <param name="o"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string RemovePrefix(string o, string prefix)
        {
            return RemovePrefix(o, prefix, StringComparison.Ordinal);
        }

        /// <summary>
        /// remove string suffix
        /// </summary>
        /// <param name="o"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string RemoveSuffix(string o, string suffix)
        {
            if (suffix == null) return o;
            return !o.EndsWith(suffix) ? o : o.Remove(o.Length - suffix.Length, suffix.Length);
        }
    }
}
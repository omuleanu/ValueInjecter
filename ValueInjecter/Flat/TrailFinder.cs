using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Omu.ValueInjecter.Utils;

namespace Omu.ValueInjecter.Flat
{
    /// <summary>
    /// 
    /// </summary>
    public static class TrailFinder
    {
        /// <summary>
        /// Get all possible trails based on the property name
        /// </summary>
        /// <param name="flatPropertyName">flat property name</param>
        /// <param name="lookupProps">properties to look into</param>
        /// <param name="match">match func used for checking the last property in the trail</param>
        /// <param name="comparison">StringComparison type used for building the flat trail</param>
        /// <param name="forFlattening">getting trails for flattening or unflattening, in the first case we need to make sure the properties are readable in the latter writeable</param>
        /// <returns>all possible trails</returns>
        public static IEnumerable<IList<string>> GetTrails(
            string flatPropertyName,
            IEnumerable<PropertyInfo> lookupProps,
            Func<string, PropertyInfo, bool> match,
            StringComparison comparison,
            bool forFlattening = true)
        {
            object lookupObject = null;
            return lookupProps.SelectMany(lookupProp => GetTrailsForProperty(flatPropertyName, lookupObject, lookupProp, match, comparison, forFlattening));
        }

        /// <summary>
        /// Get all possible trails based on the property name
        /// </summary>
        /// <param name="flatPropertyName">flat property name</param>
        /// <param name="lookupObject">object whose properties to look into</param>
        /// <param name="match">match func used for checking the last property in the trail</param>
        /// <param name="comparison">StringComparison type used for building the flat trail</param>
        /// <param name="forFlattening">getting trails for flattening or unflattening,
        /// in the first case we need to make sure the properties are readable in the latter writeable</param>
        /// <returns>all possible trails</returns>
        public static IEnumerable<IList<string>> GetTrails(
            string flatPropertyName,
            object lookupObject,
            Func<string, PropertyInfo, bool> match,
            StringComparison comparison,
            bool forFlattening = true)
        {
            var lookupProps = lookupObject.GetProps();
            return lookupProps.SelectMany(lookupProp => GetTrailsForProperty(flatPropertyName, lookupObject, lookupProp, match, comparison, forFlattening));
        }

        private static IEnumerable<IList<string>> GetTrailsForProperty(
            string flatPropName, 
            object lookupObject,
            PropertyInfo lookupProp, 
            Func<string, PropertyInfo, bool> match,
            StringComparison comparison, 
            bool forFlattening = true)
        {
            if (forFlattening && !lookupProp.CanRead || !forFlattening && !lookupProp.CanWrite)
            {
                yield return null;
                yield break;
            }

            if (match(flatPropName, lookupProp))
            {
                yield return new List<string> { lookupProp.Name };
                yield break;
            }

            if (flatPropName.StartsWith(lookupProp.Name, comparison))
            {
                if (lookupObject != null)
                {
                    lookupObject = lookupProp.GetValue(lookupObject);
                }

                var props = lookupObject == null
                    ? lookupProp.PropertyType.GetProps()
                    : lookupObject.GetProps();

                foreach (var pro in props)
                {
                    foreach (var trail in GetTrailsForProperty(StrUtil.RemovePrefix(flatPropName, lookupProp.Name, comparison), lookupObject, pro, match, comparison, forFlattening))
                    {
                        if (trail != null)
                        {
                            var result = new List<string> { lookupProp.Name };
                            result.AddRange(trail);
                            yield return result;
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Omu.ValueInjecter.Flat
{
    /// <summary>
    /// performs flattening and unflattening
    /// first version of this class was made by Vadim Plamadeala ☺
    /// </summary>
    public static class UberFlatter
    {
        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object target, Func<string, PropertyInfo, bool> match, StringComparison comparison, Func<PropertyInfo, object, object> activator = null)
        {
            var trails = TrailFinder.GetTrails(flatPropertyName, target.GetType().GetProps(), match, comparison, false).Where(o => o != null);

            return trails.Select(trail => Tunnelier.Digg(trail, target, activator));
        }

        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object target, Func<string, PropertyInfo, bool> match, Func<PropertyInfo, object, object> activator = null)
        {
            return Unflat(flatPropertyName, target, match, StringComparison.Ordinal, activator);
        }

        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object target, Func<PropertyInfo, object, object> activator = null)
        {
            return Unflat(flatPropertyName, target, (upn, pi) => upn == pi.Name, activator);
        }

        public static IEnumerable<PropertyWithComponent> Flat(string flatPropertyName, object source, Func<string, PropertyInfo, bool> match)
        {
            return Flat(flatPropertyName, source, match, StringComparison.Ordinal);
        }

        public static IEnumerable<PropertyWithComponent> Flat(string flatPropertyName, object source, Func<string, PropertyInfo, bool> match, StringComparison comparison)
        {
            var trails = TrailFinder.GetTrails(flatPropertyName, source.GetType().GetProps(), match, comparison).Where(o => o != null);

            return trails.Select(trail => Tunnelier.GetValue(trail, source));
        }

        public static IEnumerable<PropertyWithComponent> Flat(string flatPropertyName, object source)
        {
            return Flat(flatPropertyName, source, (up, pi) => up == pi.Name);
        }
    }
}
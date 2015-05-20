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
        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object target, Func<string, PropertyInfo, bool> match, StringComparison comparison)
        {
            var trails = TrailFinder.GetTrails(flatPropertyName, target.GetType().GetProps(), match, comparison, false).Where(o => o != null);

            return trails.Select(trail => Tunnelier.Digg(trail, target));
        }

        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object target, Func<string, PropertyInfo, bool> match)
        {
            return Unflat(flatPropertyName, target, match, StringComparison.Ordinal);
        }

        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object target)
        {
            return Unflat(flatPropertyName, target, (upn, pi) => upn == pi.Name);
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
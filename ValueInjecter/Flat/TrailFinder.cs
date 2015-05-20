using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Omu.ValueInjecter.Flat
{
    public static class TrailFinder
    {
        public static IEnumerable<IList<string>> GetTrails(string upn, IEnumerable<PropertyInfo> props, Func<string, PropertyInfo, bool> match, StringComparison comparison, bool flat = true)
        {
            return props.SelectMany(prop => GetTrails(upn, prop, match, new List<string>(), comparison, flat));
        }

        public static IEnumerable<IList<string>> GetTrails(string upn, PropertyInfo prop, Func<string, PropertyInfo, bool> match, IList<string> root, StringComparison comparison, bool flat = true)
        {
            if (flat && !prop.CanRead || !flat && !prop.CanWrite)
            {
                yield return null;
                yield break;
            }

            if (match(upn, prop))
            {
                var l = new List<string> { prop.Name };
                yield return l;
                yield break;
            }

            if (upn.StartsWith(prop.Name, comparison))
            {
                root.Add(prop.Name);
                foreach (var pro in prop.PropertyType.GetProps())
                {
                    foreach (var trail in GetTrails(upn.RemovePrefix(prop.Name, comparison), pro, match, root, comparison, flat))
                    {
                        if (trail != null)
                        {
                            var r = new List<string> { prop.Name };
                            r.AddRange(trail);
                            yield return r;
                        }
                    }
                }
            }
        }
    }
}
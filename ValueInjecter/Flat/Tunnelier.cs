using System;
using System.Collections.Generic;

namespace Omu.ValueInjecter.Flat
{
    public static class Tunnelier
    {
        public static PropertyWithComponent Digg(IList<string> trail, object o)
        {
            var type = o.GetType();
            if (trail.Count == 1)
            {
                return new PropertyWithComponent { Component = o, Property = type.GetProperty(trail[0]) };
            }

            var prop = type.GetProperty(trail[0]);

            var val = prop.GetValue(o);

            if (val == null)
            {
                val = Activator.CreateInstance(prop.PropertyType);
                prop.SetValue(o, val);
            }

            trail.RemoveAt(0);
            return Digg(trail, val);
        }

        public static PropertyWithComponent GetValue(IList<string> trail, object o, int level = 0)
        {
            var type = o.GetType();

            if (trail.Count == 1)
            {
                return new PropertyWithComponent { Component = o, Property = type.GetProperty(trail[0]), Level = level };
            }

            var prop = type.GetProperty(trail[0]);
            var val = prop.GetValue(o);
            if (val == null) return null;
            trail.RemoveAt(0);
            return GetValue(trail, val, level + 1);
        }
    }
}
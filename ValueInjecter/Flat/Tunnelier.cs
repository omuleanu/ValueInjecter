using System;
using System.Collections.Generic;
using System.Reflection;

namespace Omu.ValueInjecter.Flat
{
    /// <summary>
    /// 
    /// </summary>
    public static class Tunnelier
    {
        /// <summary>
        /// given the trail (path) get the unflat target property info
        /// </summary>
        /// <param name="trail">Unflattening path</param>
        /// <param name="target">object to digg into</param>
        /// <param name="activator">object creator, used to create objects along the way if null is encountered by default Activator.CreateIntance is used</param>
        /// <returns>unflat target property info</returns>
        public static PropertyWithComponent Digg(IList<string> trail, object target, Func<PropertyInfo, object, object> activator = null)
        {
            var type = target.GetType();
            if (trail.Count == 1)
            {
                return new PropertyWithComponent { Component = target, Property = type.GetProperty(trail[0]) };
            }

            var prop = type.GetProperty(trail[0]);

            var val = prop.GetValue(target, null);

            if (val == null)
            {
                val = activator == null ? Activator.CreateInstance(prop.PropertyType) : activator(prop, target);

                prop.SetValue(target, val, null);
            }

            trail.RemoveAt(0);
            return Digg(trail, val, activator);
        }

        /// <summary>
        /// Get the unflat target info
        /// </summary>
        /// <param name="trail">unflat path</param>
        /// <param name="target">object to look into</param>
        /// <returns>unflat target property info</returns>
        public static PropertyWithComponent Find(IList<string> trail, object target)
        {
            return FindTargetInfo(trail, target, 0);
        }

        private static PropertyWithComponent FindTargetInfo(IList<string> trail, object target, int level)
        {
            var type = target.GetType();

            if (trail.Count == 1)
            {
                return new PropertyWithComponent { Component = target, Property = type.GetProperty(trail[0]), Level = level };
            }

            var prop = type.GetProperty(trail[0]);
            var val = prop.GetValue(target, null);
            if (val == null) return null;
            trail.RemoveAt(0);
            return FindTargetInfo(trail, val, level + 1);
        }
    }
}
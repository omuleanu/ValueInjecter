using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Omu.ValueInjecter.Flat
{
    /// <summary>
    /// performs flattening and unflattening; 
    /// flattening = from a.b.c to abc;
    /// unflattening = from abc to a.b.c (if b is null new object is created and assigned);
    /// it's possible to have multiple matches abc => [a.b.c, ab.c a.bc], which is why all methods return a collection of targets;
    /// first version of this class was made by Vadim Plamadeala ☺
    /// </summary>
    public static class UberFlatter
    {
        /// <summary>
        /// Get unflat targets for given flatPropertyName, objects will be created if null encountered in the path towards the target, you can set the value from flatProperty into the target(s)
        /// </summary>
        /// <param name="flatPropertyName">flat property name</param>
        /// <param name="lookupObject">object to look for targets into</param>
        /// <param name="match">match func used for checking the last property in the trail</param>
        /// <param name="comparison"></param>
        /// <param name="activator">creator func, used to create objects along the way if null is encountered, by default Activator.CreateIntance is used</param>
        /// <returns>all matching unflat targets info</returns>
        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object lookupObject, Func<string, PropertyInfo, bool> match, StringComparison comparison, Func<PropertyInfo, object, object> activator = null)
        {
            var trails = TrailFinder.GetTrails(flatPropertyName, lookupObject, match, comparison, false).Where(o => o != null);

            return trails.Select(trail => Tunnelier.Digg(trail, lookupObject, activator));
        }

        /// <summary>
        /// Get unflat targets for given flatPropertyName, objects will be created if null encountered in the path towards the target, you can set the value from flatProperty into the target(s)
        /// </summary>
        /// <param name="flatPropertyName">flat property name</param>
        /// <param name="lookupObject">object to look for targets into</param>
        /// <param name="match">match func used for checking the last property in the trail</param>
        /// <param name="activator">creator func, used to create objects along the way if null is encountered, by default Activator.CreateIntance is used</param>
        /// <returns>all matching unflat targets info</returns>
        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object lookupObject, Func<string, PropertyInfo, bool> match, Func<PropertyInfo, object, object> activator = null)
        {
            return Unflat(flatPropertyName, lookupObject, match, StringComparison.Ordinal, activator);
        }

        /// <summary>
        /// Get unflat targets for given flatPropertyName, objects will be created if null encountered in the path towards the target, you can set the value from flatProperty into the target(s)
        /// </summary>
        /// <param name="flatPropertyName">flat property name</param>
        /// <param name="lookupObject">object to look for targets into</param>
        /// <param name="activator">creator func, used to create objects along the way if null is encountered, by default Activator.CreateIntance is used</param>
        /// <returns>all matching unflat targets info</returns>
        public static IEnumerable<PropertyWithComponent> Unflat(string flatPropertyName, object lookupObject, Func<PropertyInfo, object, object> activator = null)
        {
            return Unflat(flatPropertyName, lookupObject, (upn, pi) => upn == pi.Name, activator);
        }

        /// <summary>
        /// Get unflat targets for given flatPropertyName, you can use the result to get value from and set it into the flat property
        /// </summary>
        /// <param name="flatPropertyName">flat property name</param>
        /// <param name="lookupObject">object to look for targets into</param>
        /// <param name="match">match func used for checking the last property in the trail</param>
        /// <returns>all matching unflat targets info</returns>
        public static IEnumerable<PropertyWithComponent> Flat(string flatPropertyName, object lookupObject, Func<string, PropertyInfo, bool> match)
        {
            return Flat(flatPropertyName, lookupObject, match, StringComparison.Ordinal);
        }

        /// <summary>
        /// Get unflat targets for given flatPropertyName, you can use the result to get value from and set it into the flat property
        /// </summary>
        /// <param name="flatPropertyName">flat property name</param>
        /// <param name="lookupObject">object to look for targets into</param>
        /// <param name="match">match func used for checking the last property in the trail</param>
        /// <param name="comparison">StringComparison type used for building the flat trail</param>
        /// <returns>all matching unflat targets info</returns>
        public static IEnumerable<PropertyWithComponent> Flat(string flatPropertyName, object lookupObject, Func<string, PropertyInfo, bool> match, StringComparison comparison)
        {
            var trails = TrailFinder.GetTrails(flatPropertyName, lookupObject, match, comparison).Where(o => o != null);

            return trails.Select(trail => Tunnelier.Find(trail, lookupObject));
        }

        /// <summary>
        /// Get unflat targets for given flatPropertyName, you can use the result to get value from and set it into the flat property
        /// </summary>
        /// <param name="flatPropertyName">flat property name</param>
        /// <param name="lookupObject">object to look for targets into</param>
        /// <returns>all matching unflat targets info</returns>
        public static IEnumerable<PropertyWithComponent> Flat(string flatPropertyName, object lookupObject)
        {
            return Flat(flatPropertyName, lookupObject, (up, pi) => up == pi.Name);
        }
    }
}
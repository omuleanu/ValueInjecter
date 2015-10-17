using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Omu.ValueInjecter.Injections;
using Omu.ValueInjecter.Utils;

namespace Tests.Injections
{
    public class SmartConventionInjection : ValueInjection
    {
        private class Path
        {
            public IDictionary<string, string> MatchingProps { get; set; }
        }

        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Tuple<Type, Type, string>, Path>> WasLearned = new ConcurrentDictionary<Type, ConcurrentDictionary<Tuple<Type, Type, string>, Path>>();

        protected string[] ignoredProps;

        public SmartConventionInjection()
        {
        }

        public SmartConventionInjection(string[] ignoredProps)
        {
            this.ignoredProps = ignoredProps;
        }

        protected virtual void SetValue(PropertyInfo prop, object component, object value)
        {
            prop.SetValue(component, value);
        }

        protected virtual object GetValue(PropertyInfo prop, object component)
        {
            return prop.GetValue(component);
        }

        protected virtual bool Match(PropertyInfo sourceProp, PropertyInfo targetProp)
        {
            if (ignoredProps != null && ignoredProps.Contains(targetProp.Name)) return false;
            return sourceProp.Name == targetProp.Name && sourceProp.PropertyType == targetProp.PropertyType;
        }

        protected virtual void ExecuteMatch(PropertyInfo sourceProp, PropertyInfo targetProp, object source, object target)
        {
            SetValue(targetProp, target, GetValue(sourceProp, source));
        }

        private Path Learn(IEnumerable<PropertyInfo> sourceProps, IEnumerable<PropertyInfo> targetProps)
        {
            Path path = null;

            foreach (var sourceProp in sourceProps)
            {
                foreach (var targetProp in targetProps)
                {
                    if (!Match(sourceProp, targetProp)) continue;

                    if (path == null)
                        path = new Path
                            {
                                MatchingProps = new Dictionary<string, string> { { sourceProp.Name, targetProp.Name } }
                            };
                    else path.MatchingProps.Add(sourceProp.Name, targetProp.Name);
                }
            }

            return path;
        }

        protected override void Inject(object source, object target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            var sourceProps = sourceType.GetProps();
            var targetProps = targetType.GetProps();

            var cacheEntry = WasLearned.GetOrAdd(GetType(), new ConcurrentDictionary<Tuple<Type, Type, string>, Path>());

            var ignoreds = ignoredProps == null ? null : string.Join(",", ignoredProps);

            var path = cacheEntry.GetOrAdd(new Tuple<Type, Type, string>(sourceType, targetType, ignoreds), pair => Learn(sourceProps, targetProps));

            if (path == null) return;

            foreach (var pair in path.MatchingProps)
            {
                ExecuteMatch(sourceType.GetProperty(pair.Key), targetType.GetProperty(pair.Value), source, target);
            }
        }
    }
}
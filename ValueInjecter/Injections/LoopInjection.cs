using System;
using System.Linq;
using System.Reflection;

namespace Omu.ValueInjecter.Injections
{
    /// <summary>
    /// LoopInjection, by default will match properties with the same name and type;
    /// override MatchTypes to change type matching;
    /// override GetTargetProp to change how the target property is determined based on the source property;
    /// override SetValue to control the how the value is set ( do type casting, ignore setting in certain scenarios etc. )
    /// </summary>
    public class LoopInjection : PropertyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        public LoopInjection()
        {
        }

        /// <summary>
        /// ctor with ignored props setter
        /// </summary>
        /// <param name="ignoredProps"></param>
        public LoopInjection(string[] ignoredProps)
            : base(ignoredProps)
        {
        }

        /// <summary>
        /// get target property name based on source property name
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        protected virtual string GetTargetProp(string sourceName)
        {
            return sourceName;
        }

        /// <summary>
        /// determine if types are matching
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected virtual bool MatchTypes(Type source, Type target)
        {
            return source == target;
        }

        /// <summary>
        /// set target property value 
        /// </summary>
        protected virtual void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source, null);
            tp.SetValue(target, val, null);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            if (sp.CanRead && sp.GetGetMethod() != null && (ignoredProps == null || !ignoredProps.Contains(sp.Name)))
            {
                var tp = target.GetType().GetProperty(GetTargetProp(sp.Name));
                if (tp != null && tp.CanWrite && tp.GetSetMethod() != null && MatchTypes(sp.PropertyType, tp.PropertyType))
                {
                    SetValue(source, target, sp, tp);
                }
            }
        }
    }
}
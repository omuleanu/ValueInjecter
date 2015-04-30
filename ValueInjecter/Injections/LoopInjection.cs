using System;
using System.Linq;
using System.Reflection;

namespace Omu.ValueInjecter.Injections
{
    public class LoopInjection : PropertyInjection
    {
        public LoopInjection()
        {
        }

        public LoopInjection(string[] ignoredProps)
            : base(ignoredProps)
        {
        }

        protected virtual string GetTargetProp(string sourceName)
        {
            return sourceName;
        }

        protected virtual bool MatchTypes(Type source, Type target)
        {
            return source == target;
        }

        protected virtual void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source);
            tp.SetValue(target, val);
        }

        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            if (ignoredProps == null || !ignoredProps.Contains(sp.Name))
            {
                var tp = target.GetType().GetProperty(GetTargetProp(sp.Name));
                if (tp != null && MatchTypes(sp.PropertyType, tp.PropertyType))
                {
                    SetValue(source, target, sp, tp);
                }
            }
        }
    }
}
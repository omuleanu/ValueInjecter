using System;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class NormalToNullables : PropertyInjection
    {
        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            var tp = target.GetType().GetProperty(sp.Name);
            if (tp != null && IsNotIgnored(sp.Name) && sp.PropertyType == Nullable.GetUnderlyingType(tp.PropertyType))
            {
                var val = sp.GetValue(source);
                tp.SetValue(target, val);
            }
        }
    }
}
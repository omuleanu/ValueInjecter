using System;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class NullablesToNormal : PropertyInjection
    {
        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            var targetProp = target.GetType().GetProperty(sp.Name);

            if (targetProp != null && IsNotIgnored(sp.Name) && Nullable.GetUnderlyingType(sp.PropertyType) == targetProp.PropertyType)
            {
                var val = sp.GetValue(source);
                if (val != null)
                    targetProp.SetValue(target, val);
            }
        }
    }
}
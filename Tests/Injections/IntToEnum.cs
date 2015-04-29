using System;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class IntToEnum : PropertyInjection
    {
        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            if (sp.PropertyType == typeof(int) && IsNotIgnored(sp.Name))
            {
                var targetProp = target.GetType().GetProperty(sp.Name);
                if (targetProp != null && targetProp.PropertyType.IsSubclassOf(typeof(Enum)))
                {
                    targetProp.SetValue(target, sp.GetValue(source));
                }
            }
        }
    }
}
using System;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class EnumToInt : PropertyInjection
    {
        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            if (sp.PropertyType.IsSubclassOf(typeof(Enum)) && IsNotIgnored(sp.Name))
            {
                var targetProp = target.GetType().GetProperty(sp.Name);
                if (targetProp != null && targetProp.PropertyType == typeof(int))
                {
                    targetProp.SetValue(target, sp.GetValue(source));
                }
            }
        }
    }
}
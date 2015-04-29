using System;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class DateToStrInjection : PropertyInjection
    {
        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            if (sp.PropertyType == typeof(DateTime) && IsNotIgnored(sp.Name))
            {
                var tp = target.GetType().GetProperty(sp.Name);
                if (tp != null && tp.PropertyType == typeof(string))
                {
                    var val = (DateTime)sp.GetValue(source);
                    
                    tp.SetValue(target, val.ToShortDateString());
                }
            }
        }
    }
}
using System;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class DateToStrInjection : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return source == typeof(DateTime) && target == typeof(string);
        }

        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = (DateTime)sp.GetValue(source);
            tp.SetValue(target, val.ToShortDateString());
        }
    }
}
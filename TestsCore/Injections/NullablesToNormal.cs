using System;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class NullablesToNormal : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return Nullable.GetUnderlyingType(source) == target;
        }

        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source);
            if (val != null)
            {
                tp.SetValue(target, val);
            }
        }
    }
}
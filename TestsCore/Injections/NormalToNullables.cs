using System;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class NormalToNullables : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return source == Nullable.GetUnderlyingType(target);
        }
    }
}
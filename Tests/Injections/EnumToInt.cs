using System;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class EnumToInt : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return source.IsSubclassOf(typeof(Enum)) && target == typeof(int);
        }
    }
}
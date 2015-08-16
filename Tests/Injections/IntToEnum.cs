using System;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class IntToEnum : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return source == typeof(int) && target.IsSubclassOf(typeof(Enum));
        }
    }
}
using System;
using System.Reflection;
using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class EnumToInt : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return source.GetTypeInfo().IsSubclassOf(typeof(Enum)) && target == typeof(int);
        }
    }
}
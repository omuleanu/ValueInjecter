using System.Reflection;

using NUnit.Framework;

using Omu.ValueInjecter.Injections;

namespace Tests.Utils
{
    public class AreEqual : LoopInjection
    {
        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            Assert.AreEqual(sp.GetValue(source), tp.GetValue(target));
        }
    }
}
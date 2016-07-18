using Omu.ValueInjecter.Utils;

namespace Omu.ValueInjecter.Injections
{
    internal class SameNameType : ValueInjection
    {
        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetProps();

            var targetType = target.GetType();

            foreach (var sp in sourceProps)
            {
                if (sp.CanRead && sp.GetGetMethod() != null)
                {
                    var tp = targetType.GetProperty(sp.Name);

                    if (tp != null && tp.CanWrite && sp.PropertyType == tp.PropertyType && tp.GetSetMethod() != null)
                    {
                        tp.SetValue(target, sp.GetValue(source, null), null);
                    }
                }
            }
        }
    }
}
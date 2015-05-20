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
                var tp = targetType.GetProperty(sp.Name);
                if (tp != null && tp.CanWrite && sp.CanRead && sp.PropertyType == tp.PropertyType)
                {
                    tp.SetValue(target, sp.GetValue(source));
                }
            }
        }
    }
}
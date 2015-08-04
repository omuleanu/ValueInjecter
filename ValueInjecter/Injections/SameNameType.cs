using System.Reflection;

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
                if (tp != null && sp.CanRead && sp.PropertyType == tp.PropertyType)
                {
                    var value = sp.GetValue(source);
                    if (tp.CanWrite)
                    {
                        tp.SetValue(target, value);
                    }
                    else if (tp.DeclaringType != null)
                    {
                        //When the property is of a baseclass with a private setter, canwrite is false
                        //The method below is used for setting these kind of properties.
                        var declaringTypePropertyInfo = tp.DeclaringType.GetProperty(tp.Name);
                        if (declaringTypePropertyInfo.CanWrite)
                        {
                            declaringTypePropertyInfo.SetValue(target, value);
                        }
                    }
                }
            }
        }
    }
}
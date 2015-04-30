using System.Linq;
using System.Reflection;

namespace Omu.ValueInjecter.Injections
{
    public class PropertyInjection : ValueInjection
    {
        protected string[] ignoredProps;

        public PropertyInjection()
        {
        }

        public PropertyInjection(string[] ignoredProps)
        {
            this.ignoredProps = ignoredProps;
        }

        protected bool IsNotIgnored(string property)
        {
            return ignoredProps == null || !ignoredProps.Contains(property);
        }

        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetType().GetProps();
            foreach (var s in sourceProps)
            {
                Execute(s, source, target);
            }
        }

        protected virtual void Execute(PropertyInfo sp, object source, object target)
        {
            if (ignoredProps == null || !ignoredProps.Contains(sp.Name))
            {
                var targetProp = target.GetType().GetProperty(sp.Name);
                if (targetProp != null && targetProp.PropertyType == sp.PropertyType)
                {
                    var val = sp.GetValue(source);
                    targetProp.SetValue(target, val);
                }
            }
        }
    }
}
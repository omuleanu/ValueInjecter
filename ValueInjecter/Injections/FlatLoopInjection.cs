using System.Linq;
using System.Reflection;

using Omu.ValueInjecter.Flat;

namespace Omu.ValueInjecter.Injections
{
    public class FlatLoopInjection : ValueInjection
    {
        protected override void Inject(object source, object target)
        {
            var targetProps = target.GetType().GetProps();
            foreach (var tp in targetProps)
            {
                Execute(tp, source, target);
            }
        }

        protected virtual bool Match(string upn, PropertyInfo prop, PropertyInfo target)
        {
            return prop.PropertyType == target.PropertyType && upn == prop.Name;
        }

        protected virtual void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source);
            tp.SetValue(target, val);
        }

        protected void Execute(PropertyInfo tp, object source, object target)
        {
            var endpoints = UberFlatter.Flat(tp.Name, source, (upn, prop) => Match(upn, prop, tp)).ToArray();

            if (endpoints.Any())
            {
                var endpoint = endpoints.First();
                if (endpoint != null)
                {
                    SetValue(endpoint.Component, target, endpoint.Property, tp);
                }
            }
        }
    }
}
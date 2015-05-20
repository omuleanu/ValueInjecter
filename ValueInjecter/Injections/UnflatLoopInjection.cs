using System.Linq;
using System.Reflection;

using Omu.ValueInjecter.Flat;

namespace Omu.ValueInjecter.Injections
{
    public class UnflatLoopInjection : ValueInjection
    {
        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetType().GetProps();
            foreach (var sp in sourceProps)
            {
                Execute(sp, source, target);
            }
        }

        protected virtual bool Match(string upn, PropertyInfo prop, PropertyInfo sourceProp)
        {
            return prop.PropertyType == sourceProp.PropertyType && upn == prop.Name;
        }

        protected virtual void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            tp.SetValue(target, sp.GetValue(source));
        }

        protected virtual void Execute(PropertyInfo sp, object source, object target)
        {
            if (sp.CanRead)
            {
                var endpoints = UberFlatter.Unflat(sp.Name, target, (upn, prop) => Match(upn, prop, sp)).ToArray();

                foreach (var endpoint in endpoints)
                {
                    SetValue(source, endpoint.Component, sp, endpoint.Property);
                }
            }
        }
    }
}
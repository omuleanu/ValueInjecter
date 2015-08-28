using System;
using System.Linq;
using System.Reflection;

using Omu.ValueInjecter.Flat;

namespace Omu.ValueInjecter.Injections
{
    public class UnflatLoopInjection : ValueInjection
    {
        protected Func<PropertyInfo, object, object> activator;

        public UnflatLoopInjection()
        {
        }

        public UnflatLoopInjection(Func<PropertyInfo, object, object> activator)
        {
            this.activator = activator;
        }

        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetType().GetProps();
            foreach (var sp in sourceProps)
            {
                Execute(sp, source, target);
            }
        }

        protected virtual bool Match(string unflatName, PropertyInfo prop, PropertyInfo sourceProp)
        {
            return prop.PropertyType == sourceProp.PropertyType && unflatName == prop.Name && prop.GetSetMethod() != null;
        }

        protected virtual void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            tp.SetValue(target, sp.GetValue(source));
        }

        protected virtual void Execute(PropertyInfo sp, object source, object target)
        {
            if (sp.CanRead && sp.GetGetMethod() != null)
            {
                var endpoints = UberFlatter.Unflat(sp.Name, target, (upn, prop) => Match(upn, prop, sp), activator).ToArray();

                foreach (var endpoint in endpoints)
                {
                    SetValue(source, endpoint.Component, sp, endpoint.Property);
                }
            }
        }
    }
}
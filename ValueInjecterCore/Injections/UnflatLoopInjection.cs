using System;
using System.Linq;
using System.Reflection;

using Omu.ValueInjecter.Flat;
using Omu.ValueInjecter.Utils;

namespace Omu.ValueInjecter.Injections
{
    /// <summary>
    /// UnflatLoopInjection, matches flat properties to unflat ( abc => a.b.c );
    /// override SetValue to control the how the value is set ( do type casting, ignore setting in certain scenarios etc. );
    /// override Match to control unflat target checking;
    /// </summary>
    public class UnflatLoopInjection : ValueInjection
    {
        protected Func<PropertyInfo, object, object> activator;

        public UnflatLoopInjection()
        {
        }

        /// <summary>
        /// Create injection and set the creator func
        /// </summary>
        /// <param name="activator">creator func, used to create objects along the way if null is encountered, by default Activator.CreateIntance is used</param>
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

        protected virtual bool Match(string propName, PropertyInfo unflatProp, PropertyInfo sourceFlatProp)
        {
            return unflatProp.PropertyType == sourceFlatProp.PropertyType && propName == unflatProp.Name && unflatProp.GetSetMethod() != null;
        }

        protected virtual void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            tp.SetValue(target, sp.GetValue(source, null), null);
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
using System.Linq;
using System.Reflection;

using Omu.ValueInjecter.Flat;
using Omu.ValueInjecter.Utils;

namespace Omu.ValueInjecter.Injections
{
    /// <summary>
    /// FlatLoopInjection, matches unflat properties to flat ( a.b.c => abc )
    /// override SetValue to control the how the value is set ( do type casting, ignore setting in certain scenarios etc. )
    /// override Match to control unflat target checking
    /// </summary>
    public class FlatLoopInjection : ValueInjection
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>        
        protected override void Inject(object source, object target)
        {
            var targetProps = target.GetType().GetProps();
            foreach (var tp in targetProps)
            {
                Execute(tp, source, target);
            }
        }

        /// <summary>
        /// match properties
        /// </summary>        
        protected virtual bool Match(string propName, PropertyInfo unflatProp, PropertyInfo targetFlatProp)
        {
            return unflatProp.PropertyType == targetFlatProp.PropertyType && propName == unflatProp.Name && unflatProp.GetGetMethod() != null;
        }


        /// <summary>
        /// set value in the target property
        /// </summary>
        protected virtual void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            tp.SetValue(target, sp.GetValue(source, null), null);
        }

        /// <summary>
        /// execute injection on all properties
        /// </summary>
        protected void Execute(PropertyInfo tp, object source, object target)
        {
            if (tp.CanWrite && tp.GetSetMethod() != null)
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
}
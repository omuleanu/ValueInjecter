using System.Linq;
using System.Reflection;

using Omu.ValueInjecter.Utils;

namespace Omu.ValueInjecter.Injections
{
    /// <summary>
    /// Property injection
    /// </summary>
    public class PropertyInjection : ValueInjection
    {
        /// <summary>
        ///  properties to ignore
        /// </summary>
        protected string[] ignoredProps;

        /// <summary>
        /// 
        /// </summary>
        public PropertyInjection()
        {
        }

        /// <summary>
        /// ctor with properties to ignore setter
        /// </summary>
        /// <param name="ignoredProps"></param>
        public PropertyInjection(string[] ignoredProps)
        {
            this.ignoredProps = ignoredProps;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetType().GetProps();
            foreach (var sourceProp in sourceProps)
            {
                Execute(sourceProp, source, target);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected virtual void Execute(PropertyInfo sp, object source, object target)
        {
            if (sp.CanRead && sp.GetGetMethod() != null && (ignoredProps == null || !ignoredProps.Contains(sp.Name)))
            {
                var tp = target.GetType().GetProperty(sp.Name);
                if (tp != null && tp.CanWrite && tp.PropertyType == sp.PropertyType && tp.GetSetMethod() != null)
                {
                    tp.SetValue(target, sp.GetValue(source, null), null);
                }
            }
        }
    }
}
namespace Omu.ValueInjecter.Injections
{
    /// <summary>
    /// known target type injection
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public abstract class KnownTargetInjection<TTarget> : IValueInjection
    {
        /// <summary>
        /// set values into target object based on source object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public object Map(object source, object target)
        {
            var theTarget = (TTarget) target;
            Inject(source, ref theTarget);
            target = theTarget;
            return target;
        }

        /// <summary>
        /// set values into target object based on source object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        protected abstract void Inject(object source, ref TTarget target);
    }
}
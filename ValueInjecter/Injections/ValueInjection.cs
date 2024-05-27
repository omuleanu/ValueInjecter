namespace Omu.ValueInjecter.Injections
{    
    /// <inheritdoc/>    
    public abstract class ValueInjection : IValueInjection
    {
        /// <inheritdoc/>    
        public object Map(object source, object target)
        {
            Inject(source, target);
            return target;
        }

        /// <summary>
        /// Map source object to target object
        /// </summary>
        protected abstract void Inject(object source, object target);
    }
}
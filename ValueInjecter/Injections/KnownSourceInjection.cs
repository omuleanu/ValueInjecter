namespace Omu.ValueInjecter.Injections
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract class KnownSourceInjection<TSource> : IValueInjection
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object Map(object source, object target)
        {
            Inject((TSource) source, target);
            return target;
        }
        
        /// <summary>
        /// map TSource object to object target
        /// </summary>
        protected abstract void Inject(TSource source, object target);
    }
}
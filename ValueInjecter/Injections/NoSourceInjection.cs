namespace Omu.ValueInjecter.Injections
{
    ///<summary>
    /// inject value without source
    ///</summary>
    public abstract class NoSourceInjection : INoSourceInjection
    {
        /// <summary>
        /// map target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public object Map(object target)
        {
            Inject(target);
            return target;
        }

        /// <summary>
        /// inject value into target object
        /// </summary>
        /// <param name="target"></param>
        protected abstract void Inject(object target);
    }
}
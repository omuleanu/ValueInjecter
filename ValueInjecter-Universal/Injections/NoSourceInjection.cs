namespace Omu.ValueInjecter.Injections
{
    ///<summary>
    /// inject value without source
    ///</summary>
    public abstract class NoSourceInjection : INoSourceInjection
    {
        public object Map(object target)
        {
            Inject(target);
            return target;
        }

        protected abstract void Inject(object target);
    }
}
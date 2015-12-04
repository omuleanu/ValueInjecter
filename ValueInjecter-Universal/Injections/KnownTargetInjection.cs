namespace Omu.ValueInjecter.Injections
{
    public abstract class KnownTargetInjection<TTarget> : IValueInjection
    {
        public object Map(object source, object target)
        {
            var theTarget = (TTarget) target;
            Inject(source, ref theTarget);
            target = theTarget;
            return target;
        }

        protected abstract void Inject(object source, ref TTarget target);
    }
}
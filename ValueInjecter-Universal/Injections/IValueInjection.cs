namespace Omu.ValueInjecter.Injections
{
    public interface IValueInjection
    {
        object Map(object source, object target);
    }
}
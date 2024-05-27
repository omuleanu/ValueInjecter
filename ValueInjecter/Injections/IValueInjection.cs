namespace Omu.ValueInjecter.Injections
{
    /// <summary>
    /// value injection
    /// </summary>
    public interface IValueInjection
    {
        /// <summary>
        ///  map source object properties to target object properties
        /// </summary>
        object Map(object source, object target);
    }
}
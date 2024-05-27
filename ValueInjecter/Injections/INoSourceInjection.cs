namespace Omu.ValueInjecter.Injections
{
    /// <summary>
    /// injection without source
    /// </summary>
    public interface INoSourceInjection
    {
        /// <summary>
        /// inject value into target object
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        object Map(object target);
    }
}
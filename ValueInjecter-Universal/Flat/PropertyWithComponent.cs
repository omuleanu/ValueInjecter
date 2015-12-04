using System.Reflection;

namespace Omu.ValueInjecter.Flat
{
    /// <summary>
    /// Unflat target property info
    /// </summary>
    public class PropertyWithComponent
    {
        /// <summary>
        /// Property that is target of unflatteing path
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// Object last target of unflattening path
        /// </summary>
        public object Component { get; set; }

        /// <summary>
        /// Unflatteing level (a.b.c = level 2)
        /// </summary>
        public int Level { get; set; }
    }
}
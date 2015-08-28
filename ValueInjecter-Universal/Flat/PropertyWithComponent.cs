using System.Reflection;

namespace Omu.ValueInjecter.Flat
{
    public class PropertyWithComponent
    {
        public PropertyInfo Property { get; set; }

        public object Component { get; set; }

        public int Level { get; set; }
    }
}
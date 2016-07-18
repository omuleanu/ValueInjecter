using System;

using Omu.ValueInjecter.Injections;

namespace Tests.Injections
{
    public class DateTimeToStr : ValueInjection
    {
        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetType().GetProperties();
            var targetType = target.GetType();
            foreach (var s in sourceProps)
            {
                var t = targetType.GetProperty(s.Name);
                if (t == null) continue;
                if (s.PropertyType == typeof(DateTime) && t.PropertyType == typeof(string))
                    t.SetValue(target, s.GetValue(source).ToString());
            }
        }
    }
}
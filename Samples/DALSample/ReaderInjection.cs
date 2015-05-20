using System;
using System.Data;
using System.Reflection;

using Omu.ValueInjecter.Injections;

namespace DALSample
{
    public class ReaderInjection : KnownSourceInjection<IDataReader>
    {
        protected override void Inject(IDataReader source, object target)
        {
            for (var i = 0; i < source.FieldCount; i++)
            {
                var trgProp = target.GetType().GetProperty(source.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (trgProp == null) continue;

                var value = source.GetValue(i);
                if (value == DBNull.Value) continue;

                trgProp.SetValue(target, value);
            }
        }
    }
}
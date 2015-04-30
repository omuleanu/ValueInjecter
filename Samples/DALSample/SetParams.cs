using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace DALSample
{
    public class SetParams : KnownTargetInjection<SqlCommand>
    {
        private IEnumerable<string> ignoredFields = new string[] { };
        private string prefix = string.Empty;

        public SetParams Prefix(string p)
        {
            prefix = p;
            return this;
        }

        public SetParams IgnoreFields(params string[] fields)
        {
            ignoredFields = fields.AsEnumerable();
            return this;
        }

        protected override void Inject(object source, ref SqlCommand cmd)
        {
            if (source == null) return;
            var sourceProps = source.GetType().GetProperties();

            foreach (var prop in sourceProps)
            {
                if (ignoredFields.Contains(prop.Name)) continue;

                var value = prop.GetValue(source) ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@" + prefix + prop.Name, value);
            }
        }
    }
}
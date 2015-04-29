using System;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace DALSample
{
    public class FieldsBy : KnownTargetInjection<string>
    {
        private IEnumerable<string> ignoredFields = new string[] { };
        private string format = "{0}";
        private string nullFormat;
        private string glue = ",";

        public FieldsBy SetGlue(string g)
        {
            glue = " " + g + " ";
            return this;
        }

        public FieldsBy IgnoreFields(params string[] fields)
        {
            ignoredFields = fields;
            return this;
        }

        public FieldsBy SetFormat(string f)
        {
            format = f;
            return this;
        }

        public FieldsBy SetNullFormat(string f)
        {
            nullFormat = f;
            return this;
        }

        protected override void Inject(object source, ref string target)
        {
            var sourceProps = source.GetType().GetProperties();
            var s = string.Empty;
            foreach (var prop in sourceProps)
            {
                if (ignoredFields.Contains(prop.Name)) continue;
                if (prop.GetValue(source) == DBNull.Value && nullFormat != null)
                    s += string.Format(nullFormat, prop.Name);
                else
                    s += string.Format(format, prop.Name) + glue;
            }
            s = s.RemoveSuffix(glue);
            target += s;
        }
    }
}
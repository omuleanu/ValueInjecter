using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

using NUnit.Framework;

using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

using Tests.Utils;

namespace Tests.Misc
{
    public class DanRyanIDataReaderTests
    {
        [Test]
        public void Main()
        {
            var persons = new List<Person>();
            var table = CreateSampleDataTable();
            var reader = table.CreateDataReader();

            while (reader.Read())
            {
                var p = new Person();

                p.InjectFrom<ReaderInjection>(reader);

                p.name = new Name();
                p.name.InjectFrom<ReaderInjection>(reader);

                persons.Add(p);
            }

            persons.Count.IsEqualTo(5);
            persons[0].id.IsEqualTo(100);
            persons[0].name.first_name.IsEqualTo("Jeff");
            persons[0].name.last_name.IsEqualTo("Barnes");
        }

        public class Person
        {
            public int id { get; set; }
            public Name name { get; set; }
        }

        public class Name
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
        }

        private static DataTable CreateSampleDataTable()
        {
            var table = new DataTable();

            table.Columns.Add("id", typeof(int));
            table.Columns.Add("first_name", typeof(string));
            table.Columns.Add("last_name", typeof(string));

            table.Rows.Add(100, "Jeff", "Barnes");
            table.Rows.Add(101, "George", "Costanza");
            table.Rows.Add(102, "Stewie", "Griffin");
            table.Rows.Add(103, "Stan", "Marsh");
            table.Rows.Add(104, "Eric", "Cartman");
            return table;
        }

        public class ReaderInjection : KnownSourceInjection<IDataReader>
        {
            protected override void Inject(IDataReader source, object target)
            {
                for (var i = 0; i < source.FieldCount; i++)
                {
                    var activeTarget = target.GetType().GetProperty(source.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (activeTarget == null) continue;

                    var value = source.GetValue(i);
                    if (value == DBNull.Value) continue;

                    activeTarget.SetValue(target, value);
                }
            }
        }
    }
}
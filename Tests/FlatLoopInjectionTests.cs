using System;
using System.Reflection;

using NUnit.Framework;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class FlatLoopInjectionTests
    {
        public class Foo
        {
            public string Name { get; set; }
            public int a { get; set; }
            public string _a { get; set; }
            public bool b { get; set; }
            public DateTime d { get; set; }
            public Foo Parent { get; set; }
            public bool Bool { get; set; }
        }

        public class FlatFoo
        {
            public string ParentName { get; set; }
            public int Parenta { get; set; }
            public string Parent_a { get; set; }
            public string Parentb { get; set; }
            public string ParentParentName { get; set; }
            public string oO { get; set; }
            public string d { get; set; }
            public object Bool { get; set; }
        }

        /// <summary>
        /// copy only bool properties into equivalent flattened property
        /// </summary>
        public class FlatBoolToString : FlatLoopInjection
        {
            protected override bool Match(string propName, PropertyInfo unflatProp, PropertyInfo targetFlatProp)
            {
                return propName == unflatProp.Name && 
                    unflatProp.PropertyType == typeof(bool) && 
                    targetFlatProp.PropertyType == typeof(string);
            }

            protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
            {
                var val = sp.GetValue(source).ToString();
                tp.SetValue(target, val);
            }
        }

        [Test]
        public void FlatteningTest()
        {
            var unflat = new Foo
            {
                Parent = new Foo
                {
                    _a = "aaa",
                    a = 23,
                    b = true,
                    Name = "v"
                }
            };

            var flat = new FlatFoo();

            flat.InjectFrom<FlatLoopInjection>(unflat);
            flat.Parent_a.IsEqualTo(unflat.Parent._a);
            flat.Parenta.IsEqualTo(unflat.Parent.a);
            flat.Parentb.IsEqualTo(null);
            flat.ParentName.IsEqualTo(unflat.Parent.Name);
            flat.ParentParentName.IsEqualTo(null);
            flat.oO.IsEqualTo(null);
            flat.d.IsEqualTo(null);
        }

        [Test]
        public void GenericFlatTest()
        {
            var unflat = new Foo
            {
                Parent = new Foo
                {
                    _a = "aaa",
                    a = 23,
                    b = true,
                    Name = "v"
                }
            };

            var flat = new FlatFoo();

            flat.InjectFrom<FlatBoolToString>(unflat);
            flat.Parentb.IsEqualTo("True");
            flat.Bool.IsEqualTo(null);
        }

        [Test]
        public void ObjectFlatTest()
        {
            var unflat = (object) new
            {
                Parent = (object)new Foo
                {                    
                    b = true,
                    Name = "v"
                }
            };

            var flat = new FlatFoo();

            flat.InjectFrom<FlatBoolToString>(unflat);
            flat.Parentb.IsEqualTo("True");            
            flat.Bool.IsEqualTo(null);
        }
    }
}
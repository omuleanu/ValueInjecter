using System;
using System.Reflection;

using NUnit.Framework;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class FlatteningTest
    {
        public class Boo
        {
            public Boo Parent { get; set; }
        }

        public class Foo
        {
            public Foo Foo1 { get; set; }
            public Foo Foo2 { get; set; }
            public string Name { get; set; }
            public string NameZype { get; set; }
            public int Age { get; set; }
        }

        public class FlatFoo
        {
            public string Foo1Foo2Foo1Name { get; set; }
            public string Foo1Name { get; set; }
            public string Foo2Name { get; set; }
            public string Foo2NameZype { get; set; }
            public string Foo1Age { get; set; }
            public bool Age { get; set; }

        }

        public class IntToStringFlat : FlatLoopInjection
        {
            protected override bool Match(string upn, PropertyInfo prop, PropertyInfo target)
            {
                return upn == prop.Name && prop.PropertyType == typeof(int) && target.PropertyType == typeof(string);
            }

            protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
            {
                var val = sp.GetValue(source).ToString();
                tp.SetValue(target, val);
            }
        }

        public class IntToStrUnflat : UnflatLoopInjection
        {
            protected override bool Match(string upn, PropertyInfo prop, PropertyInfo sourceProp)
            {
                return upn == prop.Name && prop.PropertyType == typeof(int) && sourceProp.PropertyType == typeof(string);
            }

            protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
            {
                var val = Convert.ToInt32(sp.GetValue(source));
                tp.SetValue(target, val);
            }
        }

        [Test]
        public void Unflattening()
        {
            var flat = new FlatFoo
                           {
                               Foo1Foo2Foo1Name = "cool",
                               Foo1Name = "abc",
                               Foo2Name = "123",
                           };

            var unflat = new Foo();

            unflat.InjectFrom<UnflatLoopInjection>(flat);

            unflat.Foo1.Foo2.Foo1.Name.IsEqualTo("cool");
            unflat.Foo1.Name.IsEqualTo("abc");
            unflat.Foo2.Name.IsEqualTo("123");
        }

        [Test]
        public void Flattening()
        {
            var unflat = new Foo
                             {
                                 Name = "foo",
                                 Foo1 = new Foo
                                            {
                                                Name = "abc",
                                                Foo2 = new Foo { Foo1 = new Foo { Name = "inner" } }
                                            },
                                 Foo2 = new Foo { Name = "123", NameZype = "aaa" },
                             };

            var flat = new FlatFoo();

            flat.InjectFrom<FlatLoopInjection>(unflat);

            flat.Foo2NameZype.IsEqualTo("aaa");
        }

        [Test]
        public void GenericFlatTest()
        {
            var foo = new Foo { Foo1 = new Foo { Age = 18 } };
            var flat = new FlatFoo();

            flat.InjectFrom<IntToStringFlat>(foo);
            flat.Foo1Age.IsEqualTo("18");
        }

        [Test]
        public void GenericUnflatTest()
        {
            var flat = new FlatFoo { Foo1Age = "16" };
            var foo = new Foo();

            foo.InjectFrom<IntToStrUnflat>(flat);
            foo.Foo1.Age.IsEqualTo(16);
        }
    }
}
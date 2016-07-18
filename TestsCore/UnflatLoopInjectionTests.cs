using System;
using System.Reflection;

using NUnit.Framework;

using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace Tests
{
    public class UnflatLoopInjectionTests
    {
        [Test]
        public void ShouldMapFlatProperties()
        {
            var flat = new FlatType { FooBarName = "hi" };

            var unflat = new UnflatType();

            unflat.InjectFrom<UnflatLoopInjection>(flat);

            Assert.AreEqual(flat.FooBarName, unflat.Foo.Bar.Name);
        }

        [Test]
        public void ShouldMapFlatInterfaceProperty()
        {
            var flat = new  { Foo2Bar2Name = "hi" };

            var unflat = new UnflatType();

            unflat.InjectFrom(new UnflatLoopInjection((prop, obj) =>
                { 
                    var propType = prop.PropertyType;
                    if (propType.GetTypeInfo().IsInterface)
                    {
                        if (propType == typeof(IFoo)) return new Foo();
                        if (propType == typeof(IBar)) return new Bar();
                    }

                    return Activator.CreateInstance(propType);

                }), flat);

            Assert.AreEqual(flat.Foo2Bar2Name, unflat.Foo2.Bar2.Name);
        }

        [Test]
        public void GenericUnflatTest()
        {
            var flat = new  { FooBarNumber = "123" };
            var foo = new UnflatType();

            foo.InjectFrom<StrToIntUnflat>(flat);
            Assert.AreEqual(123, foo.Foo.Bar.Number);
        }

        public class FlatType
        {
            public string FooBarName { get; set; }
        }

        public class UnflatType
        {
            public Foo Foo { get; set; }

            public IFoo Foo2 { get; set; }
        }

        public interface IFoo
        {
            Bar Bar { get; set; }

            IBar Bar2 { get; set; }
        }

        public class Foo : IFoo
        {
            public Bar Bar { get; set; }

            public IBar Bar2 { get; set; }
        }

        public interface IBar
        {
            string Name { get; set; }
        }

        public class Bar : IBar
        {
            public string Name { get; set; }

            public int Number { get; set; }
        }

        public class StrToIntUnflat : UnflatLoopInjection
        {
            protected override bool Match(string propName, PropertyInfo unflatProp, PropertyInfo sourceFlatProp)
            {
                return propName == unflatProp.Name && unflatProp.PropertyType == typeof(int) && sourceFlatProp.PropertyType == typeof(string);
            }

            protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
            {
                var val = Convert.ToInt32(sp.GetValue(source));
                tp.SetValue(target, val);
            }
        }
    }
}
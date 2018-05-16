using NUnit.Framework;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Omu.ValueInjecter.Utils;

namespace Tests.Misc
{
    public class ShadowPropTest
    {
        public class Foo
        {
            public virtual string Name { get; set; }

            public virtual int Prop2 { get; set; }

        }

        public class FooBar : Foo
        {
            public new string Name { get; set; }

            public new string Prop2 { get; set; }
        }

        public class SameNameType2 : ValueInjection
        {
            protected override void Inject(object source, object target)
            {
                var sourceProps = source.GetProps();

                var targetType = target.GetType();

                foreach (var sp in sourceProps)
                {
                    if (sp.CanRead && sp.GetGetMethod() != null)
                    {
                        var tp = targetType.GetProperty(sp.Name, sp.PropertyType);

                        if (tp != null && tp.CanWrite && sp.PropertyType == tp.PropertyType && tp.GetSetMethod() != null)
                        {
                            tp.SetValue(target, sp.GetValue(source, null), null);
                        }
                    }
                }
            }
        }

        [Test]
        public void ShouldMap()
        {
            var src = new FooBar
            {
                Name = "abc",
                Prop2 = "bce"
            };

            Mapper.AddMap<FooBar, FooBar>(from =>
            {
                var fb = new FooBar();
                fb.InjectFrom<SameNameType2>(from);
                return fb;
            });

            var res = Mapper.Map<FooBar>(src);

            Assert.AreEqual(res.Name, src.Name);
            Assert.AreEqual(res.Prop2, src.Prop2);
        }
    }
}
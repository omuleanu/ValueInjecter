using NUnit.Framework;
using Omu.ValueInjecter;

using Tests.Injections;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class NullableInjectionsTests
    {
        public class E
        {
            public bool b { get; set; }
            public int i { get; set; }
            public int j { get; set; }
            public int x { get; set; }
            public string s { get; set; }
        }

        public class D
        {
            public bool? b { get; set; }
            public int? i { get; set; }
            public int? j { get; set; }
            public int x { get; set; }
            public int? s { get; set; }
        }
       
        [Test]
        public void NullablesToNormalTest()
        {
            var d = new D { b = true, i = 1, x = 3 };
            var e = new E();

            e.InjectFrom<NullablesToNormal>(d);

            e.i.IsEqualTo(1);
            e.b.IsEqualTo(true);
            e.x.IsEqualTo(0);
            e.j.IsEqualTo(0);
            e.s.IsEqualTo(null);
        }

        [Test]
        public void NormalToNullablesTest()
        {
            var e = new E { b = true, i = 1, x = 3213213, j = 123 };
            var d = new D();

            d.InjectFrom<NormalToNullables>(e);

            d.i.IsEqualTo(1);
            d.b.IsEqualTo(true);
            d.x.IsEqualTo(0);
            d.j.IsEqualTo(123);
            d.s.IsEqualTo(null);
        }
    }
}

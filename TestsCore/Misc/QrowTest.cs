using System;
using NUnit.Framework;

using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

using Tests.Utils;

namespace Tests.Misc
{
    public class QrowTest
    {
        public class Boo
        {
            public string Id { get; set; }
            public int Boo1Id { get; set; }
            public Boo Boo1 { get; set; }
        }

        public class FlatBoo
        {
            public int Boo1Id { get; set; }
        }

        [Test]
        public void AnotherUnflatTest()
        {
            var flat = new FlatBoo { Boo1Id = 5 };
            var boo = new Boo();

            boo.InjectFrom<UnflatLoopInjection>(flat);

            boo.Boo1Id.IsEqualTo(5);
            boo.Boo1.IsEqualTo(null);
        }

        [Test]
        public void MainTest()
        {
            var b = new B
            {
                Id = Guid.NewGuid(),
                Name = "Test!",
                Age = 47
            };

            var a = new A();

            a.InjectFrom(b);
        }
    }

    public class A
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class B : A
    {
        public int Age { get; set; }
    }
}
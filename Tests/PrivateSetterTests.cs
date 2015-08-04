using System;
using NUnit.Framework;
using Omu.ValueInjecter;

namespace Tests
{

    [TestFixture]
    public class PrivateSetterTests
    {
        [Test]
        public void WhenInjectingAPrivateSetterShouldBeIgnored()
        {
            var e = new EntityForPrivateSetter
            {
                Name = Guid.NewGuid().ToString(),
                Dummy = Guid.NewGuid().ToString()
            };

            var dto = new DtoWithPrivateSetter();
            dto.InjectFrom(e);
            Assert.AreEqual(e.Name, dto.Name);
            Assert.AreEqual(e.Dummy, dto.Dummy);
        }

        [Test]
        public void WhenInjectingAPrivateSetterFromBaseClassShouldBeIgnored()
        {
            var e = new EntityForPrivateSetter
            {
                Name = Guid.NewGuid().ToString(),
                Dummy = Guid.NewGuid().ToString()
            };

            var dto = new DtoWithPrivateSetterInBase();
            dto.InjectFrom(e);
            Assert.AreEqual(e.Name, dto.Name);
            Assert.AreEqual(e.Dummy, dto.Dummy);
        }
    }

    public class DtoWithPrivateSetterInBaseBase
    {
        public string Name { get; private set; }
    }

    public class DtoWithPrivateSetterInBase : DtoWithPrivateSetterInBaseBase
    {
        public string Dummy { get; set; }
    }

    public class DtoWithPrivateSetter
    {
        public string Name { get; private set; }
        public string Dummy { get; set; }
    }

    public class EntityForPrivateSetter    
    {
        public string Name { get; set; }
        public string Dummy { get; set; }
    }
}
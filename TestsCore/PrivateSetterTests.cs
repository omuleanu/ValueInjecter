using NUnit.Framework;

using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

using Tests.SampleTypes.PrivateSetter;

namespace Tests
{
    [TestFixture]
    public class PrivateSetterTests
    {
        [Test]
        public void ShouldNotUsePrivateGetterProperty()
        {
            var src = new Privategetset { Pget = "hi", BasePget = "hi2" };

            var res = new Publicgetset();

            res.InjectFrom(src);
            Assert.IsNull(res.Pget);
            Assert.IsNull(res.BasePget);
            
            res.InjectFrom<LoopInjection>(src);
            res.InjectFrom<FlatLoopInjection>(src);
            res.InjectFrom<UnflatLoopInjection>(src);
            Assert.IsNull(res.Pget);
            Assert.IsNull(res.BasePget);
        }

        [Test]
        public void ShouldNotUsePrivateSetterProperty()
        {
            var src = new
                {
                    Pset = "hi",
                    BasePset = "hi2",
                };

            var res = new Privategetset();
            res.InjectFrom(src);

            Assert.IsNull(res.Pset);
            Assert.IsNull(res.BasePset);
            
            res.InjectFrom<LoopInjection>(src);
            res.InjectFrom<FlatLoopInjection>(src);
            res.InjectFrom<UnflatLoopInjection>(src);
            Assert.IsNull(res.Pset);
            Assert.IsNull(res.BasePset);
        }
    }
}
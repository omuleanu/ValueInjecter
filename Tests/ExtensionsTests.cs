using NUnit.Framework;
using Omu.ValueInjecter;

using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void RemovePrefix()
        {
            const string S = "txtName";
            S.RemovePrefix("txt").IsEqualTo("Name");
        }

        [Test]
        public void RemoveSuffix()
        {
            const string S = "NameRaw";
            S.RemoveSuffix("Raw").IsEqualTo("Name");
        }
    }
}
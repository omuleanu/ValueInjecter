using NUnit.Framework;
using Omu.ValueInjecter.Utils;

using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void RemovePrefix()
        {
            const string S = "txtName";
            StrUtil.RemovePrefix(S, "txt").IsEqualTo("Name");
        }

        [Test]
        public void RemoveSuffix()
        {
            const string S = "NameRaw";
            StrUtil.RemoveSuffix(S, "Raw").IsEqualTo("Name");
        }
    }
}
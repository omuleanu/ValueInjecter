using System.Collections.Generic;
using NUnit.Framework;

using Omu.ValueInjecter.Flat;

using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class TunnelierTests
    {
        public class Foo
        {
            public string Name { get; set; }
            public Foo Parent { get; set; }
        }

        [Test]
        public void GetValueFromAUnfullBranchReturnNull()
        {
            var o = new Foo() { Parent = new Foo() { } };
            Tunnelier.Find(new List<string>() { "Parent", "Parent", "Name" }, o).IsEqualTo(null);
        }

        [Test]
        public void GetValueReturns()
        {
            var o = new Foo { Parent = new Foo { Parent = new Foo { Name = "hey" } } };
            var endpoint = Tunnelier.Find(new List<string>{"Parent", "Parent","Name"}, o);
            endpoint.Property.GetValue(endpoint.Component).IsEqualTo("hey");
        }

        [Test]
        public void DiggDuggs()
        {
            var o = new Foo();
            Tunnelier.Digg(new List<string> {"Parent", "Parent", "Name"}, o, null).IsNotNull();
        }
    }
}
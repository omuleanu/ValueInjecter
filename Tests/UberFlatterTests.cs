using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using Omu.ValueInjecter.Flat;

using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class UberFlatterTests
    {
        public class Bar1
        {
            public string Name { get; set; }
        }

        public class FooBar1
        {
            public string Name { get; set; }
        }

        public class Foo1
        {
            public Bar1 Bar { get; set; }
        }

        public class Unflat
        {
            public Unflat The { get; set; }
            public Foo1 Foo { get; set; }
            public FooBar1 FooBar { get; set; }
            public Unflat The2 { get; set; }
        }

        public class Flat
        {
            public string FooBarName { get; set; }
        }

        private bool Match(string upn, PropertyInfo propertyInfo)
        {
            return upn == propertyInfo.Name && propertyInfo.PropertyType == typeof(string);
        }

        private bool MatchIgnoreCase(string upn, PropertyInfo propertyInfo)
        {
            return upn.Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase) && propertyInfo.PropertyType == typeof(string);
        }

        [Test]
        public void FlatTest()
        {
            var u = new Unflat() { Foo = new Foo1() { Bar = new Bar1() { Name = "dasName" } }, FooBar = new FooBar1() { Name = "uber" } };
            var vv = UberFlatter.Flat("FooBarName", u, Match);
            vv.Count().IsEqualTo(2);

            var vvv = UberFlatter.Flat("FooBarName", u);
            vvv.Count().IsEqualTo(2);
        }

        [Test]
        public void FlatTestIgnoreCase()
        {
            var u = new Unflat() { Foo = new Foo1() { Bar = new Bar1() { Name = "dasName" } }, FooBar = new FooBar1() { Name = "uber" } };
            var vv = UberFlatter.Flat("foobarname", u, MatchIgnoreCase, StringComparison.OrdinalIgnoreCase);
            vv.Count().IsEqualTo(2);

            var vvv = UberFlatter.Flat("foobarname", u, MatchIgnoreCase, StringComparison.OrdinalIgnoreCase);
            vvv.Count().IsEqualTo(2);
        }

        [Test]
        public void UnflatTest()
        {
            var u = new Unflat();
            var es = UberFlatter.Unflat("FooBarName", u, Match);
            es.Count().IsEqualTo(2);

            UberFlatter.Unflat("FooBarName", u).Count().IsEqualTo(2);
        }

        [Test]
        public void UnflatTestIgnoreCase()
        {
            var u = new Unflat();
            var es = UberFlatter.Unflat("foobarname", u, MatchIgnoreCase, StringComparison.OrdinalIgnoreCase);
            es.Count().IsEqualTo(2);

            UberFlatter.Unflat("foobarname", u, (upn, pi) => upn.Equals(pi.Name, StringComparison.OrdinalIgnoreCase), StringComparison.OrdinalIgnoreCase).Count().IsEqualTo(2);
        }

        [Test]
        public void GetTrails()
        {
            var l = TrailFinder.GetTrails("TheFooBarName", typeof(Unflat).GetProperties().Single(o => o.Name == "The"), Match, new List<string>(), StringComparison.Ordinal).ToList();
            l.Count.IsEqualTo(2);
        }

        [Test]
        public void GetAllTrails()
        {
            var l = TrailFinder.GetTrails("The2FooBarName", typeof(Unflat).GetProperties(), Match, StringComparison.Ordinal);
            l.Count().IsEqualTo(2);
        }
        
        [Test]
        public void Speed()
        {
            var w = new Stopwatch();

            w.Start();
            var f = "hello";
            for (int i = 0; i < 1000; i++)
            {
                f = string.Format("{0}.{1}", f, "hello");
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);

            w.Reset(); w.Start();
            var s = "hello";
            for (int i = 0; i < 1000; i++)
            {
                s = s + "." + "hello";
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);

            w.Reset(); w.Start();
            var x = new[] { "hello" };
            for (int i = 0; i < 1000; i++)
            {
                x = x.Concat(new[] { "hello" }).ToArray();
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);

            w.Reset(); w.Start();
            var z = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                var zz = new List<string> { "hello" };
                z.AddRange(zz);
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);
        }

        [Test]
        public void S2()
        {
            var w = new Stopwatch();
            w.Start();
            var f = "hello";
            for (int i = 0; i < 10000; i++)
            {
                var b = f == string.Format("{0}{1}", string.Empty, "hello");
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);


            w.Reset(); w.Start();
            var z = "hello";
            for (int i = 0; i < 10000; i++)
            {
                var b = z == string.Empty + "hello";
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);


            w.Reset(); w.Start();
            var n = "hello";
            for (int i = 0; i < 10000; i++)
            {
                var b = n == null + "hello";
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);

            w.Reset(); w.Start();
            var x = "hello";
            for (int i = 0; i < 10000; i++)
            {
                var b = x == "hello";
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);

            w.Reset(); w.Start();
            var y = "hello";
            for (int i = 0; i < 10000; i++)
            {
                var b = y.Equals("hello");
            }
            w.Stop();
            Console.Out.WriteLine(w.Elapsed);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using Omu.ValueInjecter;

using Tests.Injections;
using Tests.SampleTypes;

namespace Tests
{
    public class Cloning
    {
        [Test]
        public void Test()
        {
            var o = new FooForClone
                        {
                            Name = "foo",
                            Number = 12,
                            NullInt = 16,
                            F1 = new FooForClone { Name = "foo one" },
                            Foos = new List<FooForClone>
                                       {
                                           new FooForClone {Name = "j1"},
                                           new FooForClone {Name = "j2"},
                                       },
                            FooArr = new FooForClone[]
                                         {
                                             new FooForClone {Name = "a1"},
                                             new FooForClone {Name = "a2"},
                                             new FooForClone {Name = "a3"},
                                         },
                            IntArr = new[] { 1, 2, 3, 4, 5 },
                            Ints = new[] { 7, 8, 9 },
                        };

            var clone = new FooForClone().InjectFrom<CloneInjection>(o) as FooForClone;

            Assert.AreEqual(o.Name, clone.Name);
            Assert.AreEqual(o.Number, clone.Number);
            Assert.AreEqual(o.NullInt, clone.NullInt);
            Assert.AreEqual(o.IntArr, clone.IntArr);
            Assert.AreEqual(o.Ints, clone.Ints);

            Assert.AreNotEqual(o.F1, clone.F1);
            Assert.AreNotEqual(o.Foos, clone.Foos);
            Assert.AreNotEqual(o.FooArr, clone.FooArr);

            //Foo F1
            Assert.AreEqual(o.F1.Name, clone.F1.Name);

            //Foo[] FooArr
            Assert.AreEqual(o.FooArr.Length, clone.FooArr.Length);
            Assert.AreNotEqual(o.FooArr[0], clone.FooArr[0]);
            Assert.AreEqual(o.FooArr[0].Name, clone.FooArr[0].Name);

            //IEnumerable<Foo> Foos
            Assert.AreEqual(o.Foos.Count(), clone.Foos.Count());
            Assert.AreNotEqual(o.Foos.First(), clone.Foos.First());
            Assert.AreEqual(o.Foos.First().Name, clone.Foos.First().Name);
        }
    }
}
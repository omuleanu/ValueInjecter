using System.Collections.Generic;

namespace Tests.SampleTypes
{
    public class FooForClone
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public int? NullInt { get; set; }
        public FooForClone F1 { get; set; }
        public IEnumerable<FooForClone> Foos { get; set; }
        public FooForClone[] FooArr { get; set; }
        public int[] IntArr { get; set; }
        public IEnumerable<int> Ints { get; set; }
    }
}
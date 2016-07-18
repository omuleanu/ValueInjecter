using System;

namespace Tests.SampleTypes
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime RegDate { get; set; }

        public string Prop1 { get; set; }

        public string Prop2
        {
            set
            {
                FirstName = value;
            }
        }
    }
}

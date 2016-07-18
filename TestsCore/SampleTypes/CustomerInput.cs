namespace Tests.SampleTypes
{
    public class CustomerInput
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RegDate { get; set; }

        public string FullName { get; set; }

        public string Prop1
        {
            get
            {
                return FirstName;
            }
        }

        public string Prop2 { get; set; }
    }
}
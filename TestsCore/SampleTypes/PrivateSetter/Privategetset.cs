namespace Tests.SampleTypes.PrivateSetter
{
    public class Privategetset : PrivategetsetBase
    {
        public string Pset { get; private set; }

        public string Pget { private get; set; }
    }
}
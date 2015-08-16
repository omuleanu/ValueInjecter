namespace Tests.SampleTypes.PrivateSetter
{
    public class PrivategetsetBase
    {
        public string BasePset { get; private set; }

        public string BasePget { private get; set; }
    }
}
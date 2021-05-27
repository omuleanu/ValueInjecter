using System;

using NUnit.Framework;

using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Tests.Injections;
using Tests.Misc;
using Tests.SampleTypes;

namespace Tests
{
    [TestFixture]
    public class LoopInjectionTests
    {
        [Test]
        public void ShouldIgnoreProperties()
        {
            var customer = GetCustomer();
            var c1 = new Customer();
            c1.InjectFrom(new LoopInjection(new[] { "FirstName" }), customer);
            
            Assert.IsNull(c1.FirstName);
            Assert.AreEqual(customer.LastName, c1.LastName);
        }

        private static Customer GetCustomer()
        {
            return new Customer { FirstName = "Art", LastName = "Vandelay", Id = 123, RegDate = DateTime.UtcNow };
        }
    }
}
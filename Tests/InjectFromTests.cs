using System;
using NUnit.Framework;
using Omu.ValueInjecter;
using Tests.SampleTypes;

namespace Tests
{
    [TestFixture]
    public class InjectFromTests
    {
        [Test]
        public void InjectStronglyTyped()
        {
            var c1 = new Customer();
            var c2 = c1.InjectFrom(GetCustomer());

            Assert.IsTrue(c1 == c2);
        }

        private static Customer GetCustomer()
        {
            return new Customer { FirstName = "Art", LastName = "Vandelay", Id = 123, RegDate = DateTime.UtcNow };
        }
    }
}
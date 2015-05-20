using System;
using System.ComponentModel;
using System.Diagnostics;

using NUnit.Framework;

using Omu.ValueInjecter;

using Tests.Injections;
using Tests.SampleTypes;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class MapperTests
    {
        [SetUp]
        public void Start()
        {
            Mapper.Reset();
        }

        [Test]
        public void PerformanceCheck()
        {
            const int Iterations = 10000;
            var customer = GetCustomer();

            Mapper.AddMap<Customer, CustomerInput>(src =>
                {
                    var res = new CustomerInput();
                    res.InjectFrom(src);
                    
                    //res.RegDate = src.RegDate.ToShortDateString();
                    //res.RegDate = src.RegDate.tos;
                    //res.FirstName = src.FirstName;
                    //res.LastName = src.LastName;
                    //res.Id = src.Id;
                    return res;
                });
            
            var w = new Stopwatch();

            w.Start();
            var customerInput = Mapper.Map<CustomerInput>(customer);
            w.Stop();

            Console.WriteLine("{0} one", w.Elapsed);

            w.Reset();
            w.Start();
            for (var i = 0; i < Iterations; i++)
            {
                Mapper.Map<Customer, CustomerInput>(customer);
            }
            w.Stop();
            Console.WriteLine("{0} <T,T>", w.Elapsed);

            w.Reset();
            w.Start();
            for (var i = 0; i < Iterations; i++)
            {
                Mapper.Map<CustomerInput>(customer);
            }
            w.Stop();
            Console.WriteLine("{0} loop", w.Elapsed);

            w.Reset();
            w.Start();
            Mapper.Map<CustomerInput>(customer);
            w.Stop();

            Console.WriteLine("{0} one", w.Elapsed);

            w.Reset();
            w.Start();
            for (int i = 0; i < Iterations; i++)
            {
                var cc = new Customer();
                cc.RegDate = customer.RegDate;
                cc.FirstName = customer.FirstName;
                cc.LastName = customer.LastName;
                cc.Id = customer.Id;
            }
            w.Stop();
            Console.WriteLine("{0} manual", w.Elapsed);
        }

        [Test]
        public void UseCustomDefaultMap()
        {
            Mapper.DefaultMap = (src, type, tag) =>
                {
                    var res = Activator.CreateInstance(type);
                    res.InjectFrom(src);

                    if (src.GetType() == typeof(Customer) && res.GetType() == typeof(CustomerInput))
                    {
                        res.InjectFrom<DateToStrInjection>(src);
                    }

                    return res;
                };

            var customer = GetCustomer();

            var ci = Mapper.Map<CustomerInput>(customer);
            Assert.AreEqual(customer.FirstName, ci.FirstName);
            Assert.AreEqual(customer.RegDate.ToShortDateString(), ci.RegDate);

            var ci2 = Mapper.Map<CustomerInput>(new { RegDate = DateTime.Now });
            Assert.IsNull(ci2.RegDate);
        }
       
        [Test]
        public void ShouldUseTag()
        {
            var customer = GetCustomer();

            Mapper.AddMap<Customer, Customer>((src, tag) =>
                {
                    var t1 = (Tag1)tag;
                    var res = new Customer { LastName = t1.P1 };
                    res.Id = src.Id;
                    res.FirstName = src.FirstName + t1.P1;
                    return res;
                });

            const string P1 = "abc";
            var c1 = Mapper.Map<Customer, Customer>(customer, new Tag1 { P1 = P1 });
            var c2 = Mapper.Map<Customer>(customer, new Tag1 { P1 = P1 });

            c1.InjectFrom<AreEqual>(c2);

            Assert.AreEqual(customer.Id, c1.Id);
            Assert.AreEqual(customer.FirstName + P1, c1.FirstName);
            Assert.AreEqual(P1, c1.LastName);
        }

        [Test]
        public void ShouldMapByDefault()
        {
            var customer = GetCustomer();

            var c1 = Mapper.Map<Customer, Customer>(customer);
            var c2 = Mapper.Map<Customer>(customer);

            c1.InjectFrom<AreEqual>(c2);

            Assert.AreEqual(customer.Id, c1.Id);
            Assert.AreEqual(customer.FirstName, c1.FirstName);
            Assert.AreEqual(customer.LastName, c1.LastName);
        }

        [Test]
        public void ShouldMapWithAddedMap()
        {
            var customer = GetCustomer();

            Mapper.AddMap<Customer, Customer>(src =>
                {
                    var res = new Customer();
                    res.Id = src.Id;
                    res.FirstName = src.FirstName;
                    res.LastName = src.LastName;
                    return res;
                });

            var c1 = Mapper.Map<Customer, Customer>(customer);
            var c2 = Mapper.Map<Customer>(customer);

            c1.InjectFrom<AreEqual>(c2);

            Assert.AreEqual(customer.Id, c1.Id);
            Assert.AreEqual(customer.FirstName, c1.FirstName);
            Assert.AreEqual(customer.LastName, c1.LastName);
        }

        [Test]
        public void ShouldMapWithNewAddedMap()
        {
            var customer = GetCustomer();
            Mapper.AddMap<Customer, CustomerInput>((src, tag) =>
            {
                var res = new CustomerInput();
                return res;
            });

            Mapper.AddMap<Customer, CustomerInput>(src =>
            {
                var res = new CustomerInput();
                res.Id = src.Id;
                res.FirstName = src.FirstName;
                res.LastName = src.LastName;
                return res;
            });

            var c1 = Mapper.Map<Customer, CustomerInput>(customer);
            var c2 = Mapper.Map<CustomerInput>(customer);

            c1.InjectFrom<AreEqual>(c2);

            Assert.AreEqual(customer.Id, c1.Id);
            Assert.AreEqual(customer.FirstName, c1.FirstName);
            Assert.AreEqual(customer.LastName, c1.LastName);
        }

        [Test]
        public void ShouldMapProxy()
        {
            var customerProxy = new CustomerProxy { FirstName = "c1", ProxyName = "proxy1", RegDate = DateTime.Now };

            Mapper.AddMap<Customer, CustomerInput>(src =>
                { 
                    var res = new CustomerInput();
                    res.InjectFrom(src);
                    res.RegDate = src.RegDate.ToShortDateString();
                    return res;
                });

            var input = Mapper.Map<Customer, CustomerInput>(customerProxy);

            Assert.AreEqual(customerProxy.RegDate.ToShortDateString(), input.RegDate);
            Assert.AreEqual(customerProxy.FirstName, input.FirstName);
        }

        private static Customer GetCustomer()
        {
            var customer = new Customer { FirstName = "Art", LastName = "Vandelay", Id = 123, RegDate = DateTime.UtcNow };
            return customer;
        }
    }
}
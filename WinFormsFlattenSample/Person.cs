using System;

namespace WinFormsFlattenSample
{
    public class Person
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public Address HomeAddress { get; set; }
        public Document DriverLicense { get; set; }
    }

    public class Document
    {
        public string Number { get; set; }
        public IssueOffice Office { get; set; }
    }

    public class IssueOffice
    {
        public string Name { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
    }

    public class PersonFlat
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string DriverLicenseOfficeName { get; set; }
        public string DriverLicenseOfficeAddressStreet { get; set; }
        public string DriverLicenseOfficeAddressHouseNumber { get; set; }
        public string DriverLicenseNumber { get; set; }
    }
}

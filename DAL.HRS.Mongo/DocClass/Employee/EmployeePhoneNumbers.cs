using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EmployeePhoneNumber 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef PhoneType { get; set; }
        public string PhoneNumber { get; set; }

        public PropertyValueRef Country { get; set; }

        public EmployeePhoneNumber()
        {
        }

        public EmployeePhoneNumber(string phoneType, string phoneNumber, string country)
        {
            PhoneType = PropertyBuilder
                .CreatePropertyValue("PhoneType", "Type of phone number", phoneType, "Type of phone number")
                .AsPropertyValueRef();
            PhoneNumber = phoneNumber;
            Country = PropertyBuilder
                .CreatePropertyValue("CountryPhone", "Country of Phone number", country, "Country of phone number")
                .AsPropertyValueRef();
        }

    }
}


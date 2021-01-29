using System;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class EmployeePhoneNumber
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef PhoneType { get; set; }
        public string PhoneNumber { get; set; }

        public PropertyValueRef Country { get; set; }

    }
}


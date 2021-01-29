using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.iMetal.Core.Models
{
    public class iMetalContactModel
    {
        public int SqlId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneFax { get; set; }
        public string PhoneOffice { get; set; }
        public string EmailBusiness { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }

    }
}

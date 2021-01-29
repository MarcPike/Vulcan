using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.iMetal.Core.Models
{
    public class iMetalCompanyAddressModel
    {
        public string Coid { get; set; }
        public int AddressId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string CountryName { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public int SqlId { get; set; }

        public int HashCode
        {
            get
            {
                var value = Address + Town + County + PostCode;
                return value.GetHashCode();
            }
        }


        //public iMetalCompanyAddressModel(Vulcan.IMetal.Context.Company.CompanySubAddress address, string coid)
        //{
        //    Coid = coid;
        //    Name = address.Name;
        //    AddressId = address.Id;
        //    Address = address.Address.Address1;
        //    Town = address.Address.Town;
        //    County = address.Address.County;
        //    PostCode = address.Address?.Postcode;
        //    CountryName = address.Address?.CountryCode?.Description;
        //    Code = address.Code;
        //    Status = address.Status;
        //    SqlId = address.Id;
        //}

        //public iMetalCompanyAddressModel(Vulcan.IMetal.Context.Company.Address address, string coid, string name)
        //{
        //    Coid = coid;
        //    Name = name;
        //    AddressId = address.Id;
        //    Address = address.Address1;
        //    Town = address.Town;
        //    County = address.County;
        //    PostCode = address.Postcode;
        //    CountryName = address.CountryCode?.Description;
        //    Code = string.Empty;
        //    Status = address.Status;
        //    SqlId = address.Id;

        //}

        //public iMetalCompanyAddressModel(Vulcan.IMetal.Context.PurchaseOrders.CompanySubAddress address, string coid)
        //{
        //    Coid = coid;
        //    Name = address.Name;
        //    AddressId = address.Id;
        //    Address = address.Address.Address1;
        //    Town = address.Address.Town;
        //    County = address.Address.County;
        //    PostCode = address.Address?.Postcode;
        //    CountryName = address.Address?.CountryCode?.Description;
        //    Code = address.Code;
        //    Status = address.Status;
        //    SqlId = address.Id;

        //}

        //public iMetalCompanyAddressModel(Vulcan.IMetal.Context.PurchaseOrders.Address address, string coid, string name)
        //{
        //    Coid = coid;
        //    Name = name;
        //    AddressId = address.Id;
        //    Address = address.Address1;
        //    Town = address.Town;
        //    County = address.County;
        //    PostCode = address.Postcode;
        //    CountryName = address.CountryCode?.Description;
        //    Code = string.Empty;
        //    Status = address.Status;
        //    SqlId = address.Id;
        //}

        public iMetalCompanyAddressModel()
        {

        }
    }
}

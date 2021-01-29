using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public static class CompanyResolver
    {
        public static readonly CompanyRefreshFromSourceSystem CompanyRefresher = new CompanyRefreshFromSourceSystem();
        private static readonly CompanySaveShippingAddress CompanySaveShippingAddress = new CompanySaveShippingAddress();
        private static readonly CompanyRemoveDuplicateAddresses CompanyRemoveDuplicateAddresses = new CompanyRemoveDuplicateAddresses();

        public static  Company Execute(Company company)
        {

            CompanyRefresher.Refresh(company);
            CompanyRemoveDuplicateAddresses.Execute(company);

            company.SaveToDatabase();
            return company;
        }


        public static Address SaveNewAddressIfNecessary(Company company, Address shipToAddress)
        {
            if (shipToAddress == null) return shipToAddress;
            CompanyRefresher.Refresh(company);
            CompanyRemoveDuplicateAddresses.Execute(company);
            CompanySaveShippingAddress.Execute(company, shipToAddress);
            company.SaveToDatabase();

            var shipToAddressFound = company.Addresses.SingleOrDefault(x => x.HashCode == shipToAddress.HashCode);
            return shipToAddressFound;
        }


        public static bool IsShipToAddressValid(int hashcode, CompanyRef companyRef )
        {
            try
            {
                var companyRefresher = new CompanyRefreshFromSourceSystem();
                var validAddresses = companyRefresher.GetList(companyRef);
                if (hashcode == validAddresses.PrimaryAddress.HashCode) return true;

                if (validAddresses.OtherAddresses.Any(x => x.HashCode == hashcode)) return true;
            }
            catch (Exception)
            {
                return false;
            }


            return false;
        }

        public static (CompanyRef CompanyRef, List<Address> Addresses) GetAllCompanyAddresses(CompanyRef companyRef)
        {
            var company = companyRef.AsCompany();
            CompanyRefresher.Refresh(company);

            return (company.AsCompanyRef(), company.Addresses);
        }

    }
}
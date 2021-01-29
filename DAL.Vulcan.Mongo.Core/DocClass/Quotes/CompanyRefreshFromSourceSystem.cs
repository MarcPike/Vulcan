using System;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class CompanyRefreshFromSourceSystem
    {
        public void Refresh(Company company)
        {
            var coid = company.Branch;
            if (coid == "USA") coid = "INC";

            var companySearchResult = CompanyQuery.GetForId(coid,company.SqlId,true).Result;

            if (companySearchResult == null) throw new Exception("Company does not exist in iMetal");


            var primaryAddress = new Address(companySearchResult.PrimaryAddress, AddressType.Primary);

            primaryAddress.ExternalCode = companySearchResult.Addresses
                .SingleOrDefault(x => x.HashCode == primaryAddress.HashCode)?.Code;

            var otherAddresses = (companySearchResult.Addresses == null) ? new List<Address>() : companySearchResult.Addresses.Select(x => new Address(x, AddressType.Other));

            foreach (var otherAddress in otherAddresses)
            {
                if (String.IsNullOrEmpty(otherAddress.ExternalCode))
                {
                    var otherAddressRaw =
                        companySearchResult.Addresses.SingleOrDefault(x => x.HashCode == otherAddress.HashCode);
                    otherAddress.ExternalCode = otherAddressRaw.Code;

                }
            }

            company.IsActive = companySearchResult.CompanyStatusCode == "A";

            company.DefaultSalesGroupCode = companySearchResult.DefaultSalesGroupCode;
            company.DefaultCurrencyCode = companySearchResult.CurrencyCode;

            company.OrderClassificationCode = companySearchResult.OrderClassificationCode;
            company.OrderClassificationDescription = companySearchResult.OrderClassificationDescription;

            //company.PrimaryAddress = primaryAddress;

            company.AddressLine1 = primaryAddress.AddressLine1;
            company.AddressLine2 = primaryAddress.AddressLine2;
            company.City = primaryAddress.City;
            company.StateProvince = primaryAddress.StateProvince;
            company.PostalCode = primaryAddress.PostalCode;
            company.County = primaryAddress.County;
            company.Country = primaryAddress.Country;

            //company.Addresses.Clear();
            //company.Addresses.Add(primaryAddress);

            // Remove any invalid primary addresses
            var invalidPrimaryAddresses = company.Addresses.Where(x => x.Type == AddressType.Primary && x.HashCode != primaryAddress.HashCode).ToList();
            foreach (var invalidPrimaryAddress in invalidPrimaryAddresses)
            {
                company.Addresses.Remove(invalidPrimaryAddress);
            }

            //// Add primary if needed
            //if (!company.Addresses.Any(x => x.Type == AddressType.Primary && x.HashCode == primaryAddress.HashCode))
            //{
            //    company.Addresses.Add(primaryAddress);
            //}

            foreach (var address in otherAddresses)
            {
                var existingAddress = company.Addresses.FirstOrDefault(x => x.HashCode == address.HashCode);
                if (existingAddress == null)
                {
                    
                    company.Addresses.Remove(existingAddress);
                }
                else
                {
                    existingAddress.ExternalCode = address.ExternalCode;
                }
            }

            // Remove any company.Addresses that are null

            //foreach (var nullAddress in company.Addresses.Where(x=>x == null))
            //{
            //    company.Addresses.Remove(nullAddress);
            //}

            // Add any new iMetal addresses we don't already have
            foreach (var companyAddressModel in companySearchResult.Addresses)
            {
                var newAddress = new Address(companyAddressModel, AddressType.Other);
                if (company.Addresses.All(x => x.HashCode != newAddress.HashCode))
                {
                    company.Addresses.Add(newAddress);
                }
            }

            // Remove any addresses that we no longer have other than the one's we created
            foreach (var companyAddress in company.Addresses.ToList().Where(x => x.Type != AddressType.ShippingNew))
            {
                if (otherAddresses.All(x => x.HashCode != companyAddress.HashCode))
                {
                    company.Addresses.Remove(companyAddress);
                }
            }

            // Remove any Addresses that were originally new addresses added in Vulcan that now exist in iMetal
            foreach (var address in company.Addresses.Where(x => x.Type == AddressType.ShippingNew).ToList())
            {
                if (company.Addresses.Any(x => x.HashCode == address.HashCode && x.Type != AddressType.ShippingNew))
                {
                    company.Addresses.Remove(address);
                }
            }

            company.Name = companySearchResult.Name;
            company.ShortName = companySearchResult.ShortName;

            // Make sure we have good Guids (pun intended)
            foreach (var companyAddress in company.Addresses)
            {
                if (companyAddress.Id == Guid.Empty) companyAddress.Id = Guid.NewGuid();
            }
        }

        public void RefreshCompanyOrderClassification(Company company)
        {
            var coid = company.Branch;
            if (coid == "USA") coid = "INC";


            var companySearchResult = CompanyQuery.GetForId(coid,company.SqlId).Result;

            if (companySearchResult == null) throw new Exception("Company does not exist in iMetal");

            if ((company.OrderClassificationCode != companySearchResult.OrderClassificationCode) ||
                (company.OrderClassificationDescription != companySearchResult.OrderClassificationDescription))
            {
                company.OrderClassificationCode = companySearchResult.OrderClassificationCode;
                company.OrderClassificationDescription = companySearchResult.OrderClassificationDescription;
                Company.Helper.Upsert(company);
            }


        }

        public (Address PrimaryAddress, List<Address> OtherAddresses) GetList(CompanyRef companyRef)
        {
            var company = companyRef.AsCompany();

            var coid = company.Branch;
            if (coid == "USA") coid = "INC";

            var companySearchResult = CompanyQuery.GetForId(coid,company.SqlId).Result;

            if (companySearchResult == null) throw new Exception("Company does not exist in iMetal");

            var primaryAddress = new Address(companySearchResult.PrimaryAddress, AddressType.Primary);
            var otherAddresses = companySearchResult.Addresses.Select(x => new Address(x, AddressType.Other)).ToList();

            return (primaryAddress, otherAddresses);
        }

    }
}
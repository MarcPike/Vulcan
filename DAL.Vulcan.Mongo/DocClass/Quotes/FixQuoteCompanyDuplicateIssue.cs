using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public static class FixQuoteCompanyDuplicateIssue
    {
        public static void Execute(CrmQuote quote)
        {

            var shipToAddressAddress1 = quote.ShipToAddress.AddressLine1;
            var shipToAddressPostalCode = quote.ShipToAddress.PostalCode;

            var company = quote.Company.AsCompany();
            CompanyResolver.Execute(company);
            quote.ShipToAddress = company.Addresses.LastOrDefault(x => x.AddressLine1 == shipToAddressAddress1 && x.PostalCode == shipToAddressPostalCode) ??
                                  company.Addresses.LastOrDefault(x => x.Type == AddressType.Shipping) ??
                                  company.Addresses.LastOrDefault(x => x.Type == AddressType.Other) ??
                                  company.Addresses.LastOrDefault(x => x.Type == AddressType.Primary) ??
                                  company.Addresses.LastOrDefault() ??
                                  new Address();
            quote.Addresses = company.Addresses.ToList();
            quote.SaveToDatabase();

        }
    }
}
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Queries;
using Vulcan.IMetal.Queries.Companies;
using Vulcan.IMetal.Results;

namespace DAL.Vulcan.Mongo.DocClass.Companies
{
    public class CompanyDefaults: BaseDocument
    {
        public static MongoRawQueryHelper<CompanyDefaults> Helper = new MongoRawQueryHelper<CompanyDefaults>();
        public string Coid { get; set; } 
        public string CompanyId { get; set; }
        public string ContactId { get; set; } = string.Empty;
        public Guid? ShippingAddressId { get; set; } = Guid.Empty;
        public string PaymentTerm { get; set; } = "30 Days From Invoice Date";
        public string FreightTerm { get; set; } = "FCA Free Carrier";
        public string SalesGroupCode { get; set; } = string.Empty;
        public int ValidityDays { get; set; } = 7;
        public CustomerUom CustomerUom { get; set; } = Models.CustomerUom.Inches;
        public string LeadTime { get; set; } = string.Empty;
        public string DisplayCurrency { get; set; } = string.Empty;

        public string SalesPersonNotes { get; set; } = string.Empty;
        public string CustomerNotes { get; set; } = string.Empty;

        public string ExcelTemplateId { get; set; } = string.Empty;

        public CompanyDefaults()
        {
        }

        public static void ApplyCompanyDefaultsToQuote(CrmQuote quote)
        {
            var company = quote.Company.AsCompany();

            if (quote.IsProspect) return;

            var isNorway = quote.Team.Name.Contains("Norway");

            var companyDefaults = CompanyDefaults.GetCompanyDefaults(quote.Coid, company, isNorway);

             if (!String.IsNullOrEmpty(companyDefaults.ContactId))
            {
                var contactId = ObjectId.Parse(companyDefaults.ContactId);

                var contact = new RepositoryBase<Contact>().AsQueryable().SingleOrDefault(x=> x.Id == contactId);
                if (contact != null)
                {
                    quote.Contact = contact.AsContactRef();
                }
            }

            if ((companyDefaults.ShippingAddressId != null) &&  (companyDefaults.ShippingAddressId != Guid.Empty))
            {
                quote.ShipToAddress =
                    company.Addresses.SingleOrDefault(x => x.Id == companyDefaults.ShippingAddressId) ??
                    quote.ShipToAddress;
            }
            else
            {
                quote.Addresses = company.Addresses;
                quote.ShipToAddress = company.Addresses.FirstOrDefault(x => x.Type == AddressType.Shipping) ??
                                      company.Addresses.FirstOrDefault(x => x.Type == AddressType.ShippingNew) ??
                                      company.Addresses.FirstOrDefault();
            }

            if (!String.IsNullOrEmpty(companyDefaults.SalesGroupCode))
            {
                quote.SalesGroupCode = companyDefaults.SalesGroupCode;
            }

            if (!string.IsNullOrEmpty(companyDefaults.DisplayCurrency))
            {
                quote.DisplayCurrency = companyDefaults.DisplayCurrency;
            }
            else if (isNorway)
            {
                quote.DisplayCurrency = "NOK";
            }

            if (!String.IsNullOrEmpty(companyDefaults.PaymentTerm))
            {
                quote.PaymentTerm = companyDefaults.PaymentTerm;
            }

            if (!String.IsNullOrEmpty(companyDefaults.FreightTerm))
            {
                quote.FreightTerm = companyDefaults.FreightTerm;
            }

            if (!String.IsNullOrEmpty(companyDefaults.CustomerNotes))
            {
                quote.CustomerNotes = companyDefaults.CustomerNotes;
            }

            if (!String.IsNullOrEmpty(companyDefaults.SalesPersonNotes))
            {
                quote.SalesPersonNotes = companyDefaults.SalesPersonNotes;
            }

            quote.ValidityDays = companyDefaults.ValidityDays;

        }

        public void ApplyCompanyDefaultsToQuoteItem(CrmQuoteItem quoteItem)
        {
            quoteItem.LeadTime = LeadTime;
        }

        public static CompanyDefaults GetCompanyDefaults(string coid, string companyId, bool isNorway)
        {
            var company = new RepositoryBase<Company>().Find(companyId);
            return GetCompanyDefaults(coid, company, isNorway);
        }

        public static CompanyDefaults ClearAndRefreshCompanyDefaults(string coid, string companyId, bool isNorway)
        {
            var rep = new RepositoryBase<CompanyDefaults>();
            var companyDefaults = GetCompanyDefaults(coid, companyId, isNorway);
            rep.RemoveOne(companyDefaults);
            return GetCompanyDefaults(coid, companyId, isNorway);
        }

        public static CompanyDefaults GetCompanyDefaults(string coid, Company company, bool isNorway)
        {
            var companyId = company.Id.ToString();
            var rep = new RepositoryBase<CompanyDefaults>();
            var companyDefaults = rep.AsQueryable().FirstOrDefault(x => x.Coid == coid && x.CompanyId == companyId);

            var validCurrencies = new List<string>() { "CAD", "USD", "GBP", "EUR", "AED", "MYR", "SGD", "NOK" };

            var iMetalQuery = new QueryCompany(coid);
            CompanySearchResult iMetalCompany = iMetalQuery.GetForId(company.SqlId); ;

            if (companyDefaults == null)
            {
                companyDefaults = new CompanyDefaults()
                {
                    Coid =  coid,
                    CompanyId = company.Id.ToString(),
                };

                OverrideDefaultFreightTermForSinMsaDub();
                if (isNorway)
                {
                    OverrideDefaultFreightTermForNorway(companyDefaults);
                }

                if (coid == "EUR")
                {
                    companyDefaults.CustomerUom = Models.CustomerUom.PerPiece;
                }

                if (company.Contacts.Any())
                {
                    companyDefaults.ContactId = company.Contacts.First().Id;
                }

                GetDefaultSalesGroupCode();
                GetDefaultCurrency();

                rep.Upsert(companyDefaults);
            }
            else
            {

                var salesGroupModified = GetDefaultSalesGroupCode();
                var defaultCurrencyModified = GetDefaultCurrency();
                if (salesGroupModified || defaultCurrencyModified) rep.Upsert(companyDefaults);

            }

            return companyDefaults;

            void OverrideDefaultFreightTermForSinMsaDub()
            {
                var exWorksCoid = new List<string>()
                {
                    "SIN",
                    "MSA",
                    "DUB"
                };

                if (exWorksCoid.Contains(coid))
                {
                    companyDefaults.FreightTerm = "EXW  Ex Works";
                }
            }

            bool GetDefaultCurrency()
            {
                var modified = false;
                if (iMetalCompany == null)
                {
                    iMetalCompany = iMetalQuery.GetForId(company.SqlId);
                }

                if (iMetalCompany != null)
                {
                    if (string.IsNullOrEmpty(companyDefaults.DisplayCurrency))
                    {
                        if (validCurrencies.Any(x => x == iMetalCompany.CurrencyCode))
                        {
                            companyDefaults.DisplayCurrency = iMetalCompany.CurrencyCode;
                            modified = true;
                        }
                        else
                        {
                            companyDefaults.DisplayCurrency = string.Empty;
                            modified = true;
                        }
                    }

                    if (validCurrencies.All(x => x != companyDefaults.DisplayCurrency))
                    {
                        companyDefaults.DisplayCurrency = string.Empty;
                        modified = true;
                    }
                }

                return modified;
            }

            bool GetDefaultSalesGroupCode()
            {

                if (iMetalCompany == null)
                {
                    iMetalCompany = iMetalQuery.GetForId(company.SqlId);
                }
                

                var modified = false;
                if (string.IsNullOrEmpty(companyDefaults.SalesGroupCode))
                {
                    if (string.IsNullOrEmpty(iMetalCompany?.DefaultSalesGroupCode))
                    {
                        SalesGroup.UpdateListForCoid(coid);
                        var salesGroups = new RepositoryBase<SalesGroup>().AsQueryable().Where(x => x.Coid == coid && x.IsActive).ToList();
                        if (salesGroups.Any())
                        {
                            companyDefaults.SalesGroupCode = salesGroups.First().Code;
                            modified = true;
                        }
                    }
                    else
                    {
                        companyDefaults.SalesGroupCode = iMetalCompany.DefaultSalesGroupCode;
                        modified = true;
                    }
                }
                return  modified;
            }
        }

        private static void OverrideDefaultFreightTermForNorway(CompanyDefaults companyDefaults)
        {
            companyDefaults.FreightTerm = "EXW Ex Works";
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class QuotesWithManaulShippingAddress
    {
        public class CompanyAddressResult
        {
            public TeamRef Team { get; set; }
            public PipelineStatus Status { get; set; }
            public ExportStatus ExportStatus { get; set; }
            public bool IsNewAddress { get; set; }
            public int Count { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
        }

        [Test]
        public void EvaluateAllQuotes()
        {
            var results = Execute();
            Console.WriteLine($"Team\t\t\tStatus\t\tExportStatus\t\tCustom Address\t\tCount");
            foreach (var companyAddressResult in results.OrderBy(x=>x.Team.Name).ThenBy(x=>x.Status).ThenBy(x=>x.ExportStatus).ThenBy(x=>x.IsNewAddress))
            {
                Console.WriteLine($"{companyAddressResult.Team.Name}\t\t\t{companyAddressResult.Status}\t\t{companyAddressResult.ExportStatus}\t\t{companyAddressResult.IsNewAddress}\t\t{companyAddressResult.Count}");
            }
        }

        private List<CompanyAddressResult> Execute()
        {
            var results = new List<CompanyAddressResult>();
            var repQuote = new RepositoryBase<CrmQuote>();
            foreach (var crmQuote in repQuote.AsQueryable().Where(x => x.Team != null && x.Company != null && x.ShipToAddress != null).ToList().OrderBy(x => x.QuoteId))
            {
                var team = crmQuote.Team;
                var status = crmQuote.Status;
                var exportStatus = crmQuote.ExportStatus;

                var isNewAddress =
                    CompanyResolver.IsShipToAddressValid(crmQuote.ShipToAddress.HashCode, crmQuote.Company);

                var counterRow = results.FirstOrDefault(x =>
                    x.Team.Id == team.Id && x.Status == status && x.ExportStatus == exportStatus &&
                    x.IsNewAddress == isNewAddress);
                if (counterRow == null)
                {
                    counterRow = new CompanyAddressResult()
                    {
                        Team = team,
                        Status = status,
                        ExportStatus = exportStatus,
                        IsNewAddress = isNewAddress,
                        Count = 0
                    };
                    results.Add(counterRow);
                }

                counterRow.Count++;
            }

            return results;

        }

        [Test]
        public void GetListOfCompanyAddressTypes()
        {
            Dictionary<string, int> addressTypes = new Dictionary<string, int>();
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();
            foreach (var company in repCompany.AsQueryable().ToList())
            {
                foreach (var companyAddress in company.Addresses)
                {
                    var key = companyAddress.Type.ToString();
                    int value;
                    if (addressTypes.TryGetValue(key, out value))
                    {
                        addressTypes[key] = value + 1;
                    }
                    else
                    {
                        addressTypes.Add(key,1);
                    }

                }
            }

            foreach (var addressType in addressTypes)
            {
                Console.WriteLine($"{addressType.Key} - {addressType.Value}");
            }
        }

        [Test]
        public void CompaniesWithCustomAddresses()
        {
            var rep = new RepositoryBase<Mongo.DocClass.Companies.Company>();
            foreach (var company in rep.AsQueryable().ToList())
            {

                var newAddresses = ModifyAllQuotesWithLatestAddressLogic(company);
                if (newAddresses.Any())
                {
                    Console.WriteLine($"{company.Name} has {newAddresses.Count} {AddressType.ShippingNew.ToString()} addresses");
                }
            }
        }

        private List<Address> ModifyAllQuotesWithLatestAddressLogic(Mongo.DocClass.Companies.Company company)
        {
            // Update Company with correct addresses
            CompanyResolver.Execute(company);

            var newShippingAddresses = new List<Address>();
            var repQuotes = new RepositoryBase<CrmQuote>();
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();
            var quotes = repQuotes.AsQueryable().Where(x =>
                x.Company != null && x.Company.Id == company.Id.ToString() && x.ShipToAddress != null);
            foreach (var crmQuote in quotes)
            {
                if (company.Addresses.All(x => x.HashCode != crmQuote.ShipToAddress.HashCode))
                {
                    crmQuote.ShipToAddress.Type = AddressType.ShippingNew;
                    if (crmQuote.ShipToAddress.Id == Guid.Empty)
                    {
                        crmQuote.ShipToAddress.Id = Guid.NewGuid();
                    }
                    repQuotes.Upsert(crmQuote);
                    if (company.Addresses.All(x => x.HashCode != crmQuote.ShipToAddress.HashCode))
                    {
                        company.Addresses.Add(crmQuote.ShipToAddress);
                        repCompany.Upsert(company);
                        newShippingAddresses.Add(crmQuote.ShipToAddress);
                    }
                }
            }

            if (quotes.Any())
            {
                Console.WriteLine($"Completed {company.ShortName} => {quotes.Count()} quotes");
            }

            return newShippingAddresses;
        }


    }
}

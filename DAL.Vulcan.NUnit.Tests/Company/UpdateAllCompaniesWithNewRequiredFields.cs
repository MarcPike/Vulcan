using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Test;
using Vulcan.IMetal.Models;
using Vulcan.IMetal.Queries.Companies;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class UpdateAllCompaniesWithNewRequiredFields
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void Execute()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var rep = new RepositoryBase<Mongo.DocClass.Companies.Company>();
            
            var companies = rep.AsQueryable().ToList();
            
            foreach (var company in companies.ToList())
            {
                //CompanyResolver.Execute(company);
                ModifyAllQuotesWithLatestAddressLogic(company);
                //Console.WriteLine(company.Name);
                //Console.WriteLine("Primary Address");
                //Console.WriteLine(ObjectDumper.Dump(company.Addresses.Single(x=>x.Type == AddressType.Primary)));
                //Console.WriteLine($"\tOther Addresses");
                //foreach (var address in company.Addresses.Where(x=>x.Type == AddressType.Other).ToList())
                //{
                //    Console.WriteLine(ObjectDumper.Dump(address));
                //}
            }
            stopWatch.Stop();

            Console.WriteLine("We are done! Elapsed Time: "+stopWatch.Elapsed);
        }

        private void ModifyAllQuotesWithLatestAddressLogic(Mongo.DocClass.Companies.Company company)
        {
            // Update Company with correct addresses
            CompanyResolver.Execute(company);

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
                    }
                }
            }

            if (quotes.Any())
            {
                Console.WriteLine($"Completed {company.ShortName} => {quotes.Count()} quotes");
            }

        }

    }

}

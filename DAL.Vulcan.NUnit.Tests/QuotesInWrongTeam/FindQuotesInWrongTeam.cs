using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.QuotesInWrongTeam
{
    [TestFixture]
    public class FindQuotesInWrongTeam
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var helperQuote = new HelperQuote();
            var allQuotes = CrmQuote.Helper.Find(x=>x.Team.Name == "Houston Sales" && x.SalesPerson.LastName == "Hinton").ToList();

            foreach (var crmQuote in allQuotes)
            {
                var quoteModel = new QuoteModel("vulcancrm", crmQuote.SalesPerson.UserId, crmQuote);

                helperQuote.SaveQuote(quoteModel);

                //try
                //{
                //var salesPersonTeam = crmQuote.SalesPerson.AsCrmUser().ViewConfig.Team;
                //if (salesPersonTeam.Id != crmQuote.Team.Id)
                //{
                //    Console.WriteLine($"{crmQuote.SalesPerson.FullName} is in Team: {salesPersonTeam.Name} but QuoteId: {crmQuote.QuoteId} is in {crmQuote.Team.Name}");
                //}

                //}
                //catch (Exception)
                //{
                //    Console.WriteLine($"   *** {crmQuote.QuoteId} has issues with getting SalesPerson");
                //    continue;
                //}
            }
        }

        [Test]
        public void FindACQuotesInCompanyDefaults()
        {

            
            var defaults = DAL.Vulcan.Mongo.DocClass.Companies.CompanyDefaults.Helper.
                Find(x => x.SalesGroupCode == "AC" && x.Coid == "INC").ToList();

            foreach (var cd in defaults)
            {
                var company = Mongo.DocClass.Companies.Company.Helper.FindById(cd.CompanyId);

                cd.SalesGroupCode = "";
                DAL.Vulcan.Mongo.DocClass.Companies.CompanyDefaults.Helper.Upsert(cd);

                Console.WriteLine($"{company.Code}-{company.Name} in {cd.Coid} did have SalesGroupCode == \"AC\" but default has been removed");
            }
        }

    }
}

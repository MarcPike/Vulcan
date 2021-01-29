using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Team_Tests
{
    [TestFixture]
    public class AssignTeamToQuotesAndPopulateReportDate
    {
        [Test]
        public void Execute()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var rep = new RepositoryBase<CrmQuote>();
            foreach (var crmQuote in rep.AsQueryable().ToList())
            {
                if (crmQuote.Team == null)
                {
                    var crmUser = crmQuote.SalesPerson.AsCrmUser();
                    crmQuote.Team = crmUser.ViewConfig.Team;
                }

                if (crmQuote.ReportDate == null)
                {
                    crmQuote.SetReportDate();
                }

                foreach (var crmQuoteItem in crmQuote.Items.Select(x=>x.AsQuoteItem()).ToList())
                {
                    crmQuoteItem.SetStockGradeForProducts();
                    crmQuoteItem.SaveToDatabase();
                }

                rep.Upsert(crmQuote);
            }
        }

        [Test]
        public void FixMissingLocation()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<CrmQuote>();
            foreach (var crmQuote in rep.AsQueryable().Where(x=>x.Team == null).ToList())
            {
                var crmUser = crmQuote.SalesPerson.AsCrmUser();
                crmQuote.Team = crmUser.ViewConfig.Team;

                rep.Upsert(crmQuote);
            }

        }

        [Test]
        public void FixMissingTeam()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<CrmQuote>();
            foreach (var crmQuote in rep.AsQueryable().Where(x => x.Team == null).ToList())
            {
                var crmUser = crmQuote.SalesPerson.AsCrmUser();
                crmQuote.Team = crmUser.ViewConfig.Team;
                rep.Upsert(crmQuote);
                Console.WriteLine($"Quote: {crmQuote.QuoteId} was updated");
            }

        }

    }
}

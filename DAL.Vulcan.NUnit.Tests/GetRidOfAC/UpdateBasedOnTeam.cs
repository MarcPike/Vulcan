using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.GetRidOfAC
{
    [TestFixture]
    class UpdateBasedOnTeam
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var quotes = CrmQuote.Helper.Find(x => x.Team.Name == "Louisiana Ragin Cajuns" && x.SalesGroupCode == "AC" && x.CreateDateTime >= DateTime.Parse("4/1/2020"))
                .ToList();
            foreach (var quote in quotes)
            {
                quote.SalesGroupCode = "LOU";
                quote.ModifiedDateTime = DateTime.Now;
                quote.ModifiedByUserId = "mpike";
                CrmQuote.Helper.Upsert(quote);
            }

            quotes = CrmQuote.Helper.Find(x => x.Team.Name == "Houston Sales" && x.SalesGroupCode == "AC" && x.CreateDateTime >= DateTime.Parse("4/1/2020"))
                .ToList();
            foreach (var quote in quotes)
            {
                quote.SalesGroupCode = "HOU";
                quote.ModifiedDateTime = DateTime.Now;
                quote.ModifiedByUserId = "mpike";
                CrmQuote.Helper.Upsert(quote);
            }

            quotes = CrmQuote.Helper.Find(x => x.Team.Name == "Oklahoma Sooner's" && x.SalesGroupCode == "AC" && x.CreateDateTime >= DateTime.Parse("4/1/2020"))
                .ToList();
            foreach (var quote in quotes)
            {
                quote.SalesGroupCode = "OKL";
                quote.ModifiedDateTime = DateTime.Now;
                quote.ModifiedByUserId = "mpike";
                CrmQuote.Helper.Upsert(quote);
            }
        }

        /*
            [Louisiana Ragin Cajuns] quotes: 3979
            [Houston Sales] quotes: 705
            [Oklahoma Sooner's] quotes: 8
        */

    }
}

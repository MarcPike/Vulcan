using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture]
    public class IsidroDemo
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void RajinCajunQuotes()
        {
            //var quotes = CrmQuote.Helper.Find(x => x.Team.Name == "Louisiana Ragin Cajuns").ToList();
            var filter = CrmQuote.Helper.FilterBuilder.Where(x => x.Team.Name == "Louisiana Ragin Cajuns") &
                         CrmQuote.Helper.FilterBuilder.Where(x => x.SalesPerson.FirstName == "Isidro");
            var project = CrmQuote.Helper.ProjectionBuilder.Expression(x => new
            {
                x.QuoteId,
                x.Items.Count,
                SalesPerson = x.SalesPerson.FullName
            });
            var quotes = CrmQuote.Helper.FindWithProjection(filter, project).ToList();
            foreach (var quote in quotes)
                Console.WriteLine($"QuoteId: {quote.QuoteId} has {quote.Count} items Salesperson {quote.SalesPerson}");
        }
    }
}
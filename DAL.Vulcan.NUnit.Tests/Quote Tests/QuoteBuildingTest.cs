using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture]
    public class QuoteBuildingTest
    {
        private HelperUser _helperUser;
        private HelperCompany _helperCompany;
        private HelperQuote _helperQuote;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
           _helperUser = new HelperUser(new HelperPerson());
           _helperCompany = new HelperCompany();
           _helperQuote = new HelperQuote();
        }


        [Test]
        public void CreateQuoteForEur02071()
        {
            var quote = _helperQuote.CreateNewQuoteForCompany("vulcancrm", "5a660b21b508d745a088a2bf", "EUR",  "5a6606dbb508d75b848f488c");
        }

        //[Test]
        //public void BuildAQuote()
        //{
        //    var user = _helperUser.LookupUserByNetworkId("mpike");
        //    var crmUser = _helperUser.GetCrmUser(AppName, user.Id.ToString());
        //    var company = _helperCompany.GetCompanyRef("593ee5ccb508d7372cf9cd50");

        //    var quoteModel = _helperQuote.CreateNewQuoteForCompany(AppName, user.Id.ToString(), "INC", company.Id);
        //    quoteModel = _helperQuote.SaveQuote(AppName, user.Id.ToString(), quoteModel);

        //    Assert.IsTrue(quoteModel.Company.Name == "Breaux Machine Works Inc");
        //    Assert.IsTrue(quoteModel.IsCompany);

        //    var quoteHeader = _helperQuote.GetQuote(quoteModel.Id);

        //    //var newItem = _helperQuote.CreateQuoteItem(quoteHeader.Id.ToString(), "925 12 SAPH", "INC", new OrderQuantity(4, 4, "in"));
        //    //Console.WriteLine(ObjectDumper.Dump(newItem));


        //}
    }
}

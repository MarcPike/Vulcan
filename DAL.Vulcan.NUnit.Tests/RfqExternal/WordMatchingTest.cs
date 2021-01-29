using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.RfqExternal
{
    [TestFixture]
    public class WordMatchingTest
    {
        private HelperUser _helperUser;
        private HelperRfq _helperRfq;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
            _helperRfq = new HelperRfq();
            _helperUser = new HelperUser(new HelperPerson());
        }

        [Test]
        public void TestWordMatch()
        {
            var crmUser = _helperUser.GetCrmUser("vulcancrm", "599b1573b508d62d0c75a115");
            var team = crmUser.ViewConfig.Team;

            //var keywords = _helperRfq.GetTeamConfig("vulcancrm", "599b1573b508d62d0c75a115", team);
            //keywords.Keywords.Add("the");
            //keywords.Keywords.Add("of");
            //_helperRfq.SaveTeamConfig(keywords);

            var rfq = _helperRfq.GetNewRfqExternalModelForCustomer();
            rfq.Location = "Texas";
            rfq.CompanyName = "Pikesoft, Inc.";
            rfq.ContactEmailAddress = "marc.pike@gmail.com";
            rfq.ContactName = "Marc Pike";
            rfq.RfqText =
                @"Pompeo spoke a day after the Pentagon issued contradictory and confusing signals about whether US troops would be pulled from Iraq and Drumpf doubled down on his threats to strike Iranian cultural sites -- an international war crime that Pompeo had earlier tried to deny the President had said at all.
His appearance Tuesday failed to clear up lingering uncertainty or quell calls for more information.";
            _helperRfq.SaveRfqExternal(rfq);

            var rfqList = _helperRfq.GetAllForTeam("vulcancrm", "599b1573b508d62d0c75a115", team, DateTime.Now.Date.AddDays(-1), DateTime.Now.Date.AddDays(1));
            var fullRfq = rfqList.SingleOrDefault(x => x.RequestForQuoteId == rfq.RequestForQuoteId);
            Assert.IsNotNull(fullRfq);

            Console.WriteLine(fullRfq.KeywordMatches);



        }
    }
}

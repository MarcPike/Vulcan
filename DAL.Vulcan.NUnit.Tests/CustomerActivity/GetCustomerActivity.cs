using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Test;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.CustomerActivity
{
    [TestFixture]
    public class GetCustomerActivity
    {
        private IHelperQuote _helperQuote;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
            _helperQuote = new HelperQuote();
        }

        [Test]
        public void Execute()
        {
            var result = _helperQuote.GetCustomerActivityView(
                "vulcancrm", 
                "5a660b21b508d745a088a2bf", 
                DateTime.Parse("5/1/2018"),
                DateTime.Now, 
                "5a660b22b508d745a088a481");

            foreach (var value in result.CompanyActivities)
            {
                Console.WriteLine(ObjectDumper.Dump(value));
            }
        }
    }
}

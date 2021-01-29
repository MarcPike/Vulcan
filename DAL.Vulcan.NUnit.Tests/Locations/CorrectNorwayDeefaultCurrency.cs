using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Currency;
using DAL.Vulcan.Mongo.DocClass.Locations;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Locations
{
    [TestFixture]
    public class CorrectNorwayDeefaultCurrency
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void Execute()
        {
            var currencyType = CurrencyType.GetCurrencyTypeFor("NOK");

            var norway = Location.Helper.Find(x => x.Office == "Norway").First();
            norway.DefaultCurrency = currencyType;
            Location.Helper.Upsert(norway);

        }

        [Test]
        public void TestGetCoidForNorway()
        {
            var norway = Location.Helper.Find(x => x.Office == "Norway").First();
            var coid = norway.GetCoid();
            Assert.AreEqual("EUR", coid);

        }

    }
}

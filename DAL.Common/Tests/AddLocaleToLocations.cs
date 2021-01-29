using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.Common.Tests
{
    [TestFixture]
    public class AddLocaleToLocations
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            var queryHelper = new MongoRawQueryHelper<Location>();

            var locations = queryHelper.GetAll();
            foreach (var location in locations.Where(x=>x.Locale == string.Empty))
            {
                if (location.Coid == null) location.Locale = "en-US";
                if (location.Coid == "INC") location.Locale = "en-US";
                if (location.Coid == "EUR") location.Locale = "en-GB";
                if (location.Coid == "CAN") location.Locale = "en-CA";
                if (location.Coid == "MSA") location.Locale = "en-MY";
                if (location.Coid == "SIN") location.Locale = "en-SG";
                if (location.Coid == "AUS") location.Locale = "en-AU";
                if (location.Coid == "IND") location.Locale = "en-ID";
                if (location.Coid == "DUB") location.Locale = "en-US";
                if (location.Coid == "CHI") location.Locale = "en-US";

                queryHelper.Upsert(location);

            }
        }
    }
}

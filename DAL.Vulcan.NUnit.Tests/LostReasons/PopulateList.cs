using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.LostReasons
{
    [TestFixture]
    public class PopulateList
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var reasons = new List<string>()
            {
                "Price",
                "Lead Time",
                "Inventory",
                "Specification"
            };

            var rep = new RepositoryBase<LostReason>();
            foreach (var reason in reasons)
            {
                var lostReason = rep.AsQueryable().SingleOrDefault(x => x.Reason == reason);
                if (lostReason == null)
                {
                    lostReason = new LostReason()
                    {
                        Reason = reason
                    };
                    lostReason.SaveToDatabase();
                    Console.WriteLine(lostReason.Reason + " was added");
                }
            }


        }

        [Test]
        public void ReportReasons()
        {
            foreach (var lostReason in LostReason.Helper.GetAll())
            {
                Console.WriteLine($"{lostReason.Reason}");
            }
        }
    }
}

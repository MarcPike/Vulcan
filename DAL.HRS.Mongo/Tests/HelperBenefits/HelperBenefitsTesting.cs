using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperBenefits
{
    [TestFixture]
    public class HelperBenefitsTesting
    {
        private Helpers.HelperBenefits _helperBenefits;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            _helperBenefits = new Helpers.HelperBenefits();
        }

        [Test]
        public void GridTest()
        {
            //var deniseUserId = "5dd40f67095f593670c5d95f";
            var loriUserId = "5dd40f67095f593670c5d929";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = _helperBenefits.GetBenefitsGrid(loriUserId);
            sw.Stop();
            Console.WriteLine($"{results.Count} rows. Elapsed {sw.Elapsed}");

            Console.WriteLine();
            foreach (var model in results.Where(x=>x.PayrollId == "001405"))
            {
                Console.WriteLine($"{model.PayrollId} - {model.FirstName } {model.LastName}");
            }

        }
    }
}

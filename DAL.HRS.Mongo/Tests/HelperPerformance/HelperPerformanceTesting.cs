using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperPerformance
{
    [TestFixture]
    class HelperPerformanceTesting
    {
        private Helpers.HelperPerformance _helperPerformance;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _helperPerformance = new Helpers.HelperPerformance();
        }

        [Test]
        public void GridTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = _helperPerformance.GetPerformanceGrid("5dd40f67095f593670c5d95f");
            sw.Stop();
            Console.WriteLine($"{results.Count} rows. Elapsed {sw.Elapsed}");
        }

    }
}

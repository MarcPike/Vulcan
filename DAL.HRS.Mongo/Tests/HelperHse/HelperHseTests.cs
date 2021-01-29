using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperHse
{
    [TestFixture]
    public class HelperHseTests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void GetBbsObservationGrid()
        {
            var helper = new Helpers.HelperHse();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var rows = helper.GetBbsObservationGrid();
            sw.Stop();
            Console.WriteLine($"{sw.Elapsed} {rows.Count} rows");
        }
    }
}

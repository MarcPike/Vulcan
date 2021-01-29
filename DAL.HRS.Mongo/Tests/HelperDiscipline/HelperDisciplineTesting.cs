using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperDiscipline
{
    [TestFixture]
    public class HelperDisciplineTesting
    {
        private Helpers.HelperDiscipline _helperDiscipline;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            _helperDiscipline = new Helpers.HelperDiscipline();
        }

        [Test]
        public void GridTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = _helperDiscipline.GetDisciplineGrid("5e5fc9ea095f5993fc5bb6d8");
            sw.Stop();
            Console.WriteLine($"{results.Count} rows. Elapsed {sw.Elapsed}");

            foreach (var disciplineGridModel in results.OrderBy(x=>x.PayrollId).ToList())
            {
                Console.WriteLine($"{disciplineGridModel.EmployeeId} {disciplineGridModel.PayrollId} {disciplineGridModel.LastName}");
            }

        }
    }
}

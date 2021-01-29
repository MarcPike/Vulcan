using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.Training
{
    [TestFixture()]
    public class TrainingTests
    {
        private HelperTraining _helperTraining;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _helperTraining = new HelperTraining();
        }

        [Test]
        public void GetAllTrainingEventsForEmployeesGrid()
        {
            var sw = new Stopwatch();
            sw.Start();
            var results = _helperTraining.GetAllTrainingEventsForEmployeesGrid(null);
            sw.Stop();
            Console.WriteLine($"Found {results.Count} in {sw.Elapsed}");

        }
    }
}

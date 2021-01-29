using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Import.Tests
{
    [TestFixture]
    public class BbsObservationTests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Take5DepartmentNotNullCount()
        {
            var docCount = BbsObservation.Helper.Find(x => x.Take5Department != null).CountDocuments();
            Console.WriteLine($"{docCount} documents have a Take5Department");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.ApplicationLogView
{
    [TestFixture]
    public class ApplicationLogDumper
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();


        }

        [Test]
        public void Examine()
        {
            var fiveMinutesBefore = new DateTime(2020, 8, 18, 2, 0, 0);

            var logs = ApplicationLog.Helper.Find(x =>
                x.CreateDateTime >= fiveMinutesBefore.AddMinutes(-5) && x.CreateDateTime <= fiveMinutesBefore).ToList();
            foreach (var applicationLog in logs)
            {
                Console.WriteLine(ObjectDumper.Dump(applicationLog));
            }

        }
    }
}

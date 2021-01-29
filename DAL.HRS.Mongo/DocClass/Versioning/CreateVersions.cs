using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.DocClass.Versioning
{
    [TestFixture]
    public class PublishVersions
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void CreateNewVersion()
        {
            VersionHistory.AddNewVersion("2.1.0.0", new List<string>()
            {
                "Resolved saving of Education Certifications"
            });
        }

        [Test]
        public void GetLatestVersion()
        {
            var version = VersionHistory.GetLatestVersion();
            Console.WriteLine(ObjectDumper.Dump(version));
        }

    }
}

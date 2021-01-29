using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Team_Tests
{
    [TestFixture]
    public class TeamCompanyTesting
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void InspectMsa()
        {
            var team = Team.Helper.Find(x => x.Name == "Malaysia").FirstOrDefault();

            var company = team.NonAlliances.FirstOrDefault(x => x.Code == "00047");
            Console.WriteLine(company.Name);

        }

    }
}

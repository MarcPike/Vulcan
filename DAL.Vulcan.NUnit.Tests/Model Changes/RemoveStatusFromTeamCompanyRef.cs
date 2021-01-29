using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Model_Changes
{
    [TestFixture]
    public class RemoveStatusFromTeamCompanyRef
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
        }

        [Test]
        public void ThisFails()
        {

            // Because the Team.Companies has a Status = "A" for each CompanyRef if I don't have it in the CompanyRef Object
            var teams = new RepositoryBase<Team>().AsQueryable().ToList();
            foreach (var team in teams)
            {
                Console.WriteLine("Team "+team.Name);
            }
        }

        [Test]
        public void Execute()
        {
            // In the CompanyRef constructor I added to RemoveFields ["IsActive","Status"] so now all I have to do is run
            // through each Team Company and reset the value
            try
            {

                var rep = new RepositoryBase<Team>();
                foreach (var team in rep.AsQueryable().ToList())
                {
                    //var index = 0;
                    foreach (var companyRef in team.Companies.ToList())
                    {
                    }

                    rep.Upsert(team);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

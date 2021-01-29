using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company_Groups
{
    [TestFixture]
    public class LoadMissingCompaniesForAllGroups
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
        }

        [Test]
        public void Execute()
        {
            var repTeam = new RepositoryBase<Team>();
            var repCompanyGroup = new RepositoryBase<CompanyGroup>();
            var allianceCompaniesAdded = new List<CompanyRef>();
            var nonAllianceCompaniesAdded = new List<CompanyRef>();
            foreach (var companyGroup in repCompanyGroup.AsQueryable().Where(x => x.IsAlliance).ToList())
            {
                var newCompanies = companyGroup.RefreshAllianceCompanyList(repCompanyGroup);
                if (newCompanies.Any())
                {
                    var teamsUsingThisGroup = repTeam.AsQueryable()
                        .Where(x => x.CompanyGroups.Any(g => g.Id == companyGroup.Id.ToString()));

                    foreach (var team in teamsUsingThisGroup)
                    {
                        team.Companies.AddListOfReferenceObjects(newCompanies);
                    }
                }
                allianceCompaniesAdded.AddRange(newCompanies);
                
            }
            foreach (var companyGroup in repCompanyGroup.AsQueryable().Where(x => x.IsAlliance == false).ToList())
            {

                var newCompanies = companyGroup.RefreshNonAllianceCompanyList(repCompanyGroup);
                if (newCompanies.Any())
                {
                    var teamsUsingThisGroup = repTeam.AsQueryable()
                        .Where(x => x.CompanyGroups.Any(g => g.Id == companyGroup.Id.ToString()));

                    foreach (var team in teamsUsingThisGroup)
                    {
                        team.Companies.AddListOfReferenceObjects(newCompanies);
                    }
                }
                nonAllianceCompaniesAdded.AddRange(newCompanies);
            }

            Console.WriteLine("Alliance Companies Added");
            Console.WriteLine("========================");
            foreach (var companyRef in allianceCompaniesAdded)
            {
                Console.WriteLine($"{companyRef.Code} - {companyRef.Name}");
            }

            Console.WriteLine("");

            Console.WriteLine("Non-Alliance Companies Added");
            Console.WriteLine("========================");
            foreach (var companyRef in nonAllianceCompaniesAdded)
            {
                Console.WriteLine($"{companyRef.Code} - {companyRef.Name}");
            }

        }

        [Test]
        public void VerifyCorrect()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<CompanyGroup>();
            var usGroup = rep.AsQueryable().Single(x=>x.Name == "US");
            var allianceGroups = rep.AsQueryable().Where(x => x.Branch == "USA" && x.NameContains != string.Empty);
            foreach (var allianceGroup in allianceGroups)
            {
                if (usGroup.Companies.Any(x => x.Name.Contains(allianceGroup.NameContains)))
                {
                    Assert.IsTrue(false);
                }
            }
        }
    }
}

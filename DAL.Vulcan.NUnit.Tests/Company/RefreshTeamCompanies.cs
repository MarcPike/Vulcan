using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;
using DAL.Vulcan.Mongo.DocClass.Companies;
using MongoDB.Bson;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class RefreshTeamCompanies
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void ExecuteRefreshTeamCompanies()
        {
            var teams = new RepositoryBase<Team>().AsQueryable().ToList();
            foreach (var team in teams)
            {
                team.RefreshTeamCompaniesList();
                team.RefreshAllianceNonAllianceLists();
            }
        }

        [Test]
        public void FixCompanyNames()
        {
            var team = Team.Helper.Find(x => x.Name == "UK Sales Team").FirstOrDefault();

            Assert.IsNotNull(team);

            team.RefreshCompanyNames();

        }

        [Test]
        public void CheckNameOfCompany()
        {
            var team = Team.Helper.Find(x => x.Name == "Singapore").Single();
            //var companyInQuestion = team.Companies.Single(x => x.Code == "00047");
            //Console.WriteLine($"{companyInQuestion.Code} {companyInQuestion.Name} {companyInQuestion.AsCompany().Location.Office}");


            Console.WriteLine("Companies that have code 00047");
            var companiesWithThatCode =
                DAL.Vulcan.Mongo.DocClass.Companies.Company.Helper.Find(x => x.Code == "00047").ToList();
            foreach (var company in companiesWithThatCode)
            {
                Console.WriteLine($"{company.Code} {company.Name} {company.Location.Office}");
            }

        }

        [Test]
        public void GetInc00083()
        {
            var companies = Mongo.DocClass.Companies.Company.Helper.Find(x => x.Code == "00083").ToList();
            foreach (var company in companies)
            {
                Console.WriteLine($"{company.Id} - {company.Name}");
            }

            //var companyId = "5a6606cab508d75b848f2228";
            //var company = Mongo.DocClass.Companies.Company.Helper.FindById(companyId);
            //Console.WriteLine(company.Name);

            //foreach (var team in Team.Helper.Find(x=>x.Companies.Any(c=>c.Id == companyId)).ToList())
            //{
            //    var teamComp = team.Companies.First(x => x.Id == companyId);
            //    teamComp.Name = company.Name;

            //    var teamAlliance = team.Alliances.FirstOrDefault(x => x.Id == companyId);
            //    if ((teamAlliance != null) && (teamAlliance.Name != company.Name))
            //        teamAlliance.Name = company.Name;

            //    var teamNonAlliance = team.NonAlliances.FirstOrDefault(x => x.Id == companyId);
            //    if ((teamNonAlliance != null) && (teamNonAlliance.Name != company.Name))
            //        teamNonAlliance.Name = company.Name;

            //    Team.Helper.Upsert(team);

            //}

        }



        [Test]
        public void ReviewTeamCompanies()
        {
            var team = Team.Helper.Find(x => x.Name == "Malaysia").Single();
            //team.RefreshCompanyNames();
            foreach (var company in team.Companies.OrderBy(x=>x.Name))
            {
                Console.WriteLine($"{company.Code} {company.Name}");
            }

        }

        [Test]
        public void RemoveBlankCompanies()
        {
            var team = Team.Helper.Find(x => x.Name == "Malaysia").Single();
            var companiesBlank = team.Companies.Where(x => x.Name == String.Empty).ToList();
            foreach (var companyRef in companiesBlank)
            {
                var indexOf = team.Companies.IndexOf(companyRef);
                team.Companies.RemoveAt(indexOf);
            }

            Team.Helper.Upsert(team);
        }


        [Test]
        public void Find00047InSingapore()
        {
            var companyQuery = new QueryCompany("DUB");
            var company = companyQuery.GetForCode("00047");
            Console.WriteLine(company.Name);
        }

    }
}

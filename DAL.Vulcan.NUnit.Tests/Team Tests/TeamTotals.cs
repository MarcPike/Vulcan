using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Team_Tests
{
    [TestFixture]
    public class TeamTotals
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void Count()
        {
            var teams = new RepositoryBase<Team>().AsQueryable().ToList();
            var repQuotes = new RepositoryBase<CrmQuote>();

            foreach (var team in teams.OrderBy(x=>x.Name))
            {
                var teamRef = team.AsTeamRef();
                Console.WriteLine("");
                Console.WriteLine($"{teamRef.Name}");
                Console.WriteLine("====================================================");
                Console.WriteLine($"\tDraft: {repQuotes.AsQueryable().Count(x => x.Team.Id == teamRef.Id && x.Status == PipelineStatus.Draft)}");
                Console.WriteLine($"\tSubmitted: {repQuotes.AsQueryable().Count(x => x.Team.Id == teamRef.Id && x.Status == PipelineStatus.Submitted)}");
                Console.WriteLine($"\tExpired: {repQuotes.AsQueryable().Count(x => x.Team.Id == teamRef.Id && x.Status == PipelineStatus.Expired)}");
                Console.WriteLine($"\tLoss: {repQuotes.AsQueryable().Count(x => x.Team.Id == teamRef.Id && x.Status == PipelineStatus.Loss)}");
                Console.WriteLine($"\tWon: {repQuotes.AsQueryable().Count(x => x.Team.Id == teamRef.Id && x.Status == PipelineStatus.Won)}");
                Console.WriteLine("====================================================");
            }

        }
    }
}

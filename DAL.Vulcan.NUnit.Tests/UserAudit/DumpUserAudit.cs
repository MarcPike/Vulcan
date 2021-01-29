using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.UserAudit
{
    [TestFixture]
    public class DumpUserAudit
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var teams = Team.Helper.GetAll().OrderBy(x => x.Name).ToList();
            foreach (var team in teams)
            {
                Console.WriteLine($"Sales Team: {team.Name}");
                foreach (var crmUser in team.CrmUsers.OrderBy(x=>x.LastName))
                {
                    Console.WriteLine($" --> {crmUser.Type} - {crmUser.FullName}");
                }
            }
        }
        
    }
}

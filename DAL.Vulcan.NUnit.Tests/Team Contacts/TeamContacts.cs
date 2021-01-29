using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Team_Contacts
{
    [TestFixture]
    public class TeamContactsTest
    {
        private RepositoryBase<CrmQuote> _repQuote;
        private string _teamId = "5ab1580005d21f1b789e9427"; // Houston Sales

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            _repQuote =  new RepositoryBase<CrmQuote>();
        }

        [Test]
        public void Execute()
        {
            var contacts = _repQuote.AsQueryable().Where(x=>x.Team.Id == _teamId && x.Contact != null).Select(x => x.Contact).Distinct().OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName).ToList();

            foreach (var contactRef in contacts)
            {
                Console.WriteLine($"{contactRef.Id} {contactRef.FirstName} {contactRef.LastName}");
            }
        }
    }
}

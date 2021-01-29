using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;
using DAL.Vulcan.Mongo;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class UpdateOrderClassifications
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var helper = DAL.Vulcan.Mongo.DocClass.Companies.Company.Helper;
            var companies = helper.GetAll();
            foreach (var company in companies)
            {
                company.LoadOrderClassificationFromIMetal();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.CompanyDefaults
{
    [TestFixture]
    public class SetCompanyDefaultForCanada
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void SetSalesGroupToCanadaSales()
        {
            var rep = new RepositoryBase<Mongo.DocClass.Companies.CompanyDefaults>();
            var companyDefaults = rep.AsQueryable().Where(x => x.Coid == "CAN" ).ToList();
            foreach (var companyDefault in companyDefaults)
            {
                if (companyDefault.CompanyId != "5ad61066b508d72f105b10ea")
                {
                    companyDefault.SalesGroupCode = "CS";
                    rep.Upsert(companyDefault);
                }
            }
        }
    }
}

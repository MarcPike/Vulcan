using System;
using System.Collections.Generic;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates;
using NUnit.Framework;

namespace DAL.Vulcan.Mongo.Core.Test.NunitTests.CompanyTests
{
    [TestFixture]
    public class FindCompany
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        public void Execute()
        {
            //var company = DAL.Vulcan.Mongo.Core.DocClass.Companies.Company.Helper.Find(x=>x.Code)
        }

    }
}

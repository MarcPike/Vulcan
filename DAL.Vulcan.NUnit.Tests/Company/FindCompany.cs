using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class FindCompany
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var company = DAL.Vulcan.Mongo.DocClass.Companies.Company.Helper.Find(x => x.Code == "02000" && x.Location.Branch == "USA").FirstOrDefault();
            Console.WriteLine($"{company.Name} IsActive={company.IsActive}");

            company.LoadIsActiveFromIMetal();
            Console.WriteLine($"{company.Name} IsActive={company.IsActive}");


        }
    }
}

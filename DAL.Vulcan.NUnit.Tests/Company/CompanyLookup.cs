using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class CompanyLookup
    {
        [Test]
        public void LookupCompany()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var companiesWithName = rep.AsQueryable().Where(x => x.Name.Contains("Knust-Godwin")).ToList();
            foreach (var company in companiesWithName)
            {
                Console.WriteLine(ObjectDumper.Dump(company));
            }
        }

        [Test]
        public void LookupCompanyDuplicate()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var companiesWithCode = rep.AsQueryable().Where(x => x.Code == "00299").ToList();
            foreach (var company in companiesWithCode)
            {
                Console.WriteLine($"{company.Location.GetCoid()} - {company.Code}:{company.Name}");
            }
        }

        [Test]
        public void InvalidCompaniesInHoustonSales()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var team = new RepositoryBase<Team>().AsQueryable().Single(x=>x.Name.Contains("Houston"));

            var companiesInOtherCoid = new List<Mongo.DocClass.Companies.Company>();

            Console.WriteLine("=========================");
            Console.WriteLine("Companies that are in INC");
            Console.WriteLine("=========================");
            foreach (var company in team.Companies.Select(x=>x.AsCompany()))
            {
                if (company.Location.GetCoid() != "INC")
                {
                    companiesInOtherCoid.Add(company);
                }
                Console.WriteLine($"{company.Location.GetCoid()} - {company.Code}:{company.Name}");

            }
            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine("Companies that are in other COID");
            Console.WriteLine("================================");
            foreach (var company in companiesInOtherCoid)
            {
                Console.WriteLine($"{company.Location.GetCoid()} - {company.Code}:{company.Name}");
            }

        }

    }
}

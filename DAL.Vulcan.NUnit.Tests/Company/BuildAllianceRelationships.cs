using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class BuildAllianceRelationships
    {
        private List<CompanyGroup> _companyGroups = new RepositoryBase<CompanyGroup>().AsQueryable().ToList();
        private List<Mongo.DocClass.Companies.Company> _companies = new RepositoryBase<Mongo.DocClass.Companies.Company>().AsQueryable().ToList();

        [Test]
        public void Execute()
        {
            foreach (var companyGroup in _companyGroups)
            {
                foreach (var company in companyGroup.GetAllCompanies().Select(x=>x.AsCompany()))
                {
                    if (company.IsAlliance != companyGroup.IsAlliance)
                    {
                        company.IsAlliance = companyGroup.IsAlliance;
                        company.SaveToDatabase();
                        Console.WriteLine($"{company.ShortName} IsAlliance set to {companyGroup.IsAlliance}");
                    }
                    else
                    {
                        company.SaveToDatabase();
                    }
                }
            }
        }

    }
}

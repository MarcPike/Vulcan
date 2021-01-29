using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.Base.ValueBucket;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.CRM;


namespace DAL.Vulcan.NUnit.Tests.ValueBucket
{
    [TestFixture]
    public class BucketTest
    {
        [Test]
        public void CompanyTest()
        {
            var companies = new RepositoryBase<DAL.Vulcan.Mongo.DocClass.Companies.Company>().AsQueryable().Where(x=>x.Name != string.Empty).ToList();
            using (var companyBucket = new ValueBucket<DAL.Vulcan.Mongo.DocClass.Companies.Company, string>())
            {
                var resultByName = companyBucket.Execute(companies, (company) => company.Name.ToUpper().Substring(0, 1));
                Console.WriteLine("");
                Console.WriteLine("Companies Grouped By First Letter of Name");
                Console.WriteLine("=========================================");
                foreach (var bucketValue in resultByName)
                {
                    Console.WriteLine($"{bucketValue.GroupBy} - {bucketValue.Documents.Count} documents");
                }
                Console.WriteLine("");
                Console.WriteLine("Companies Grouped By Office");
                Console.WriteLine("===========================");
                var resultsByOffice = companyBucket.Execute(companies, (company) => company.Location.Office);
                foreach (var bucketValue in resultsByOffice)
                {
                    Console.WriteLine($"{bucketValue.GroupBy} - {bucketValue.Documents.Count} documents");
                }
                Console.WriteLine("");
                Console.WriteLine("Companies Grouped By Country");
                Console.WriteLine("============================");
                var resultsByCountry = companyBucket.Execute(companies, (company) => company.Location.Country);
                foreach (var bucketValue in resultsByCountry)
                {
                    Console.WriteLine($"{bucketValue.GroupBy} - {bucketValue.Documents.Count} documents");
                }

            }
        }

        [Test]
        public void UserTest()
        {
            var crmUsers = new RepositoryBase<Mongo.DocClass.CRM.CrmUser>().AsQueryable().ToList();
            using (var userBucket = new ValueBucket<Mongo.DocClass.CRM.CrmUser, CrmUserType>())
            {
                var result = userBucket.Execute(crmUsers, (user) => user.UserType);
                foreach (var bucketValue in result)
                {
                    Console.WriteLine($"{bucketValue.GroupBy} - {bucketValue.Documents.Count} documents");
                }

            }
        }
    }
}

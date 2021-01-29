using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Tests.Employee_Tests;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.Properties
{
    [TestFixture()]
    public class GetProperties
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void GetJobTitles()
        {
            var jobTitles = new MongoRawQueryHelper<JobTitle>().GetAll().OrderBy(x=>x.Name);
            foreach (var jobTitle in jobTitles)
            {
                Console.WriteLine(jobTitle.Name);
            }
        }

        [Test]
        public void GetAll()
        {
            var propertyTypes = new MongoRawQueryHelper<PropertyType>().GetAll();
            
            foreach (var propertyType in propertyTypes)
            {
                Console.WriteLine($"{propertyType.Type} - {propertyType.Description}");
                Console.WriteLine("================================================");
                var queryHelper = new MongoRawQueryHelper<PropertyValue>();
                var filter = queryHelper.FilterBuilder.Where(x => x.Type == propertyType.Type);

                foreach (var propertyValue in queryHelper.Find(filter).OrderBy(x=>x.Code))
                {
                    Console.WriteLine(propertyValue.Code);
                }
                Console.WriteLine("");
            }
        }
    }
}

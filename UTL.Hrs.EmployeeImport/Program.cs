using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Tests;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace UTL.Hrs.EmployeeImport
{
    [TestFixture]
    class Program
    {

        static void Main(string[] args)
        {
            //ImportDemographics();
        }

        [Test]
        public void CheckForMissingEntity()
        {
            var queryHelper = new MongoRawQueryHelper<HrsUser>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Entity == null);
            var usersWithNoEntity = queryHelper.Find(filter);
            foreach (var hrsUser in usersWithNoEntity)
            {
                Console.WriteLine(hrsUser.FullName);
            }
        }

        [Test]
        public void RecreateHrsUserEmployeeLocations()
        {
            //EnvironmentSettings.HrsDevelopment();
            var queryHelper = new MongoRawQueryHelper<HrsUser>();
            var allUsers = queryHelper.GetAll();
            foreach (var hrsUser in allUsers)
            {
                hrsUser.Location = hrsUser.Location.AsLocation().AsLocationRef();
                queryHelper.Upsert(hrsUser);
            }
        }

        [Test]
        public void AddSpacesToLocationOffice()
        {
            var queryHelper = new MongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Entity.Name == "Edgen Murray");
            var edgenLocations = queryHelper.Find(filter).ToList();
            foreach (var edgenLocation in edgenLocations)
            {
                edgenLocation.Office = edgenLocation.Office.Replace("EM-", "EM - ");
                edgenLocation.Office = edgenLocation.Office.Replace("HSP-", "HSP - ");
                queryHelper.Upsert(edgenLocation);
            }
        }

        [Test]
        public void RemoveInvalidLocations()
        {
            var queryHelper = new MongoRawQueryHelper<Location>();
            var locations = queryHelper.GetAll();
            var badLocations = locations.Where(x => x.Coid == "<unknown>").ToList();
            foreach (var badLocation in badLocations)
            {
                queryHelper.DeleteOne(badLocation.Id);
            }
        }

        [Test]
        public void GetEmployeesWithNotLocation()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Location == null);
            var project =
                queryHelper.ProjectionBuilder.Expression(x => new
                    { x.Id, x.FirstName, x.LastName, x.ExternalLocationText });
            var results = queryHelper.FindWithProjection(filter, project);
            foreach (var emp in results)
            {
                Console.WriteLine(ObjectDumper.Dump(emp));
            }
        }
    }
}

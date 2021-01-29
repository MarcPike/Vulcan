using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Test.Update_Location_All_Users
{
    [TestFixture()]
    public class FixLocationForAllUsers
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void ExecuteForAllLdapUsers()
        {
            var locations = new RepositoryBase<Location>().AsQueryable().ToList();
            foreach (var ldapUser in new RepositoryBase<LdapUser>().AsQueryable().ToList())
            {
                if (ldapUser.Location.Coid == null)
                {
                    ldapUser.Location = locations.FirstOrDefault(x => x.Id.ToString() == ldapUser.Location.Id)
                        .AsLocationRef();
                    ldapUser.SaveToDatabase();
                }
            }
        }

        [Test]
        public void ExecuteForAlHrsUsers()
        {
            foreach (var hrsUser in new RepositoryBase<HrsUser>().AsQueryable().ToList())
            {
                hrsUser.SaveToDatabase();
            }
        }

        [Test]
        public void ExecuteForAlEmployees()
        {
            var locations = new MongoRawQueryHelper<Location>().GetAll();

            var helperEmployee = new MongoRawQueryHelper<Employee>();
            var employees = helperEmployee.GetAll();
            foreach (var employee in employees)
            {
                var actualLocation = locations.Single(x => x.Office == employee.Location.Office);
                if (employee.Location.Id != actualLocation.Id.ToString())
                {
                    employee.Location = actualLocation.AsLocationRef();
                    helperEmployee.Upsert(employee);
                }
            }
        }


        [Test]
        public void FixLocationsForAllHrsUsers()
        {

            var locations = new MongoRawQueryHelper<Location>().GetAll();

            var helperHrsUser = new MongoRawQueryHelper<HrsUser>();
            var hrsUsers = helperHrsUser.GetAll();
            foreach (var hrsUser in hrsUsers)
            {
                var modified = false;
                var index = 0;
                foreach (var hrsSecurityLocation in hrsUser.HrsSecurity.Locations.ToList())
                {
                    var actualLocation = locations.Single(x => x.Office == hrsSecurityLocation.Office);
                    if (actualLocation.Id.ToString() != hrsSecurityLocation.Id)
                    {
                        hrsUser.HrsSecurity.Locations[index] = actualLocation.AsLocationRef();
                        modified = true;
                    }
                    index += 1;
                }

                if (modified) helperHrsUser.Upsert(hrsUser);
            }

        }


    }
}

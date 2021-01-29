using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.Employee_Tests
{
    [TestFixture()]
    public class EmployeeGetLdapUser
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var filter = queryHelper.FilterBuilder.Where(x => x.LdapUser == null);
            var employees = queryHelper.Find(filter).AsReadOnly();
            var activeNoMatch = 0;
            var moreThanOne = 0;
            foreach (var employee in employees)
            {
                var isActive = (employee.TerminationDate == null || employee.TerminationDate > DateTime.Now);
                if (!isActive) continue;


                var ldapUsers = employee.GetLdapUserData();
                if (ldapUsers.Count == 0)
                {
                    Console.WriteLine($"No find for => {employee.LastName}, {employee.FirstName}");
                    activeNoMatch++;
                }

                if (ldapUsers.Count > 1)
                {
                    Console.WriteLine($"More than one for => {employee.LastName}, {employee.FirstName}");
                    moreThanOne++;
                }

                if ((ldapUsers.Count == 1) && (employee.LdapUser == null))
                {
                    employee.LdapUser = ldapUsers[0].AsLdapUserRef();
                    queryHelper.Upsert(employee);
                    Console.WriteLine($"Found and bound! ->  {employee.LastName}, {employee.FirstName}");
                }
            }
            Console.WriteLine($"Active with no matching Ldap record: {activeNoMatch}");
            Console.WriteLine($"More than one Ldap record: {moreThanOne}");
        }
    }
}

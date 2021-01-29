using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.LdapCompareTest
{
    [TestFixture()]
    public class CompareLdap
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsQualityControl();
        }

        [Test]
        public void EmployeeLdapDiffLastNames()
        {
            var queryHelperEmployee = new MongoRawQueryHelper<Employee>();
            var empWithLdapFilter = queryHelperEmployee.FilterBuilder.Where(x => x.LdapUser != null);
            var employeesWithLdap = queryHelperEmployee.Find(empWithLdapFilter);

            foreach (var employee in employeesWithLdap)
            {
                employee.LdapUser = employee.LdapUser.AsLdapUser().AsLdapUserRef();
                var updatedName = employee.LastName;

                if (employee.LastName != employee.LdapUser.LastName)
                {

                    Console.WriteLine($"Employee: {employee.PayrollId} - {employee.FirstName} {employee.LastName} has for Ldap: {employee.LdapUser.GetFullName()}");
                    employee.LdapUser = null;
                }
                queryHelperEmployee.Upsert(employee);

            }



        }

        [Test]
        public void Execute()
        {
            var repEmployee = new RepositoryBase<Employee>();
            var repLdap = new RepositoryBase<LdapUser>();


            foreach (var employee in repEmployee.AsQueryable().ToList().OrderBy(x=>x.LastName).Where(x => (x.TerminationDate == null || x.TerminationDate > DateTime.Now)))
            {
                var ldap = repLdap.AsQueryable().FirstOrDefault(x => x.NetworkId == employee.Login);
                if (ldap == null)
                {
                    
                    var ldapFound = repLdap.AsQueryable().FirstOrDefault(x=>x.Person.LastName == employee.LastName && x.Person.FirstName == employee.FirstName);

                    if (ldapFound != null)
                    {
                        Console.WriteLine($"{employee.LastName}, {employee.FirstName} Employee.LoginId: {employee.Login} corrected to NetworkId: {ldapFound.NetworkId} ");
                        employee.Login = ldapFound.NetworkId;
                        repEmployee.Upsert(employee);
                    }
                    else
                    {
                        Console.WriteLine($"{employee.LastName}, {employee.FirstName} no Ldap row found for {employee.Login} ");
                    }

                }
            }
        }
    }
}

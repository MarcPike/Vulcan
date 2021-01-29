using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Ldap;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Prepare_Database
{
    [TestFixture()]
    public class AddHrsAdminsToDevelopment
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void AddAdmins()
        {
            AddMarc();
            AddIsidro();
            AddShannen();
        }

        [Test]
        public void GetHrsUser()
        {
            var helper = new HelperUser();
            var ldapUser = helper.GetUser("5cd32056ae7ad422c84f19ec");
            Console.WriteLine("ldap");
            Console.WriteLine(ObjectDumper.Dump(ldapUser));
            var hrsUser = helper.GetHrsUser("5cd32056ae7ad422c84f19ec");
            Console.WriteLine("hrsUser");
            Console.WriteLine(ObjectDumper.Dump(hrsUser));
        }

        private SecurityRole GetHrsSystemAdmin()
        {
            var filter = SecurityRole.Helper.FilterBuilder.Where(x => x.RoleType.Name == "HrsSystemAdmin");
            return SecurityRole.Helper.Find(filter).FirstOrDefault();
        }

        private SecurityRole GetHseSystemAdmin()
        {
            var filter = SecurityRole.Helper.FilterBuilder.Where(x => x.RoleType.Name == "HseSystemAdmin");
            return SecurityRole.Helper.Find(filter).FirstOrDefault();
        }

        private void AddMarc()
        {
            var user = new RepositoryBase<LdapUser>().AsQueryable()
                .SingleOrDefault(x => x.Person.LastName == "Pike" || x.Person.FirstName == "Marc");



            var hrsSecurityRole = GetHrsSystemAdmin();
            var hseSecurityRole = GetHseSystemAdmin();

            var empFilter =
                Employee.Helper.FilterBuilder.Where(x => x.LastName == user.LastName && x.FirstName == user.FirstName);
            var empProject = Employee.Helper.ProjectionBuilder.Expression(x => new EmployeeRef(x));
            var employeeRef = Employee.Helper.FindWithProjection(empFilter, empProject).FirstOrDefault();

            var hrsUser = HrsUser.CreateHrsUser(user, hrsSecurityRole, hseSecurityRole, employeeRef);
            hrsUser.SystemAdmin = true;
            hrsUser.Entity = Entity.GetRefByName("Howco");
            HrsUser.Helper.Upsert(hrsUser);
        }

        private void AddIsidro()
        {
            var user = new RepositoryBase<LdapUser>().AsQueryable()
                .SingleOrDefault(x => x.Person.FirstName == "Isidro" && x.LastName == "Gallegos");
            var hrsSecurityRole = GetHrsSystemAdmin();
            var hseSecurityRole = GetHseSystemAdmin();

            var empFilter =
                Employee.Helper.FilterBuilder.Where(x => x.LastName == user.LastName && x.FirstName == user.FirstName);
            var empProject = Employee.Helper.ProjectionBuilder.Expression(x => new EmployeeRef(x));
            var employeeRef = Employee.Helper.FindWithProjection(empFilter, empProject).FirstOrDefault();

            var hrsUser = HrsUser.CreateHrsUser(user, hrsSecurityRole, hseSecurityRole, employeeRef);
            hrsUser.SystemAdmin = true;
            hrsUser.Entity = Entity.GetRefByName("Howco");
            HrsUser.Helper.Upsert(hrsUser);
        }
        private void AddShannen()
        {
            var user = new RepositoryBase<LdapUser>().AsQueryable()
                .SingleOrDefault(x => x.Person.FirstName == "Shannen" && x.Person.LastName == "Reese");
            var hrsSecurityRole = GetHrsSystemAdmin();
            var hseSecurityRole = GetHseSystemAdmin();

            var empFilter =
                Employee.Helper.FilterBuilder.Where(x => x.LastName == user.LastName && x.FirstName == user.FirstName);
            var empProject = Employee.Helper.ProjectionBuilder.Expression(x => new EmployeeRef(x));
            var employeeRef = Employee.Helper.FindWithProjection(empFilter, empProject).FirstOrDefault();

            var hrsUser = HrsUser.CreateHrsUser(user, hrsSecurityRole, hseSecurityRole, employeeRef);
            hrsUser.SystemAdmin = true;
            hrsUser.Entity = Entity.GetRefByName("Howco");
            HrsUser.Helper.Upsert(hrsUser);
        }


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Config;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UTL.Hrs.Production.Builder
{
    [TestFixture]
    public class DataMoverActions
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public static void CopyPropertyTypes()
        {
            var copier = new HrsDocumentCopier<PropertyType>();
            var result = copier.Move();
            Console.WriteLine($"PropertyTypes copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopyPropertyValues()
        {
            var copier = new HrsDocumentCopier<PropertyValue>();
            var result = copier.Move();
            Console.WriteLine($"PropertyValues copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopyJobTitles()
        {
            var copier = new HrsDocumentCopier<JobTitle>();
            var result = copier.Move();
            Console.WriteLine($"JobTitles copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopyBaseHours()
        {
            var copier = new HrsDocumentCopier<BaseHours>();
            var result = copier.Move();
            Console.WriteLine($"BaseHours copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopySecurityRoles()
        {
            var copier = new HrsDocumentCopier<SecurityRole>();
            var result = copier.Move();
            Console.WriteLine($"SecurityRoles copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopySecurityRoleTypes()
        {
            var copier = new HrsDocumentCopier<SecurityRoleType>();
            var result = copier.Move();
            Console.WriteLine($"SecurityRoleTypes copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopySystemModuleTypes()
        {
            var copier = new HrsDocumentCopier<SystemModuleType>();
            var result = copier.Move();
            Console.WriteLine($"SecurityModuleTypes copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopyUserConfiguration()
        {
            var copier = new HrsDocumentCopier<UserConfiguration>();
            var result = copier.Move();
            Console.WriteLine($"UserConfigurations copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopyHrsUsers()
        {
            var copier = new HrsDocumentCopier<HrsUser>();
            var result = copier.Move();
            Console.WriteLine($"HrsUsers copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void CopyTheseEmployees()
        {
            var copier = new HrsDocumentCopier<Employee>();
            var employees = new List<Employee>();

            var queryHelper = new MongoRawQueryHelper<Employee>();

            var hrsUsers = new MongoRawQueryHelper<HrsUser>().GetAll();

            employees.Add(hrsUsers.FirstOrDefault(x => x.LastName == "Gallegos").Employee.AsEmployee());
            employees.Add(hrsUsers.FirstOrDefault(x => x.LastName == "Reese").Employee.AsEmployee());
            employees.Add(hrsUsers.FirstOrDefault(x => x.LastName == "Walker").Employee.AsEmployee());

            var result = copier.AddThese(employees);
            Console.WriteLine($"Employees copied: {result.Added} Removed: {result.Removed}");
        }

        [Test]
        public static void AddEmployeeKennyNess()
        {
            var copier = new HrsDocumentCopier<Employee>();
            var employees = new List<Employee>();

            var entity = Entity.GetRefByName("Edgen Murray");

            var queryHelperEmployee = new MongoRawQueryHelper<Employee>();
            var filter = queryHelperEmployee.FilterBuilder.Where(x => x.LastName == "Ness");

            var kenny = queryHelperEmployee.Find(filter).FirstOrDefault();

            employees.Add(kenny);

            var result = copier.AddThese(employees);
            Console.WriteLine($"Employees copied: {result.Added} Removed: {result.Removed}");
        }

    }





[TestFixture()]
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}

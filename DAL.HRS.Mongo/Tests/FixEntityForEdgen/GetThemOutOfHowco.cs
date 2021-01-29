using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.FixEntityForEdgen
{
    [TestFixture]
    public class GetThemOutOfHowco
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var employeesNotInHowcoWithEM = Employee.Helper.Find(x =>
                x.Entity.Name == "Howco" && x.PayrollId.StartsWith("EM")).ToList();

            var employeesNotInHowcoWithHSP = Employee.Helper.Find(x =>
                x.Entity.Name == "Howco" && x.PayrollId.StartsWith("HSP")).ToList();

            //Console.WriteLine($"EM: {employeesNotInHowcoWithEM.Count}");
            //Console.WriteLine($"HSP: {employeesNotInHowcoWithHSP.Count}");

            Console.WriteLine("EM Employees with incorrect Entity");
            foreach (var employee in employeesNotInHowcoWithEM)
            {
                //Console.WriteLine($"{employee.PayrollId} - {employee.FirstName} {employee.LastName}");
                employee.Entity = Entity.GetRefByName("Edgen Murray");
                Employee.Helper.Upsert(employee);
            }

            Console.WriteLine("HSP Employees with incorrect Entity");
            foreach (var employee in employeesNotInHowcoWithHSP)
            {
                //Console.WriteLine($"{employee.PayrollId} - {employee.FirstName} {employee.LastName}");
                employee.Entity = Entity.GetRefByName("Edgen Murray");
                Employee.Helper.Upsert(employee);
            }

        }

        [Test]
        public void EmployeesMissingEntity()
        {
            var emps = Employee.Helper.Find(x => x.Entity == null).ToList();
            foreach (var employee in emps)
            {
                Console.WriteLine($"{employee.PayrollId} {employee.FirstName} {employee.LastName} has not Entity");
            }

        }

        [Test]
        public void EmployeesMissingLocationEntity()
        {
            var emps = Employee.Helper.Find(x => x.Location.Entity == null).ToList();
            foreach (var employee in emps)
            {
                Console.WriteLine($"{employee.PayrollId} {employee.FirstName} {employee.LastName} has not Location.Entity");
            }

        }



        [Test]
        public void EmployeesMissingLocation()
        {
            var emps = Employee.Helper.Find(x => x.Location == null).ToList();
            foreach (var employee in emps)
            {
                Console.WriteLine($"{employee.PayrollId} {employee.FirstName} {employee.LastName} has not Location");
            }

        }




        [Test]
        public void EmployeesEntityDifferentFromLocationEntity()
        {
            var findFilter = Employee.Helper.FilterBuilder.Empty;
            var project = Employee.Helper.ProjectionBuilder.Expression(x =>
                new {x.Entity, x.Location, x.PayrollId, x.FirstName, x.LastName});


            var emps = Employee.Helper.FindWithProjection(findFilter, project).ToList();
            foreach (var employee in emps /*.Where(x=>x.Entity.Id != x.Location.Entity.Id)*/)
            {
                //Console.WriteLine($"{employee.PayrollId} {employee.FirstName} {employee.LastName} {employee.Entity.Name} and the Location is {employee.Location.Office} which is in {employee.Location.Entity.Name}");
                Console.WriteLine($"{employee.Entity.Name} == {employee.Location.Entity.Name}");
            }
        }
    }
}

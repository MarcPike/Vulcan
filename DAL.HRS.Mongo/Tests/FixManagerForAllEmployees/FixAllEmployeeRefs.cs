using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.FixManagerForAllEmployees
{
    [TestFixture]
    public class FixAllEmployeeRefs
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            Console.WriteLine($"Environment: {EnvironmentSettings.CurrentEnvironment.ToString()}");
            var rows = Employee.Helper.GetAll();
            foreach (var employee in rows)
            {
                var changesMade = false;
                if (employee.Manager != null)
                {
                    var manager = employee.Manager.AsEmployee();
                    if (manager == null)
                    {
                        var managerRef = Employee.Helper.Find(x => x.PayrollId == employee.Manager.PayrollId)
                            .Project(x => new EmployeeRef(x))?.FirstOrDefault();
                        //Assert.IsNotNull(managerRef);

                        if (managerRef != null)
                        {

                            Console.WriteLine($"\t{employee.FirstName} {employee.LastName} {employee.PayrollId} - Manager correction: {managerRef.PayrollId} {managerRef.FullName}");

                            employee.Manager = managerRef;
                            changesMade = true;
                        }
                        else
                        {
                            Console.WriteLine($"***\t{employee.FirstName} {employee.LastName} {employee.PayrollId} - Manager not found: {employee.Manager.PayrollId} {employee.Manager.FullName}");
                        }

                        var index = 0;
                        foreach (var employeeDirectReport in employee.DirectReports.ToList())
                        {
                            var directReport = employeeDirectReport.AsEmployee();
                            if (directReport == null)
                            {
                                var directReportRef = Employee.Helper.Find(x => x.PayrollId == employeeDirectReport.PayrollId)
                                    .Project(x => new EmployeeRef(x))?.FirstOrDefault();
                                //Assert.IsNotNull(directReportRef);

                                if (directReportRef != null)
                                {
                                    Console.WriteLine($"\t{employee.FirstName} {employee.LastName} {employee.PayrollId} - DirectReport correction: {directReportRef.PayrollId} {directReportRef.FullName}");
                                    employee.DirectReports[index] = directReportRef;
                                    changesMade = true;
                                }
                                else
                                {
                                    Console.WriteLine($"***\t{employee.FirstName} {employee.LastName} {employee.PayrollId} - DirectReport not found: {employeeDirectReport.PayrollId} {employeeDirectReport.FullName}");

                                }

                            }

                            index++;
                        }
                    }
                }

                if (changesMade)
                {
                    Employee.Helper.Upsert(employee);
                }

            }
        }
    }
}

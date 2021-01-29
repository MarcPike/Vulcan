using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Bson;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Fixes
{
    [TestFixture]
    public class UpdateEmployeesToCorrectLocation
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void FixAllEmployeeLocationRefs()
        {
            var employees = Employee.Helper.GetAll();
            foreach (var employee in employees)
            {
                var changesMade = false;

                var correctLocation = GetEmployeeLocation(employee.AsEmployeeRef());

                if (employee.Location != null)
                {
                    if (employee.Location.Office != correctLocation.Office)
                    {
                        employee.Location.Office = correctLocation.Office;
                        changesMade = true;
                    }

                }

                if (employee.LdapUser != null)
                {
                    var actualLocation = Location.Helper.FindById(employee.LdapUser.Location.Id);

                    if (employee.LdapUser.Location.Office != actualLocation.Office)
                    {
                        employee.LdapUser.Location.Office = actualLocation.Office;
                        changesMade = true;
                    }
                }

                foreach (var performance in employee.Performance.Where(x=>x.Reviewer != null))
                {
                    if (performance.Reviewer != null)
                    {
                        var actualPerformanceLocation = GetEmployeeLocation(performance.Reviewer);
                        if (performance.Reviewer.Location.Office != actualPerformanceLocation.Office)
                        {
                            performance.Reviewer.Location.Office = actualPerformanceLocation.Office;
                            changesMade = true;
                        }
                    }
                }

                foreach (var directReport in employee.DirectReports)
                {


                    var actualLocation = GetEmployeeLocation(directReport);
                    if (directReport.Location != null)
                    {
                        var locationFound = Location.Helper.FindById(directReport.Location.Id);

                        if (locationFound != null)
                        {
                            if (directReport.Location.Office != locationFound.Office)
                            {
                                directReport.Location = locationFound.AsLocationRef();
                                changesMade = true;
                                continue;
                            }
                        }
                        else 
                        {
                            if (actualLocation != null)
                            {
                                directReport.Location = actualLocation;
                            }
                            else
                            {
                                directReport.Location = Location.Unknown.AsLocationRef();
                            }
                            changesMade = true;
                        }
                    }

                    
                }

                foreach (var discipline in employee.Discipline)
                {
                    if ((discipline.Employee == null) && (discipline.Manager == null)) continue;

                    var actualLocation = GetEmployeeLocation(discipline.Employee ?? discipline.Manager);
                    if (discipline.Location != null)
                    {
                        if (discipline.Location.Office != actualLocation.Office)
                        {
                            discipline.Location.Office = actualLocation.Office;
                            changesMade = true;
                        }
                    }

                    if (discipline.Manager != null)
                    {
                        if (discipline.Manager.Location.Office != actualLocation.Office)
                        {
                            discipline.Manager.Location = actualLocation;
                            changesMade = true;
                        }
                    }

                    if (discipline.Employee != null)
                    {
                        if (discipline.Employee.Location.Office != actualLocation.Office)
                        {
                            discipline.Employee.Location = actualLocation;
                            changesMade = true;
                        }
                    }
                }

                if (employee.Manager != null)
                {
                    var actualManagerLocation = GetEmployeeLocation(employee.Manager);
                    if (employee.Manager.Location.Office != actualManagerLocation.Office)
                    {
                        employee.Manager.Location.Office = actualManagerLocation.Office;
                        changesMade = true;
                    }
                }

                if (changesMade)
                {
                    Employee.Helper.Upsert(employee);
                }

                LocationRef GetEmployeeLocation(EmployeeRef employeeRef)
                {
                    try
                    {
                        var employeeFull = employeeRef.AsEmployee();
                        if (employeeFull.LdapUser != null)
                        {
                            return employeeFull.LdapUser.Location.AsLocation().AsLocationRef();

                        }
                        else if (employeeFull.Location != null)
                        {
                            return employeeFull.Location.AsLocation().AsLocationRef();
                        }
                        else
                        {
                            return Location.Unknown.AsLocationRef();
                        }
                    }
                    catch (Exception)
                    {
                        return Location.Unknown.AsLocationRef();
                    }


                }

            }
        }
    }
}

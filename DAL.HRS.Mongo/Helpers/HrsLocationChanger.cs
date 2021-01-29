using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.Common.Helper;
using DAL.HRS.Mongo.DocClass.Config;
using DAL.HRS.Mongo.DocClass.Discipline;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Helpers
{

    [TestFixture]
    public class HrsLocationChangerExecute
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        public void ChangeOfficeName(string locationId, string newName)
        {
            //new LocationChanger<Entity>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<LdapUser>().ChangeOfficeName(locationId, newName);

            //new LocationChanger<Employee>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<TrainingEvent>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<PropertyValue>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<Certification>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<EmployeeDrugTest>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<EmployeeIncident>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<HrsUser>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<JobTitle>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<RequiredActivity>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<SupportingDocument>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<TrainingCourse>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<UserConfiguration>().ChangeOfficeName(locationId, newName);
            //new LocationChanger<Location>().ChangeOfficeName(locationId, newName);

        }

        [Test]
        public void ExecuteForNameChangesJan2020()
        {
            // change [EM - Dubai] to [EM - Middle East]
            // change [HSP - Dubai] to [HSP - Middle East]
            // change [EM - Paris] to [EM - France]
            Dictionary<string, string> officeNameChanges = new Dictionary<string, string>();

            var filter = Location.Helper.FilterBuilder.Where(x => x.Office == "EM - Dubai" || x.Office == "EM - Middle East");
            var emDubai = Location.Helper.Find(filter).SingleOrDefault();
            Assert.IsNotNull(emDubai);

            filter = Location.Helper.FilterBuilder.Where(x => x.Office == "HSP - Dubai" || x.Office == "HSP - Middle East");
            var hspDubai = Location.Helper.Find(filter).SingleOrDefault();
            Assert.IsNotNull(hspDubai);

            filter = Location.Helper.FilterBuilder.Where(x => x.Office == "EM - Paris" || x.Office == "EM - France");
            var emParis = Location.Helper.Find(filter).SingleOrDefault();
            Assert.IsNotNull(emParis);


            officeNameChanges.Add(emDubai.Id.ToString(), "EM - Middle East");
            officeNameChanges.Add(hspDubai.Id.ToString(), "HSP - Middle East");
            officeNameChanges.Add(emParis.Id.ToString(), "EM - France");

            Console.WriteLine(ObjectDumper.Dump(officeNameChanges));

            foreach (var officeNameChange in officeNameChanges)
            {
                ChangeOfficeName(officeNameChange.Key, officeNameChange.Value);
            }

        }

        [Test]
        public void FixEmployeeRefs()
        {
            var helper = Employee.Helper;
            var employees = helper.GetAll();
            foreach (var employee in employees)
            {
                if (employee.Location != null)
                {
                    employee.Location = employee.Location.AsLocation().AsLocationRef();
                    helper.Upsert(employee);
                }
            }

            foreach (var employee in employees)
            {
                if (employee.Manager != null)
                {
                    try
                    {
                        employee.Manager = employee.Manager.AsEmployee().AsEmployeeRef();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Problem: {employee.FirstName} {employee.LastName} with Direct Report");
                    }

                }

                if (employee.DirectReports != null)
                {
                    foreach (var dr in employee.DirectReports.ToList())
                    {
                        var index = employee.DirectReports.IndexOf(dr);
                        try
                        {
                            employee.DirectReports[index] = dr.AsEmployee().AsEmployeeRef();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Problem: {employee.FirstName} {employee.LastName} with Direct Report");
                        }
                    }
                }
                helper.Upsert(employee);

            }
        }

        [Test]
        public void FixEmployeeIncidents()
        {
            var helper = EmployeeIncident.Helper;
            var incidents = helper.GetAll();
            foreach (var i in incidents)
            {
                if (i.Location != null)
                {
                    i.Location = i.Location.AsLocation().AsLocationRef();
                }

                if (i.Employee != null)
                {
                    i.Employee = i.Employee.AsEmployee().AsEmployeeRef();
                }

                if (i.Manager != null)
                {
                    i.Manager = i.Manager.AsEmployee().AsEmployeeRef();
                }

                if (i.ReportedBy != null)
                {
                    i.ReportedBy = i.ReportedBy.AsEmployee().AsEmployeeRef();
                }

                if (i.ApprovedBy != null)
                {
                    i.ApprovedBy = i.ApprovedBy.AsEmployee().AsEmployeeRef();
                }

                helper.Upsert(i);
            }

        }

        [Test]
        public void FixTrainingCourses()
        {
            var helper = TrainingCourse.Helper;
            var courses = helper.GetAll();
            foreach (var course in courses)
            {
                if (course.Locations != null)
                {
                    foreach (var loc in course.Locations.ToList())
                    {
                        var index = course.Locations.IndexOf(loc);
                        course.Locations[index] = course.Locations[index].AsLocation().AsLocationRef();
                    }

                    helper.Upsert(course);
                }
            }

        }

        [Test]
        public void FixTrainingEvents()
        {
            var helper = TrainingEvent.Helper;
            var events = helper.GetAll();
            foreach (var i in events)
            {
                if (i.Location != null)
                {
                    i.Location = i.Location.AsLocation().AsLocationRef();
                }

                if (i.InternalInstructor != null)
                {
                    i.InternalInstructor = i.InternalInstructor.AsEmployee().AsEmployeeRef();
                }


                if (i.Attendees != null)
                {
                    foreach (var attendee in i.Attendees)
                    {
                        if (attendee.Employee != null)
                        {
                            attendee.Employee = attendee.Employee.AsEmployee().AsEmployeeRef();

                        }
                    }

                }
                helper.Upsert(i);
            }

        }
    }
}

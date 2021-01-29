using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Dashboard;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;
using RequiredActivity = DAL.HRS.Mongo.DocClass.Training.RequiredActivity;
using TrainingCourse = DAL.HRS.Mongo.DocClass.Training.TrainingCourse;

namespace DAL.HRS.Mongo.Tests.HelperTraining_Tests
{
    [TestFixture]
    public class HelperTrainingTests
    {
        private HelperTraining _helperTraining;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            _helperTraining = new HelperTraining();
        }

        [Test]
        public void UpdateAllTrainingCoursesActive()
        {
            var trainingCourses = TrainingCourse.Helper.GetAll();
            foreach (var trainingCourse in trainingCourses)
            {
                TrainingCourse.Helper.Upsert(trainingCourse);
            }
        }

        [Test]
        public void RemoveJobRequiredCoursesTextFromRequiredActivities()
        {
            var requiredActivities = RequiredActivity.Helper.Find(x => x.Description.StartsWith("Job Required Course: "))
                .ToList();
            foreach (var requiredActivity in requiredActivities)
            {
                requiredActivity.Description = requiredActivity.Description.Replace("Job Required Course: ", "");
                RequiredActivity.Helper.Upsert(requiredActivity);
            }
        }


        [Test]
        public void GetAllTrainingEvents()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var trainingEvents = _helperTraining.GetAllTrainingEvents(null);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            //foreach (var trainingEvent in trainingEvents)
            //{
            //    Console.WriteLine($"{trainingEvent.TrainingCourse.Name} Location: {trainingEvent.Location?.Office ?? "null"}");
            //}
        }

       

        [Test]
        public void GetTrainingCourseReferences()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var trainingCourseRefs= _helperTraining.GetTrainingCourseReferences();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            foreach (var trainingCourseRef in trainingCourseRefs)
            {
                Console.WriteLine($"{trainingCourseRef.Name} Location: {trainingCourseRef.ExpirationMonths}");
            }
        }

        [Test]
        public void GetTrainingCourseReferencesForLocation()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var locationId = "5dd40ee0095f595a7460a785"; // telge
            var trainingCourseRefs = _helperTraining.GetTrainingCourseReferencesForLocation(locationId);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            foreach (var trainingCourseRef in trainingCourseRefs)
            {
                Console.WriteLine($"{trainingCourseRef.Name} Location: {trainingCourseRef.ExpirationMonths}");
            }
        }

        [Test]
        public void GetTrainingCoursesWithExpirationMonths()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var queryHelper = new MongoRawQueryHelper<TrainingCourse>();
            var filter = queryHelper.FilterBuilder.Where(x => x.ExpirationMonths > 0);
            var courses = queryHelper.Find(filter);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            foreach (var trainingCourse in courses)
            {
                Console.WriteLine($"{trainingCourse.Name} Months: {trainingCourse.ExpirationMonths}");
            }
        }


        [Test]
        public void GetAllTrainingEventSupportingDocuments()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var queryHelperTraining = new MongoRawQueryHelper<TrainingEvent>();
            var queryHelperDocs = new MongoRawQueryHelper<SupportingDocument>();

            var supportDocFilter = 
                queryHelperDocs.FilterBuilder.Where(x => x.Module.Name == "Training");
            //var supportDocProject = 
            //    queryHelperDocs.ProjectionBuilder.Expression(x=> new
            //    {
            //        FileId = x.FileInfo.Id, x.BaseDocumentId, x.DocumentDate, x.FileName, x.DocumentType, x.FileSize, x.MimeType, x.Comments 
            //    });

            //var supportingDocs = queryHelperDocs.FindWithProjection(supportDocFilter, supportDocProject);
            var supportingDocs = queryHelperDocs.Find(supportDocFilter);

            var trainingFilter =
                queryHelperTraining.FilterBuilder.In(x=> x.Id, supportingDocs.Select(d=>d.BaseDocumentId) );
            var trainingEvents = queryHelperTraining.Find(trainingFilter);

            var result = trainingEvents.Select(x => new
                    {TrainingEvent = x, SupportingDocuments = supportingDocs.Where(s => s.BaseDocumentId == x.Id)})
                .ToList();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            foreach (var trainingEventDocs in result)
            {
                foreach (var supportDoc in trainingEventDocs.SupportingDocuments.OrderByDescending(x=>x.DocumentDate))
                {
                    Console.WriteLine($"{trainingEventDocs.TrainingEvent.TrainingCourse.Name} - {supportDoc.FileName}");
                }
                Console.WriteLine("============================");
            }
            //foreach (var trainingEvent in trainingEvents)
            //{
            //    Console.WriteLine($"{trainingEvent.TrainingCourse.Name} Location: {trainingEvent.Location?.Office ?? "null"}");
            //}
        }

        [Test]
        public void GetTrainingEventSupportingDocumentsNested()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var results = _helperTraining.GetTrainingEventSupportingDocumentsNested();

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine($"{results.Count} rows");

        }

        [Test]
        public void GetTrainingEventSupportingDocumentsFlat()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var results = _helperTraining.GetTrainingEventSupportingDocumentsFlat();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine($"{results.Count} rows");

        }

        [Test]
        public void FixTrainingCourseReferencesForTrainingEvent()
        {
            var queryHelper = new MongoRawQueryHelper<TrainingEvent>();
            var trainingEvents = queryHelper.GetAll();
            foreach (var trainingEvent in trainingEvents)
            {
                if (trainingEvent.TrainingCourse == null) continue;
                trainingEvent.TrainingCourse = trainingEvent.TrainingCourse.AsTrainingCourse().AsTrainingCourseRef();
                queryHelper.Upsert(trainingEvent);
            }
        }

        [Test]
        public void FixTrainingCourseReferencesForRequiredActivity()
        {
            var queryHelper = new MongoRawQueryHelper<RequiredActivity>();
            var activities = queryHelper.GetAll();
            foreach (var activity in activities)
            {
                if (activity.TrainingCourse == null) continue;
                activity.TrainingCourse = activity.TrainingCourse.AsTrainingCourse().AsTrainingCourseRef();
                queryHelper.Upsert(activity);
            }
        }

        [Test]
        public void FixTrainingCourseReferencesForEmployeeTrainingEvents()
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var employees = queryHelper.GetAll();
            foreach (var employee in employees)
            {
                var anyChanges = false;
                foreach (var employeeTrainingEvent in employee.TrainingEvents)
                {
                    anyChanges = true;
                    employeeTrainingEvent.TrainingCourse =
                        employeeTrainingEvent.TrainingCourse.AsTrainingCourse().AsTrainingCourseRef();
                }

                if (anyChanges)
                {
                    queryHelper.Upsert(employee);
                }
            }
        }

        [Test]
        public void GetAllTrainingEventsForEmployeesGrid()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var results = _helperTraining.GetAllTrainingEventsForEmployeesGrid(null);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine($"{results.Count} rows");

        }

        [Test]
        public void GetTrainingEventsGrid()
        {
            var hrsUser = HrsUser.Helper.Find(x => x.LastName == "Black").FirstOrDefault();
            var trainingEvents = _helperTraining.GetAllTrainingEventsForEmployeesGrid(hrsUser);
            foreach (var trainingEvent in trainingEvents)
            {
                Console.WriteLine($"{trainingEvent.Location.Coid} - {trainingEvent.FirstName} {trainingEvent.LastName} {trainingEvent.TrainingEventsCount}");
            }
        }

        [Test]
        public void GetTrainingEventsWithTrainingHours()
        {
            var trainingEvents = TrainingEvent.Helper.Find(x => x.TrainingHours > 0).ToList();
                
            foreach (var trainingEvent in trainingEvents)
            {
                var trainingEventRows = QngTrainingInfoModel.GetForTrainingEvent(trainingEvent).ToList();
                foreach (var model in trainingEventRows)
                {
                    Console.WriteLine($"{model.CourseName} Hours: {trainingEvent.TrainingHours}");
                }
            }
        }

        [Test]
        public void GetQngTrainingInfo()
        {
            var trainingEvents = TrainingEvent.Helper.Find(x => x.TrainingHours > 0).ToList();

            foreach (var trainingEvent in trainingEvents)
            {
                var trainingEventRows = QngTrainingInfoModel.GetForTrainingEvent(trainingEvent).ToList();
                foreach (var model in trainingEventRows)
                {
                    Console.WriteLine($"{model.CourseName} Hours: {trainingEvent.TrainingHours}");
                }
            }
        }


        [Test]
        public void GetAllTrainingEventsForEmployees()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            
            var results = _helperTraining.GetTrainingEventsForEmployee("5e4e80c6095f5969288721a1");
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine($"{results.Count} rows");

        }

        [Test]
        public void CheckForDuplicateAttendeesInTrainingEvent()
        {
            var queryHelper = new MongoRawQueryHelper<TrainingEvent>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Attendees.Any());
            var project = queryHelper.ProjectionBuilder.Expression(x => new { x.Id, x.Attendees });
            var trainingEvents = queryHelper.FindWithProjection(filter, project).ToList();
            
            var changed = new List<TrainingEvent>();
           
            foreach (var evt in trainingEvents)
            {
                var isDirty = false;
                var tmpAttendees = new List<DocClass.Training.TrainingAttendee>();

                foreach (var attendee in evt.Attendees)
                {
                    var dup = tmpAttendees.Where(x => x.Employee.Id == attendee.Employee.Id).FirstOrDefault();

                    if (dup == null)
                    {
                        tmpAttendees.Add(attendee);
                        continue;
                    }

                    isDirty = true;                    
                }

                if (isDirty)
                {
                    var tmpEvt = new TrainingEvent { Id = evt.Id };
                    tmpEvt.Attendees = tmpAttendees;
                    changed.Add(tmpEvt);
                }
                
            }

            foreach(var evt in changed)
            {
                var empEvent = TrainingEvent.Helper.FindById(evt.Id);
                empEvent.Attendees = evt.Attendees;

                TrainingEvent.Helper.Upsert(empEvent);
            }
            //var results = _helperTraining.GetTrainingEventsForEmployee("5e4e80c6095f5969288721a1");
        }

        //[Test]
        //public void GetTrainingEventEmployees()
        //{
        //    var queryHelper = new MongoRawQueryHelper<TrainingEvent>();
        //    var filter = queryHelper.FilterBuilder.Empty;
        //    var project = queryHelper.ProjectionBuilder.Expression(x => new {x.Employee, x.Attendees});
        //    var trainingEventEmployees = queryHelper.FindWithProjection(filter, project).ToList();

        //    var employees = new List<EmployeeRef>();
        //    var employeesNotInAttendance = new List<EmployeeRef>();
        //    foreach (var trainingEmployee in trainingEventEmployees)
        //    {
        //        if (employees.All(x=>x.Id != trainingEmployee.Employee.Id))
        //        {
        //            if (trainingEmployee.Attendees.All(a=>a.Employee.Id != a.Employee.Id))
        //            {
        //                employeesNotInAttendance.Add(trainingEmployee.Employee);
        //            }
        //            else
        //            {
        //                employees.Add(trainingEmployee.Employee);
        //            }
        //        }
        //    }

        //    Console.WriteLine($"Reg: {employees.Count} Not Attending: {employeesNotInAttendance.Count}");
        //}
    }
}

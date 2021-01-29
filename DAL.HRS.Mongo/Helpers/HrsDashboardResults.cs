using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Helpers
{
    public class HrsDashboardResults : BaseDashboard
    {
        public List<EmployeeAuditTrailFlatModel> EmployeeAuditTrailHistory = new List<EmployeeAuditTrailFlatModel>();

        public List<ExpiringEventModel> ExpiringEvents = new List<ExpiringEventModel>();
        public List<MedicalLeaveHistoryGridModel> MedicalLeaves = new List<MedicalLeaveHistoryGridModel>();

        public List<string> Timers = new List<string>();

        public HrsDashboardResults(HrsUser hrsUser) : base(hrsUser)
        {
        }

        public static HrsDashboardResults GetDashboardResults(HrsUser hrsUser)
        {
            var result = new HrsDashboardResults(hrsUser);
            result.Fetch();
            return result;
        }

        private void Fetch()
        {
            var onlyActive = true;
            var employees = GetEmployees(onlyActive);


            var expiringEvents = new List<ExpiringEventModel>();
            var taskExpiringEvents = new Task(() =>
            {
                expiringEvents =
                    GetExpiringTrainingEvents(employees.EmpIdList); //helperTraining.GetAllExpiringEvents(empIdList);
            });
            taskExpiringEvents.Start();


            var expiringTrainingMatrix = new List<ExpiringEventModel>();
            var taskExpiringTrainingMatrix = new Task(() =>
            {
                expiringTrainingMatrix = GetExpiringTrainingMatrix(employees.EmpIdList);
            });
            taskExpiringTrainingMatrix.Start();

            var expiringEmployeeVerifications = new List<ExpiringEventModel>();
            var taskExpiringDocs = new Task(() =>
            {
                expiringEmployeeVerifications = GetEmployeeVerifications(employees.EmpIdList);
            });
            taskExpiringDocs.Start();

            var expiringEmployeeEducationCertifications = new List<ExpiringEventModel>();
            var taskEmployeeEducationCertifications = new Task(() =>
            {
                expiringEmployeeEducationCertifications = GetExpiringEducationCerts(employees.EmpIdList);
            });
            taskEmployeeEducationCertifications.Start();


            var expiringTrainingCertifications = new List<ExpiringEventModel>();
            var taskExpiringCerts = new Task(() =>
            {
                expiringTrainingCertifications = GetExpiringTrainingCerts(employees.EmpIdList);
            });
            taskExpiringCerts.Start();

            var taskEmployeeAuditHistory = new Task(GetAuditTrailHistory);
            taskEmployeeAuditHistory.Start();

            var taskMedicalLeaves = new Task(() =>
            {
                var helperMedicalInfo = new HelperMedicalInfo();

                MedicalLeaves = helperMedicalInfo.GetMedicalLeaves(employees.Filter);
            });
            taskMedicalLeaves.Start();


            Task.WaitAll(
                taskExpiringEvents,
                taskExpiringTrainingMatrix,
                taskExpiringDocs,
                taskExpiringCerts,
                taskEmployeeEducationCertifications,
                taskEmployeeAuditHistory,
                taskMedicalLeaves);

            ExpiringEvents.AddRange(expiringEvents);
            ExpiringEvents.AddRange(expiringTrainingMatrix);
            ExpiringEvents.AddRange(expiringEmployeeVerifications);
            ExpiringEvents.AddRange(expiringTrainingCertifications);
            ExpiringEvents.AddRange(expiringEmployeeEducationCertifications);
        }

        private void GetAuditTrailHistory()
        {
            var sw = new Stopwatch();
            sw.Start();

            var allEmployees = GetEmployees(false);


            var fromDate = DateTime.Now.AddDays(-90);
            var auditTrailHistory = GetNormalAuditTrailHistory();
            var compensationHistory = GetCompensationHistory();
            var newHires = GetNewHires();

            auditTrailHistory.AddRange(newHires);
            auditTrailHistory.AddRange(compensationHistory);
            EmployeeAuditTrailHistory = auditTrailHistory;

            sw.Stop();
            Timers.Add($"GetEmployeeAuditHistory - Elapsed: {sw.Elapsed}");

            List<EmployeeAuditTrailFlatModel> GetNewHires()
            {
                var newHiresFilter = Employee.Helper.FilterBuilder.Where(x =>
                    x.OriginalHireDate != null && x.OriginalHireDate >= fromDate &&
                    x.OriginalHireDate <= DateTime.Now.Date);
                var newHiresProject = Employee.Helper.ProjectionBuilder.Expression(x =>
                    new EmployeeAuditTrailFlatModel
                    {
                        Employee = new EmployeeRef(x),
                        FieldName = "OriginalHireDate",
                        Id = Guid.NewGuid(),
                        Module = "EmployeeDetails",
                        OldValue = "<null>",
                        NewValue = x.OriginalHireDate.ToString(),
                        UpdatedAt = x.CreateDateTime
                    });
                newHires = Employee.Helper.FindWithProjection(newHiresFilter, newHiresProject).ToList();
                return newHires;
            }

            List<EmployeeAuditTrailFlatModel> GetNormalAuditTrailHistory()
            {
                auditTrailHistory =
                    new EmployeeAuditTrailHistoryModel(fromDate, "EmployeeDetails", _hrsUser, allEmployees.EmpIdList)
                        .AuditTrailHistory;

                return auditTrailHistory;
            }


            List<EmployeeAuditTrailFlatModel> GetCompensationHistory()
            {
                compensationHistory = new List<EmployeeAuditTrailFlatModel>();

                var compensationChanges = EmployeeAuditTrail.Helper
                    .Find(x => x.UpdatedAt >= DateTime.Now.Date.AddDays(-90) &&
                               (x.AuditTrail.CompensationChanges.ValueChanges.Any()
                                ||
                                x.AuditTrail.CompensationChanges.ValueChanges.Any())
                               && allEmployees.EmpIdList.Contains(x.AuditTrail.Employee.Id)).ToList();


                foreach (var compensationChange in compensationChanges)
                    if (compensationHistory.All(x => x.Employee.Id != compensationChange.AuditTrail.Employee.Id))
                        compensationHistory.Add(new EmployeeAuditTrailFlatModel
                        {
                            Employee = compensationChange.AuditTrail.Employee,
                            FieldName = "Compensation Modified",
                            Module = "Compensation",
                            UpdatedAt = compensationChange.UpdatedAt,
                            UpdatedBy = compensationChange.UpdatedBy
                        });

                return compensationHistory;
            }
        }

        private List<ExpiringEventModel> GetExpiringEducationCerts(List<string> empIdList)
        {
            var sw = new Stopwatch();
            sw.Start();

            var result = new List<ExpiringEventModel>();
            var helper = Employee.Helper;
            var toDate = DateTime.Now.Date.AddMonths(6);

            var objectIdList = empIdList.Select(x => new ObjectId(x)).ToList();

            var filter = helper.FilterBuilder.In(x => x.Id, objectIdList) &
                         helper.FilterBuilder.Where(x =>
                             x.EducationCertifications.Any(e =>
                                 e.CertExpiration != null && e.CertExpiration <= toDate && !e.Dismissed));
            var project = helper.ProjectionBuilder.Expression(x =>
                new {x.Id, x.EducationCertifications, x.Location, EmployeeRef = x.AsEmployeeRef()});

            var emps = helper.FindWithProjection(filter, project).ToList();
            foreach (var emp in emps)
            foreach (var educationCertification in emp.EducationCertifications)
                if (educationCertification.CertExpiration <= toDate && !educationCertification.Dismissed)
                    result.Add(new ExpiringEventModel
                    {
                        DueDate = educationCertification.CertExpiration ?? DateTime.MinValue,
                        Location = emp.Location,
                        Employee = emp.EmployeeRef,
                        Type = "EducationCertification"
                    });

            sw.Stop();
            Timers.Add($"GetExpiringEducationCerts - Elapsed: {sw.Elapsed}");

            return result;
        }

        private List<ExpiringEventModel> GetExpiringTrainingCerts(List<string> empIdList)
        {
            var sw = new Stopwatch();
            sw.Start();

            var result = new List<ExpiringEventModel>();
            //return result;
            var helper = RequiredActivity.Helper;
            var filter = helper.FilterBuilder.Where(x => x.Type == RequiredActivityType.TrainingCertification)
                         &
                         helper.FilterBuilder.In(x => x.Employee.Id, empIdList.AsEnumerable())
                         &
                         helper.FilterBuilder.Or(
                             helper.FilterBuilder.Where(x => x.RevisedCompletionDeadline == null &&
                                                             x.CompletionDeadline != null &&
                                                             x.CompletionDeadline <= DateTime.Now.Date.AddMonths(6)),
                             helper.FilterBuilder.Where(x =>
                                 x.RevisedCompletionDeadline != null &&
                                 x.RevisedCompletionDeadline <= DateTime.Now.Date.AddMonths(6))
                         );


            var project = helper.ProjectionBuilder.Expression(x =>
                new ExpiringEventModel
                {
                    DueDate = x.RevisedCompletionDeadline ?? x.CompletionDeadline ?? DateTime.MinValue,
                    Employee = x.Employee,
                    Location = x.Employee.Location,
                    TrainingCourse = x.TrainingCourse,
                    Type = "TrainingCertification"
                });

            result = helper.FindWithProjection(filter, project).ToList();

            sw.Stop();
            Timers.Add($"GetExpiringTrainingCerts - Elapsed: {sw.Elapsed}");

            return result;
        }

        private List<ExpiringEventModel> GetEmployeeVerifications(List<string> empIdList)
        {
            var sw = new Stopwatch();
            sw.Start();

            var result = new List<ExpiringEventModel>();
            var helper = Employee.Helper;
            var toDate = DateTime.Now.Date.AddMonths(6);

            var objectIdList = empIdList.Select(x => new ObjectId(x)).ToList();

            var filter = helper.FilterBuilder.In(x => x.Id, objectIdList) &
                         helper.FilterBuilder.Where(x =>
                             x.EmployeeVerifications.Any(e =>
                                 e.DocumentExpiration != null && e.DocumentExpiration <= toDate));
            var project = helper.ProjectionBuilder.Expression(x =>
                new {x.Id, x.EmployeeVerifications, x.Location, EmployeeRef = x.AsEmployeeRef()});

            var emps = helper.FindWithProjection(filter, project).ToList();
            foreach (var emp in emps)
            foreach (var verification in emp.EmployeeVerifications)
                if (verification.DocumentExpiration <= toDate && !verification.Dismissed)
                    result.Add(new ExpiringEventModel
                    {
                        DueDate = verification.DocumentExpiration ?? DateTime.MinValue,
                        Location = emp.Location,
                        Employee = emp.EmployeeRef,
                        Type = "EmployeeVerification"
                    });

            Timers.Add($"GetEmployeeVerifications - Elapsed: {sw.Elapsed}");

            return result;
        }

        private List<ExpiringEventModel> GetExpiringTrainingMatrix(List<string> empIdList)
        {
            var sw = new Stopwatch();
            sw.Start();

            var result = new List<ExpiringEventModel>();
            //return result;
            var helper = RequiredActivity.Helper;
            var endDate = DateTime.Now.Date.AddMonths(6);

            var filterRequiredActivities = helper.FilterBuilder.In(x => x.Employee.Id, empIdList);
            filterRequiredActivities = filterRequiredActivities &
                                       helper.FilterBuilder.Where(x =>
                                           x.ActivityType.Code == "Training");

            filterRequiredActivities = filterRequiredActivities &
                                       helper.FilterBuilder.Where(x =>
                                           x.TrainingCourse.GroupClassification.Code.Contains("HR"));
            filterRequiredActivities = filterRequiredActivities & helper.FilterBuilder.Or(
                helper.FilterBuilder.Where(x => x.RevisedCompletionDeadline == null &&
                                                x.CompletionDeadline != null &&
                                                x.CompletionDeadline <= endDate && x.DateCompleted == null),
                helper.FilterBuilder.Where(x =>
                    x.RevisedCompletionDeadline != null &&
                    x.RevisedCompletionDeadline <= endDate && x.DateCompleted == null));


            var project =
                RequiredActivity.Helper.ProjectionBuilder.Expression(x =>
                    new ExpiringEventModel
                    {
                        Employee = x.Employee,
                        Location = x.Employee.Location,
                        TrainingCourse = x.TrainingCourse,
                        Type = "Training Matrix",
                        DueDate = x.RevisedCompletionDeadline ?? x.CompletionDeadline ?? DateTime.MinValue
                    });

            result = RequiredActivity.Helper.FindWithProjection(filterRequiredActivities, project).ToList()
                .OrderBy(x => x.DueDate).ToList();
            sw.Stop();

            Timers.Add($"GetExpiringTrainingMatrix - Elapsed: {sw.Elapsed}");


            return result;
        }


        private List<ExpiringEventModel> GetExpiringTrainingEvents(List<string> empIdList)
        {
            var sw = new Stopwatch();
            sw.Start();

            var result = new List<ExpiringEventModel>();
            //return result;
            var queryHelper = new MongoRawQueryHelper<TrainingEvent>();

            //var filterEvents = queryHelper.FilterBuilder.Where(x => x.Attendees.Any());
            var fromDate = DateTime.Now.Date;
            var toDate = DateTime.Now.AddMonths(3);


            // Stage1
            // All [Hr] and [Hr and HSE] Group Classifications that have a CertificateExpiration Expiring Today through 6 months forward

            var trainingEventsFilterStage1 =
                TrainingEvent.Helper.FilterBuilder.Where(x =>
                    x.TrainingCourse != null && x.TrainingCourse.GroupClassification.Code.Contains("HR")) &
                //TrainingEvent.Helper.FilterBuilder.Gte(x => x.CertificateExpiration, fromDate) &
                TrainingEvent.Helper.FilterBuilder.Lte(x => x.CertificateExpiration, toDate);

            var trainingProject = queryHelper.ProjectionBuilder.Expression(
                x => new TrainingEventModel(x));

            var eventList = queryHelper.FindWithProjection(trainingEventsFilterStage1, trainingProject).ToList();

            foreach (var empEvent in eventList)
            {
                var attendees = empEvent.Attendees.Where(x => empIdList.Contains(x.Employee.Id)).Distinct().ToList();
                if (attendees.Count == 0) continue;


                var dueDate = empEvent.CertificateExpiration ?? empEvent.EndDate ?? DateTime.MinValue;

                foreach (var attendee in attendees)
                {
                    if (attendee.Dismissed || attendee.DateCompleted != null) continue;

                    result.Add(new ExpiringEventModel
                    {
                        Employee = attendee.Employee,
                        DueDate = dueDate,
                        Location = empEvent?.Location ?? attendee.Employee.Location,
                        TrainingCourse = empEvent.TrainingCourse,
                        Type = "Training Event"
                    });
                }
            }

            // Stage 2
            // TrainingEvents not completed with expected completion dates 6 months forward
            //var trainingEventsFilterStage2 =
            //    TrainingEvent.Helper.FilterBuilder.Where(x => x.TrainingCourse != null && x.TrainingCourse.GroupClassification.Code.Contains("HR")) &
            //    TrainingEvent.Helper.FilterBuilder.Lte(x => x.EndDate, DateTime.Now.Date.AddDays(90))
            //    &
            //    TrainingEvent.Helper.FilterBuilder.Where(x => x.Attendees.Any(a => empIdList.Contains(a.Employee.Id)));

            //eventList = TrainingEvent.Helper.FindWithProjection(trainingEventsFilterStage2, trainingProject)
            //    .ToList();

            ////eventList = eventList.Where(x => x.Attendees.Any(a => empIdList.Contains(a.Employee.Id))).ToList();

            //foreach (var empEvent in eventList)
            //{
            //    var attendees = empEvent.Attendees;
            //    if (attendees.Count == 0) continue;

            //    foreach (var attendee in attendees.Where(x => empIdList.Contains(x.Employee.Id)))
            //    {

            //        if ((attendee.Dismissed) || (attendee.DateCompleted != null)) continue;

            //        var dueDate = empEvent.CertificateExpiration ?? empEvent.EndDate ?? DateTime.MinValue;

            //        result.Add(new ExpiringEventModel()
            //        {
            //            Employee = attendee.Employee,
            //            DueDate = dueDate,
            //            Location = empEvent?.Location ?? attendee.Employee.Location,
            //            TrainingCourse = empEvent.TrainingCourse,
            //            Type = "Training Event"
            //        });

            //    }

            //}

            sw.Stop();

            Timers.Add($"GetExpiringTrainingEvents - Elapsed: {sw.Elapsed}");

            return result.OrderBy(x => x.DueDate).ToList();
        }
    }
}
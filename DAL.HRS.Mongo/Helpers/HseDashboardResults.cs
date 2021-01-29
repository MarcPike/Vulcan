using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.Helpers
{
    public class HseDashboardResults : BaseDashboard
    {
        public List<ExpiringEventModel> ExpiringEvents = new List<ExpiringEventModel>();
        public List<IncidentSeverityModel> IncidentsSeverity = new List<IncidentSeverityModel>();
        public List<IncidentsYearToYearModel> IncidentsYearToYear = new List<IncidentsYearToYearModel>();

        public HseDashboardResults(HrsUser hrsUser) : base(hrsUser)
        {
        }

        public static HseDashboardResults GetDashboardResults(HrsUser hrsUser)
        {
            var result = new HseDashboardResults(hrsUser);
            result.Fetch();
            return result;
        }

        private void Fetch()
        {
            var employees = GetEmployees(true);


            var expiringEvents = new List<ExpiringEventModel>();
            var taskExpiringEvents = new Task(() =>
            {
                expiringEvents =
                    GetExpiringTrainingEvents(employees.EmpIdList); //helperTraining.GetAllExpiringEvents(empIdList);
            });
            taskExpiringEvents.Start();


            var helperHse = new HelperHse();
            var taskIncidentsSeverity = new Task(() =>
            {
                IncidentsSeverity = helperHse.GetIncidentSeverityByLocation();
            });
            taskIncidentsSeverity.Start();
            Task.WaitAll(taskExpiringEvents, taskIncidentsSeverity);

            var taskIncidentsYearToYear = new Task(() => { IncidentsYearToYear = helperHse.GetIncidentsYearToYear(); });
            taskIncidentsYearToYear.Start();

            Task.WaitAll(taskExpiringEvents, taskIncidentsSeverity, taskIncidentsYearToYear);
            ExpiringEvents = expiringEvents;
        }

        private List<ExpiringEventModel> GetExpiringTrainingEvents(List<string> empIdList)
        {
            var result = new List<ExpiringEventModel>();
            //return result;
            var queryHelper = new MongoRawQueryHelper<TrainingEvent>();

            //var filterEvents = queryHelper.FilterBuilder.Where(x => x.Attendees.Any());
            var toDate = DateTime.Now.AddMonths(3);


            // Stage1
            // All [Hr] and [Hr and HSE] Group Classifications that have a CertificateExpiration Expiring Today through 6 months forward

            var trainingEventsFilterStage1 =
                TrainingEvent.Helper.FilterBuilder.Where(x =>
                    x.TrainingCourse != null && x.TrainingCourse.GroupClassification.Code == "HSE") &
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

            return result.OrderBy(x => x.DueDate).ToList();
        }
    }
}
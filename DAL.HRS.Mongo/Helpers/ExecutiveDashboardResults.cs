using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public class ExecutiveDashboardResults
    {
        public ExecutiveDashboardResults()
        {
        }

        public static ExecutiveDashboardResults GetDashboardResults()
        {
            var result = new ExecutiveDashboardResults();
            result.Fetch();
            return result;
        }

        private void Fetch()
        {
            Task taskHeadCount = new Task(() =>
            {
                var helperEmployee = new HelperEmployee();
                GlobalHeadCount = helperEmployee.GetGlobalHeadCount();
            });
            taskHeadCount.Start();

            var helperHse = new HelperHse();
            Task taskIncidentsSeverity = new Task(() =>
            {
                IncidentsSeverity = helperHse.GetIncidentSeverityByLocation();
            });
            taskIncidentsSeverity.Start();

            Task taskIncidentsYearToYear = new Task(() =>
            {
                IncidentsYearToYear = helperHse.GetIncidentsYearToYear();
            });
            taskIncidentsYearToYear.Start();

            Task.WaitAll(taskHeadCount, taskIncidentsSeverity, taskIncidentsYearToYear);

        }

        public List<IncidentsYearToYearModel> IncidentsYearToYear { get; set; }

        public List<IncidentSeverityModel> IncidentsSeverity { get; set; }

        public List<GlobalHeadcountModel> GlobalHeadCount { get; set; }
    }
}
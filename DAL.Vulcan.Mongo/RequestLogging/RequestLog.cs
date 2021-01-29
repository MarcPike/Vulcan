using BLL.EMail;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Vulcan.Mongo.RequestLogging
{
    public class RequestLog : BaseDocument
    {

        public static MongoRawQueryHelper<RequestLog> Helper = new MongoRawQueryHelper<RequestLog>();
        public string Method { get; set; }
        public string PathValue { get; set; }
        public int Response { get; set; }

        public static void PerformRestart()
        {
            if (EnvironmentSettings.RunningLocal) return;

            DeleteRowsOlderThanSixHours();

            EmailLast100Request();
        }

        private static void EmailLast100Request()
        {
            var emptyFilter = Helper.FilterBuilder.Empty;
            var order = Helper.SortBuilder.Descending(x => x.CreateDateTime);
            var project = Helper.ProjectionBuilder.Expression(x => new {x.CreateDateTime, x.PathValue, x.Response});

            var requestLogs = Helper.FindWithProjection(emptyFilter, project, order).Take(100).ToList();


            if (!requestLogs.Any())
            {
                return;
            }

            var emailBuilder = new EMailBuilder {Subject = $"Post Vulcan Restart : Last 100 Requests : {EnvironmentSettings.CurrentEnvironment.ToString()}"};
            emailBuilder.Recipients.Add("marc.pike@howcogroup.com");
            emailBuilder.Recipients.Add("isidro.gallegos@howcogroup.com");

            var body = new StringBuilder();
            foreach (var requestLog in requestLogs)
            {
                body.AppendLine(
                    $"TimeOf: {requestLog.CreateDateTime.TimeOfDay} Route: {requestLog.PathValue} Response: {requestLog.Response}");
            }


            emailBuilder.Body = body.ToString();
            emailBuilder.Send();
        }

        private static void DeleteRowsOlderThanSixHours()
        {
            var filter = RequestLog.Helper.FilterBuilder.Where(x => x.CreateDateTime <= DateTime.Now.Date.AddHours(-12));
            RequestLog.Helper.DeleteMany(filter);
        }

        public static RequestLog Create(string method, string path, int response)
        {
            return Helper.Upsert(new RequestLog()
            {
                Method = method,
                PathValue = path,
                Response = response
            });
        }

        public struct RequestLogModel
        {
            public DateTime DateOf { get; set; }
            public string Method { get; set; }
            public string PathValue { get; set; }
            public int Response { get; set; }
        }



        public static List<RequestLogModel> LastHour()
        {
            return Helper.Find(x => x.CreateDateTime >= DateTime.Now.AddHours(-1) && x.Response != 204).ToList().Select(x => new RequestLogModel()
            {
                DateOf = x.CreateDateTime,
                Method = x.Method,
                PathValue = x.PathValue,
                Response = x.Response
            }).ToList();
        }

        public static List<RequestLogModel> Between10MinutesOfTime(DateTime dateTime)
        {
            return Helper.Find(x => x.CreateDateTime >= dateTime.AddMinutes(-10) && x.CreateDateTime <= dateTime && x.Response != 204).ToList().Select(x => new RequestLogModel()
            {
                DateOf = x.CreateDateTime,
                Method = x.Method,
                PathValue = x.PathValue,
                Response = x.Response
            }).ToList();
        }



        public static List<RequestLogModel> LastDay()
        {
            return Helper.Find(x => x.CreateDateTime >= DateTime.Now.AddDays(-1) && x.Response != 204).ToList().Select(x => new RequestLogModel()
            {
                DateOf = x.CreateDateTime,
                Method = x.Method,
                PathValue = x.PathValue,
                Response = x.Response
            }).ToList();
        }

    }
}

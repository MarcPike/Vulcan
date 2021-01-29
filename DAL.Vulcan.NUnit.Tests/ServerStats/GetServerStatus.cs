using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.StockItemCacheLog;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.RequestLogging;
using DocumentFormat.OpenXml.Drawing;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.ServerStats
{
    [TestFixture]
    public class GetServerStatus
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void StockItemCacheLoadsToday()
        {
            var coidList = new List<string>()
            {
                "INC",
                "CAN",
                "EUR",
                "DUB",
                "MSA",
                "SIN"
            };

            Console.WriteLine("StockItem loads in the last 24 hours");
            Console.WriteLine("====================================");
            foreach (var coid in coidList)
            {
                var today = DateTime.Now.AddHours(-24);
                var loadedToday = StockItemCacheLogValue.Helper.Find(x => x.StockItemsQueryModel.Coid == coid && x.OccurredAt >= today).ToList()
                    .OrderByDescending(x => x.OccurredAt).ToList();
                var coidIndex = 0;
                foreach (var stockItemCacheLogValue in loadedToday)
                {
                    Console.WriteLine($"{coidIndex} - StockItem Last load for {coid} Occurred: {stockItemCacheLogValue.OccurredAt} by SalesPerson: {stockItemCacheLogValue.SalesPerson.FullName}");
                }

            }


        }

        [Test]
        public void RequestsLastHour()
        {
            var requestLogs = RequestLog.LastHour().OrderByDescending(x=>x.DateOf).ToList();
            Console.WriteLine($"Last Hour requests ({requestLogs.Count}) processed");
            Console.WriteLine("======================================");
            var index = 0;
            foreach (var requestLog in requestLogs)
            {
                Console.WriteLine($"{index++} - TimeOf: {requestLog.DateOf.TimeOfDay} : {requestLog.PathValue} : {requestLog.Response}");
            }
        }

        [Test]
        public void RequestByHour()
        {
            var timeOf = DateTime.Now.Date.AddHours(6);
            for (int i = 1; i <= 12; i++)
            {
                var startTime = timeOf.AddHours(i);
                var endTime = startTime.AddHours(1).AddMinutes(-1);
                var filterCount =
                    RequestLog.Helper.FilterBuilder.Where(x =>
                        x.CreateDateTime >= startTime && x.CreateDateTime <= endTime);
                var count = RequestLog.Helper.GetRowCount(filterCount);

                var errorsCount = RequestLog.Helper.FilterBuilder.Where(x =>
                    x.CreateDateTime >= startTime && x.CreateDateTime <= endTime && x.Response != 200 && x.Response != 404);
                var errors = RequestLog.Helper.GetRowCount(errorsCount);

                Console.WriteLine($"{startTime.TimeOfDay} Request per Hour: {count} Errors: {errors}");

                if (errors > 0)
                {
                    var errorList = RequestLog.Helper.Find(errorsCount).ToList();
                    foreach (var requestLog in errorList)
                    {
                        Console.WriteLine($"  {requestLog.Response} - {requestLog.Method} {requestLog.PathValue}");
                    }
                }

                //Console.WriteLine($"{startTime.TimeOfDay.ToString("g")} Request per Hour: {count}");

            }
        }

        [Test]
        public void GetAllCodes401()
        {
            var requestLogs = RequestLog.Helper.Find(x => x.Response == 401 && x.PathValue.Contains("SaveQuote")).ToList()
                .OrderByDescending(x => x.CreateDateTime).ToList();
            var index = 0;
            foreach (var requestLog in requestLogs)
            {
                Console.WriteLine($"{index++} - TimeOf: {requestLog.CreateDateTime} : {requestLog.PathValue} : {requestLog.Response}");
            }

        }


        [Test]
        public void ErrorsBetween10MinutesOfTime()
        {
            var dateTime = DateTime.Parse("11/9/2020 10:34am");
            var requestLogs = RequestLog.Between10MinutesOfTime(dateTime).Where(x=>x.Response != 200);
            Console.WriteLine($"10 minutes and up to {dateTime}");
            Console.WriteLine("==================================================");
            foreach (var requestLog in requestLogs)
            {
                Console.WriteLine($"TimeOf: {requestLog.DateOf.TimeOfDay} : {requestLog.PathValue} : {requestLog.Response}");
            }

        }

        [Test]
        public void ErrorsBetweenOneHourOfTime()
        {
            var maxDate = DateTime.Parse("9/17/2020 11:35am");
            var minDate = maxDate.AddHours(-1);
            var requestLogs = RequestLog.Helper.Find(x=> x.CreateDateTime >= minDate && x.CreateDateTime <= maxDate && x.Response != 200).ToList().OrderByDescending(x=>x.CreateDateTime);
            Console.WriteLine($"All errors prior to {maxDate}");
            Console.WriteLine("==================================================");
            foreach (var requestLog in requestLogs)
            {
                Console.WriteLine($"TimeOf: {requestLog.CreateDateTime.DayOfWeek} {requestLog.CreateDateTime.TimeOfDay} : {requestLog.PathValue} : {requestLog.Response}");
            }

        }

        [Test]
        public void AllErrorsStatusCodeNotEqualTo200()
        {
            var requestLogs = RequestLog.Helper.Find(x => x.Response != 200).ToList().OrderByDescending(x => x.CreateDateTime);
            Console.WriteLine($"All errors");
            Console.WriteLine("==================================================");
            foreach (var requestLog in requestLogs)
            {
                Console.WriteLine($"TimeOf: {requestLog.CreateDateTime.DayOfWeek} {requestLog.CreateDateTime.TimeOfDay} : {requestLog.PathValue} : {requestLog.Response}");
            }

        }


        [Test]
        public void GetSalesPerson()
        {
            var userId = "5f5de48205d21f28d84853a9";

            var helperUser = new HelperUser(new HelperPerson());

            var crmUser = helperUser.GetCrmUser("vulcancrm", userId);

            if (crmUser == null)
            {
                Console.WriteLine("No SalesPerson found");
                return;
            }
            Console.WriteLine($"{crmUser.User.GetFullName()}");
        }

    }
}

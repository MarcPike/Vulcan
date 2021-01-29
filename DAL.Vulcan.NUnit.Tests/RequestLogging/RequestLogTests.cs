using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.RequestLogging;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.RequestLogging
{
    [TestFixture]
    public class RequestLogTests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void LastHour()
        {
            var lastHourRequests = RequestLog.LastHour();
            foreach (var lastHourRequest in lastHourRequests)
            {
                Console.WriteLine($"{lastHourRequest.DateOf.TimeOfDay} {lastHourRequest.Method} {lastHourRequest.PathValue} {lastHourRequest.Response}");
            }
        }

        [Test]
        public void LastDay()
        {
            var lastHourRequests = RequestLog.LastDay();
            foreach (var lastHourRequest in lastHourRequests)
            {
                Console.WriteLine($"{lastHourRequest.DateOf.TimeOfDay} {lastHourRequest.Method} {lastHourRequest.PathValue} {lastHourRequest.Response}");
            }
        }


    }
}

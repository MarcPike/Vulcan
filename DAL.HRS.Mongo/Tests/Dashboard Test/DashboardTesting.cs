using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Dashboard_Test
{
    [TestFixture]
    public class DashboardTesting
    {
       
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsQualityControl();
        }

        [Test]
        public void HrsDashboard()
        {
            var sw = new Stopwatch();
            sw.Start();

            var hrsUser = HrsUser.Helper.Find(x => x.FirstName == "Lori").First();
            var helperDashboard = new HelperDashboard();
            var result = helperDashboard.GetHrsDashboard(hrsUser);

            sw.Stop();
            Console.WriteLine(
                $"Elapsed: {sw.Elapsed} - Expiring Events: {result.ExpiringEvents.Count} Medical Info: {result.MedicalLeaves.Count} Changes: {result.EmployeeAuditTrailHistory.Count}");

            Console.WriteLine(
                $"Training Event rows found: {result.ExpiringEvents.Count(x => x.Type == "Training Event")}");
            foreach (var timer in result.Timers) Console.WriteLine(timer);
        }

        [Test]
        public void HseDashboard()
        {
            var sw = new Stopwatch();
            sw.Start();

            var hrsUser = HrsUser.Helper.Find(x => x.FirstName == "Lori").First();
            var helperDashboard = new HelperDashboard();
            var result = helperDashboard.GetHseDashboard(hrsUser);

            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");
        }

        [Test]
        public async Task CallSomeCountTask()
        {
            var number = await SomeCountTask();
            Console.WriteLine($"Done: {number}");
        }

        private async Task<int> SomeCountTask()
        {
            var result = 0;
            var task = new Task(() =>
            {
                Thread.Sleep(1000);
                result = 1000;
            });
            task.Start();
            await task;

            return result;
        }
        
        // [Test]
        // public void FindEmployeeVerificationWithDismissed()
        // {
        //     var helper = DAL.HRS.Mongo.DocClass.AuditTrails.EmployeeAuditTrail.Helper;
        //
        //     var empsWithVer = helper.Find(x => x.Original.EmployeeVerifications.Any()).ToList();
        //
        //     foreach (var auditTrail in empsWithVer)
        //     {
        //         var employeeVerifications = auditTrail.Original.EmployeeVerifications.ToList();
        //         foreach (var ver in employeeVerifications)
        //         {
        //             var doc = ver.ToBsonDocument();
        //             if (doc.IndexOfName("Dismissed") > -1)
        //             {
        //                 Console.WriteLine(auditTrail.Current.PayrollId);
        //             }
        //         }
        //     }
        // }
    }
}
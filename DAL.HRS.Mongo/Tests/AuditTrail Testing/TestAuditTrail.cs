using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.AuditTrail_Testing
{
    [TestFixture()]
    class TestAuditTrail
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void BasicTest()
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var incident = queryHelper.FindById("5ddebf52095f595388fae86d");
            Assert.IsNotNull(incident);

            // clear changeHistory for this test
            incident.ChangeHistory.Clear();
            queryHelper.Upsert(incident);

            var model = new EmployeeIncidentModel(incident);
            model.HrsUser = GetTestUser();
            model.IIQ.EmployeeAuthorized = true;
            model.IIQ.EmployeeAuthorizedComment = "Marc playing around again";

            var helperEmployeeIncidents = new HelperEmployeeIncidents();

            var newModel = helperEmployeeIncidents.SaveEmployeeIncident(model);

            foreach (var changeHistory in newModel.ChangeHistory)
            {
                Console.WriteLine(ObjectDumper.Dump(changeHistory));
            }

        }

        private HrsUserRef GetTestUser()
        {
            var queryHelper = new MongoRawQueryHelper<HrsUser>();
            var filter = queryHelper.FilterBuilder.Where(x => x.LastName == "Reese");
            var user = queryHelper.Find(filter).FirstOrDefault();
            Assert.IsNotNull(user);
            return user.AsHrsUserRef();
        }

        class MyClass 
        {
            public string SomeValue { get; set; }
        }
        public static Type GetObjectType(Expression<Func<object>> expr)
        {
            var obj = ((UnaryExpression)expr.Body).Operand;
            return (Type)((PropertyInfo)obj.GetType()
                .GetProperty("Type", BindingFlags.Instance | BindingFlags.Public)).GetValue(obj);
        }
        [Test]
        public void GetTypeOfNullProperty()
        {
            var x = new MyClass();
            
        }

        [Test]
        public void SetMissingModifiedToYesterday()
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var filter = queryHelper.FilterBuilder.Where(x => x.ChangeHistory.Count > 0);
            var incidentsWithChangeHistory = queryHelper.Find(filter);

            foreach (var employeeIncident in incidentsWithChangeHistory)
            {
                var modified = false;
                foreach (var changeHistory in employeeIncident.ChangeHistory)
                {
                    if (changeHistory.ModifiedDate < DateTime.Parse("12/1/2019"))
                    {
                        modified = true;
                        changeHistory.ModifiedDate = DateTime.Now.Date.AddDays(-1);
                    }
                }
                if (modified) queryHelper.Upsert(employeeIncident);
            }



        }
    }
}

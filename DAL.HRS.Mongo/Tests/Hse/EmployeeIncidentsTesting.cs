using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.Hse
{
    [TestFixture()]
    public class EmployeeIncidentsTesting
    {
        private HelperEmployeeIncidents _helperEmployeeIncidents;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _helperEmployeeIncidents = new HelperEmployeeIncidents();
        }

        [Test]
        public void GetEmployeeIncidentModel()
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var incident = queryHelper.FindById("5ddebf54095f595388fae898");
            var model = new EmployeeIncidentModel(incident);
            Console.WriteLine(model);
        }

        [Test]
        public void GetAllEmployeeIncidents()
        {
            var employeeIncidents = _helperEmployeeIncidents.GetAllEmployeeIncidents();
            foreach (var employeeIncidentModel in employeeIncidents)
            {
                Console.WriteLine(employeeIncidentModel.Employee?.FullName ?? "unknown employee");
            }
        }

        [Test]
        public void SetMissingIncidentId()
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var filter = queryHelper.FilterBuilder.Where(x => x.IncidentId == 0);
            var incidentsMissingId = queryHelper.Find(filter).ToList();
            foreach (var employeeIncident in incidentsMissingId)
            {
                employeeIncident.IncidentId = EmployeeIncident.GetNextIncidentId();
                queryHelper.Upsert(employeeIncident);
            }

        }
    }
}

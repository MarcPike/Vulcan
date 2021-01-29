using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Import.Tests
{
    [TestFixture()]
    public class ConvertIncidentAndSeverityTypesToProperties
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        //[Test]
        //public void Execute()
        //{
        //    var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
        //    var incidents = queryHelper.GetAll();
        //    foreach (var incident in incidents)
        //    {
        //        incident.NearMissTypeCode = PropertyBuilder.New("NearMissTypeCode", "Type of Near Miss",
        //            incident.NearMissTypeCode.Prefix ?? incident.NearMissTypeCode.Name, incident.NearMissTypeCode.Name);
        //        incident.SeverityTypeCode = PropertyBuilder.New("SeverityTypeCode", "Type of Severity",
        //            incident.SeverityTypeCode.Name, incident.SeverityTypeCode.Name);
        //        queryHelper.Upsert(incident);
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.MergeProperties
{
    [TestFixture]
    public class MergePropertyExecutor
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void MergeEmployeeDetails()
        {
            var merger = new MergeProperty();
            //merger.Execute("Employee DetailsDocumentType", "EmployeeDocumentType", "Howco", true);
            merger.Execute("RequiredActivityDocumentType", "Required ActivitiesDocumentType", "Howco", true);
        }
    }
}

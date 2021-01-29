using System.Linq;
using DAL.HRS.Import.ImportHrs;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Import.Tests
{
    [TestFixture]
    public class RunSensitizerForAllEmployeesInQC
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsQualityControl();
        }

        [Test]
        public void VerifyThatWeAreInQuality()
        {
            var test = new TestClass();
            TestClass.Helper.Upsert(test);
        }

        //[Test]
        //public void Execute()
        //{

        //    var employees = Employee.Helper.GetAll();
        //    foreach (var employee in employees)
        //    {
        //        DataSensitizer.Execute(employee);
        //        Employee.Helper.Upsert(employee);
        //    }
        //}
    }

}
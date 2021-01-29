using DAL.HRS.Import.ImportHrs;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Import.Tests
{
    [TestFixture]
    public class RunSensitizerForAllEmployeesInDev
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void VerifyThatWeAreInDev()
        {
            var test = new TestClass();
            TestClass.Helper.Upsert(test);
        }

        [Test]
        public void Execute()
        {

            var employees = Employee.Helper.GetAll();
            foreach (var employee in employees)
            {
                DataSensitizer.Execute(employee);
                Employee.Helper.Upsert(employee);
            }
        }
    }
}
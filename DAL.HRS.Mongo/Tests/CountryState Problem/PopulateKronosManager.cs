using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.CountryState_Problem
{
    [TestFixture]
    public class PopulateKronosManager
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            foreach (var employee in Employee.Helper.GetAll())
            {
                employee.KronosManager = employee.Manager;
                Employee.Helper.Upsert(employee);
            }
        }
    }
}

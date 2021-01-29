using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.Trim_Employee_Names
{
    [TestFixture]
    public class TrimNames
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            foreach (var employee in new RepositoryBase<Employee>().AsQueryable().ToList())
            {
                var modified = false;
                if (employee.FirstName != employee.FirstName.TrimEnd())
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} firstname not trimmed");
                    employee.FirstName = employee.FirstName.TrimEnd();
                    modified = true;
                }

                if (employee.LastName != employee.LastName.TrimEnd())
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} lastname not trimmed");
                    employee.LastName = employee.LastName.TrimEnd();
                    modified = true;
                }

                if ((employee.PreferredName != null) && (employee.PreferredName != employee.PreferredName.TrimEnd()))
                {
                    Console.WriteLine($"{employee.PreferredName} {employee.LastName} preferred name not trimmed");
                    employee.PreferredName = employee.PreferredName.TrimEnd();
                    modified = true;
                }

                if (modified) employee.SaveToDatabase();

            }
        }
    }
}

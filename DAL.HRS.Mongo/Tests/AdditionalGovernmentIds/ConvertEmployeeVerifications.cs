using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;
using DAL.Vulcan.Mongo.Base.Encryption;
using MongoDB.Driver;


namespace DAL.HRS.Mongo.Tests.AdditionalGovernmentIds
{
    [TestFixture]
    public class ConvertEmployeeVerifications
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }


        [Test]
        public void Execute()
        {
            var enc = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var filter = Employee.Helper.FilterBuilder.Where(x => x.EmployeeVerifications.Any());
            var employees = Employee.Helper.Find(filter);

            foreach (var employee in employees)
            {
                foreach (var employeeVerification in employee.EmployeeVerifications)
                {
                    employeeVerification.AdditionalGovernmentId1 = enc.Encrypt<string>(string.Empty);
                    employeeVerification.AdditionalGovernmentId2 = enc.Encrypt<string>(string.Empty);
                    var additionalGovernmentIdNumber =
                        enc.Decrypt<int>(employeeVerification.AdditionalGovernmentIdNumber);
                    var additionalGovernmentIdString =
                        enc.Decrypt<string>(employeeVerification.AdditionalGovernmentIdString);

                    if (additionalGovernmentIdNumber > 0)
                    {
                        employeeVerification.AdditionalGovernmentId1 =
                            enc.Encrypt<string>(additionalGovernmentIdNumber.ToString());
                    }

                    if ((additionalGovernmentIdString == "<Empty>") || (additionalGovernmentIdString == null))
                    {
                        additionalGovernmentIdString = string.Empty;
                    }
                    else
                    {
                        //var x = 1;
                    }

                    employeeVerification.AdditionalGovernmentId2 =
                        enc.Encrypt<string>(additionalGovernmentIdString);

                }

                Employee.Helper.Upsert(employee);
            }

        }
    }
}

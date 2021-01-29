using System;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Encryption
{
    [TestFixture()]
    public class EncryptionTest
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void BasicTest()
        {
            var encryption =  DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;
            var employee = new RepositoryBase<Employee>().AsQueryable()
                .First(x => x.LastName == "Pike" && x.FirstName == "Marc");
            var governmentId = encryption.Decrypt<string>(employee.GovernmentId);
            var governmentIdEncrypted = encryption.Encrypt(governmentId);

            Assert.AreEqual(governmentIdEncrypted, employee.GovernmentId);

            var employeeModel = new EmployeeModel(employee);
            Assert.AreEqual(employeeModel.Birthday, new DateTime(1966,12,20));

            Console.WriteLine(governmentId);




            /*
             * encryption.Decrypt<string>(rdr.GetValueOrDefault<byte[]>("MedicalNotes", null))
             *                     leave.LeaveDate = encryption.Decrypt<DateTime>(dr.GetValueOrDefault<byte[]>("DateOfLeave", null));
                    leave.ReturnDate = encryption.Decrypt<DateTime>(dr.GetValueOrDefault<byte[]>("DateOfActualReturn", null));
                    leave.HoursAway = encryption.Decrypt<decimal>(dr.GetValueOrDefault<byte[]>("HoursAway", null));
                    leave.DaysRestrictedWork = encryption.Decrypt<int>(dr.GetValueOrDefault<byte[]>("DaysRestrictedWork", null));

             */
        }

        [Test]
        public void TestNullInt()
        {
            int? testValue = null;

            var enc = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var encValue = enc.Encrypt<int?>(testValue);
            var value = enc.Decrypt<int?>(encValue);

            if (value == null)
            {
                Console.WriteLine($"value was null and decrypt is: null");
            }
            else
            {
                Console.WriteLine($"value was null and decrypt is: {value}");
            }


            testValue = 100;
            encValue = enc.Encrypt<int?>(testValue);
            value = enc.Decrypt<int?>(encValue);

            Console.WriteLine($"value set to 100 and decrypt is: {value}");


        }

    }
}

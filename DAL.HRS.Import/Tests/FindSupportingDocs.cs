using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Import.Tests
{
    [TestFixture]
    public class FindSupportingDocs
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsQualityControl();
        }

        [Test]
        public void GetCompensationSupportingDocsForEmployee()
        {
            var emp = Employee.Helper.Find(x => x.PayrollId == "DUB188").First();

            var module = SystemModuleType.Helper.Find(x => x.Name == "Compensation").First().AsSystemModuleTypeRef();

            var supportingDocs = SupportingDocument.Helper
                .Find(x => x.Module.Id == module.Id && x.BaseDocumentId == emp.Id).ToList();

            foreach (var supportingDocument in supportingDocs)
            {
                Console.WriteLine($"{supportingDocument.FileName} {supportingDocument.DocumentDate} {supportingDocument.DocumentType.Code}");
            }
        }
    }
}

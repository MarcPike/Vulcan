using System;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.FileOperations
{
    [TestFixture()]
    public class FileOperationTests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void GetAllBenefitsDocuments()
        {

            //var supportingDocs = new RepositoryBase<SupportingDocument>().AsQueryable().Where(x => x.Module.Name == "Employee Details" &&).ToList();

            //var employee = new RepositoryBase<Employee>().AsQueryable().FirstOrDefault(x => x.LastName == "Pike");
            //var files = FileAttachmentsHrs.GetAllAttachmentsForDocument(employee, "Benefits").ToList();
            //foreach (var fileInfo in files)
            //{
            //    var fileModel = new FileAttachmentModel(fileInfo);
            //    Console.WriteLine($"{fileModel.FileName} - {fileInfo.Length / 1000}kb Date: {fileModel.DocumentDate}");
            //}
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperFile_Tests
{
    [TestFixture]
    public class HelperFileTesting
    {
        private HelperFile _helperFile;
        
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            _helperFile = new HelperFile();
        }

        [Test]
        public void GetAllDocuments()
        {
            var employeeFilter = Employee.Helper.FilterBuilder.Where(x => x.PayrollId == "A053005");
            var employee = Employee.Helper.Find(employeeFilter).FirstOrDefault();

            var documents = _helperFile.GetAllAttachmentsForDocument(employee, "Employee Details");
            foreach (var fileAttachmentModel in documents)
            {
                Console.WriteLine(ObjectDumper.Dump(fileAttachmentModel));
            }

        }

        
        
        [Test]
        public void GetUniqueDocumentTypesForIncidentReportingScreen()
        {
            var supportingDocs = SupportingDocument.Helper.
                Find(x => x.Module.Name == "Incident Reporting Screen")
                .ToList();
            var uniqueDocTypeCodes = supportingDocs.Select(x => x.DocumentType.Code).Distinct().OrderBy(x => x);
            foreach (var x in uniqueDocTypeCodes)
            {
                Console.WriteLine(x);
            }

            var count = supportingDocs.Count(x => x.DocumentType.Code == "Medical Report");
            Console.WriteLine(count);
        }

        [Test]
        public void GetIncidentReportingScreenDocsThatShouldBeMoved()
        {
            var supportingDocs = SupportingDocument.Helper.
                Find(x => x.Module.Name == "Incident Reporting Screen")
                .ToList();
            var docTypesToMove = new List<string>()
            {
                "Drug Test Chain of Custody",
                "Drug Test Report",
                "Drug Test Result",
                "Insurance Claim",
                "Medical Report",
                "Return to Work",
                "Work Status Report",
                "Worker's Comp Rpt",
            };

            var docsToMove = supportingDocs.Where(x => docTypesToMove.Contains(x.DocumentType.Code)).ToList();

            var moduleToMoveTo = SystemModuleType.Helper.Find(x => x.Name == "Medical Information").First();
            
            foreach (var doc in docsToMove)
            {
                doc.ChangeModule(moduleToMoveTo).
                    ChangeDocumentType(
                        "MedicalInformationDocumentType", 
                        "Medical Information Document Type", 
                        true);
                
            }
            
        }

        [Test]
        public void GetSupportingDocumentsWithAdminNotes()
        {
            var docs = SupportingDocument.Helper.Find(x => x.AdminNotes != null).ToList();
            Console.WriteLine(docs.Count);            
        }
        

        [Test]
        public void RemoveAllCompensationDocs()
        {
            var docsRemoved = FileAttachmentsHrs.RemoveAllDocumentsForModule("Compensation");
            Console.WriteLine($"{docsRemoved} Compensation documents removed");
        }


        [Test]
        public void LoadEmployeeImages()
        {
            var files = Directory.GetFiles(@"\\s-us-web02\EmployeeImages").Select(x=>Path.GetFileName(x)).ToList();
            var filter = Employee.Helper.FilterBuilder.Where(x=>x.EmployeeImageFileName == null || x.EmployeeImageFileName == string.Empty);
            var project =
                Employee.Helper.ProjectionBuilder.Expression(x => new {x.Id, x.PayrollId, x.EmployeeImageFileName});
            var employeesMissingImage = Employee.Helper.FindWithProjection(filter, project).ToList();
            foreach (var emp in employeesMissingImage)
            {
                var fileName = emp.PayrollId + ".jpg";
                var filterThisEmp = Employee.Helper.FilterBuilder.Where(x => x.Id == emp.Id);
                if (files.Any(x => x == fileName))
                {
                    var update = Employee.Helper.UpdateBuilder.Set(x => x.EmployeeImageFileName, fileName);
                    Employee.Helper.UpdateOne(filterThisEmp, update);
                }
                else
                {
                    var update = Employee.Helper.UpdateBuilder.Set(x => x.EmployeeImageFileName, "default.jpg");
                    Employee.Helper.UpdateOne(filterThisEmp, update);
                }
            }

        }

    }
}

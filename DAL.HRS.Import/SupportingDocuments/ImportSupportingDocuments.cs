using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.SqlServer;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using MongoDB.Driver;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;

namespace DAL.HRS.Import.SupportingDocuments
{
    [TestFixture()]
    public class ImportSupportingDocuments
    {
        private RepositoryBase<Employee> _repEmployee;
        private List<int> OnlyLoadIds = new List<int>();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            _repEmployee = new RepositoryBase<Employee>();
        }

        [Test]
        public void RemoveCompensationForTesting()
        {
            var count = FileAttachmentsHrs.RemoveAllDocumentsForModule("Compensation");
            Console.WriteLine($"Removed {count} Compensation documents");
        }

        [Test]
        public void ImportOnlyIsidroAndShannen()
        {
            var isidro = _repEmployee.AsQueryable().First(x => x.FirstName == "Isidro" && x.LastName == "Gallegos").OldHrsId;
            var shannen = _repEmployee.AsQueryable().First(x => x.FirstName == "Shannen" && x.LastName == "Reese").OldHrsId;

            OnlyLoadIds.Clear();
            //OnlyLoadIds.Add(isidro);
            //OnlyLoadIds.Add(shannen);

            ImportRequiredActivityDocs();

        }

        [Test]
        public void GetStats()
        {
            //Console.WriteLine($"Benefits - {FileAttachmentsHrs.CountDocumentsForModule("Benefits")}");
            //Console.WriteLine($"Compensation - {FileAttachmentsHrs.CountDocumentsForModule("Compensation")}");
            //Console.WriteLine($"Performance - {FileAttachmentsHrs.CountDocumentsForModule("Performance")}");
            //Console.WriteLine($"Discipline - {FileAttachmentsHrs.CountDocumentsForModule("Discipline")}");
            //Console.WriteLine($"Incident Reporting Screen - {FileAttachmentsHrs.CountDocumentsForModule("Incident Reporting Screen")}");
            //Console.WriteLine($"Employee Medical Info - {FileAttachmentsHrs.CountDocumentsForModule("Employee Medical Info")}");
            //Console.WriteLine($"Employee Details - {FileAttachmentsHrs.CountDocumentsForModule("Employee Details")}");
            //Console.WriteLine($"Incident Reporting Screen - {FileAttachmentsHrs.CountDocumentsForModule("Incident Reporting Screen")}");
            Console.WriteLine($"Employment Verification - {FileAttachmentsHrs.CountDocumentsForModule("Employment Verification")}");


        }

        [Test]
        public void Find10IncidentsWithSupportingDocs()
        {
            var supportingDocs = SupportingDocument.Helper
                .Find(x => x.Module.Name == "Incident Reporting Screen" && x.DocumentType.Code == "Photograph").ToList()
                .Take(10);

            foreach (var supportingDocument in supportingDocs)
            {
                var incident = EmployeeIncident.Helper.Find(x => x.Id == supportingDocument.BaseDocumentId)
                    .FirstOrDefault();

                if (incident != null)
                {
                    Console.WriteLine($"Incident Id: {incident.IncidentId} should have a photograph: {supportingDocument.FileName}");
                }
            }

        }

        [Test]
        public void FindEmployeesWithEmploymentVerificationDocs()
        {
            var supportingDocs = SupportingDocument.Helper.Find(x => x.Module.Name == "Employment Verification")
                .ToList();

            foreach (var supportingDocument in supportingDocs)
            {
                var emp = Employee.Helper.FindById(supportingDocument.BaseDocumentId);

                Console.WriteLine($"{emp.PayrollId} has a {supportingDocument.DocumentType.Code} FileName: {supportingDocument.FileName}");
            }
        }

        [Test]
        public void ImportAll()
        {
            Stopwatch sw = new Stopwatch();
            var dt = DateTime.Now;

            Console.WriteLine("");
            sw.Start();
            ImportBenefitsDocs();
            Console.WriteLine($"Benefits = {FileAttachmentsHrs.CountDocumentsForModule("Benefits")}");
            sw.Stop();
            Console.WriteLine($"Elapsed time:  {sw.Elapsed}");

            Console.WriteLine("");
            sw.Start();
            ImportCompensationDocs();
            Console.WriteLine($"Compensation = {FileAttachmentsHrs.CountDocumentsForModule("Compensation")}");
            sw.Stop();
            Console.WriteLine($"Elapsed time:  {sw.Elapsed}");

            Console.WriteLine("");
            sw.Start();
            ImportDisciplineDocs();
            Console.WriteLine($"Discipline = {FileAttachmentsHrs.CountDocumentsForModule("Discipline")}");
            sw.Stop();
            Console.WriteLine($"Elapsed time:  {sw.Elapsed}");

            Console.WriteLine("");
            sw.Start();
            ImportEmployeeDetailsDocs();
            Console.WriteLine($"Employee Details = {FileAttachmentsHrs.CountDocumentsForModule("Employee Details")}");
            sw.Stop();
            Console.WriteLine($"Elapsed time:  {sw.Elapsed}");

            Console.WriteLine("");
            sw.Start();
            ImportPerformanceDocs();
            Console.WriteLine($"Performance = {FileAttachmentsHrs.CountDocumentsForModule("Performance")}");
            sw.Stop();
            Console.WriteLine($"Elapsed time:  {sw.Elapsed}");

            Console.WriteLine("");
            sw.Start();
            ImportTrainingDocs();
            Console.WriteLine($"Training = {FileAttachmentsHrs.CountDocumentsForModule("Training")}");
            sw.Stop();
            Console.WriteLine($"Elapsed time:  {sw.Elapsed}");

            Console.WriteLine("");
            sw.Start();
            ImportRequiredActivityDocs();
            Console.WriteLine($"Required Activities = {FileAttachmentsHrs.CountDocumentsForModule("Required Activities")}");
            sw.Stop();
            Console.WriteLine($"Elapsed time:  {sw.Elapsed}");


            Console.WriteLine("");
            Console.WriteLine($"Total time for all docs: {DateTime.Now - dt}");

        }

        [Test]
        public void AddNewEmployeeVerificationDocs()
        {

            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Employment Verification";
                var myHrsUser = GetAdminUser();
                var counter = 0;
                foreach (var doc in context.EmploymentVerification.Where(x => x.GCRecord == null).Include("EmploymentVerificationDocumentType").Include("MyFileData").AsNoTracking())
                {
                    counter++;
                    if (doc.Employee1 == null) continue;

                    var employeeId = doc.Employee;
                    var empIncident = Employee.Helper.Find(x => x.OldHrsId == employeeId).FirstOrDefault();
                    if (empIncident == null) continue;



                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;
                    
                    var comments = $"[Employment Verification] Satisfied: {doc.IsSatisfied} Issued: {doc.IssueDate} Expires: {doc.Expiration}";
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.IssueDate ?? DateTime.Now;
                    var documentType = doc.EmploymentVerificationDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, empIncident,
                            moduleName, documentType, userId, comments, empIncident.OldHrsId, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Incident: {empIncident.OldHrsId}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }



        [Test]
        public void AddNewEmployeeIncidentDocs()
        {

            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Incident Reporting Screen";
                var myHrsUser = GetAdminUser();
                var counter = 0;
                foreach (var doc in context.IncidentSupportingDocument.Where(x => x.GCRecord == null).Include("IncidentSupportingDocumentType").Include("MyFileData").AsNoTracking())
                {
                    counter++;
                    if (doc.Incident1 == null) continue;

                    //if (OnlyLoadIds.Count > 0)
                    //{
                    //    if (OnlyLoadIds.All(x => x != doc.Employee))
                    //    {
                    //        continue;
                    //    }
                    //}

                    var incidentId = doc.Incident;
                    var empIncident = EmployeeIncident.Helper.Find(x => x.OldHrsId == incidentId).FirstOrDefault();
                    if (empIncident == null) continue;



                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.IncidentSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("EmployeeIncidentDocumentType",
                        "Type of Employee Incident Document",
                        documentType, "Document Type");


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, empIncident,
                            moduleName, documentType, userId, comments, empIncident.OldHrsId, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Incident: {empIncident.OldHrsId}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

        [Test]
        public void LoadRequiredActivitiesAndTrainingDocuments()
        {
            Stopwatch sw = new Stopwatch();
            //var dt = DateTime.Now;

            //Console.WriteLine("");
            //sw.Start();
            //ImportTrainingDocs();
            //Console.WriteLine($"Training Events = {FileAttachmentsHrs.CountDocumentsForModule("Training Events Grid")}");
            //sw.Stop();
            //Console.WriteLine($"Elapsed time:  {sw.Elapsed}");

            Console.WriteLine("");
            sw.Start();
            ImportRequiredActivityDocs();
            Console.WriteLine($"Required Activities = {FileAttachmentsHrs.CountDocumentsForModule("Required Activities")}");
            sw.Stop();
            Console.WriteLine($"Elapsed time:  {sw.Elapsed}");


            //Console.WriteLine("");
            //Console.WriteLine($"Total time for all docs: {DateTime.Now - dt}");

        }


        [Test]
        public void ImportRequiredActivityDocs()
        {
            //var count = FileAttachmentsHrs.RemoveAllDocumentsForModule("Required Activities");
            //Console.WriteLine($"Removed {count} Training documents");

            var fromDate = DateTime.Parse("1/1/2014");

            var repTrainingEvent = new RepositoryBase<TrainingEvent>();
            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 120;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Required Activities";
                var myHrsUser = new RepositoryBase<HrsUser>().AsQueryable().ToList()
                    .Single(x => x.User.LastName == "Walker" && x.User.FirstName == "Denise");
                var rep = new RepositoryBase<RequiredActivity>();

                var docs = context.RequiredActivitySupportingDocument.Include("MyFileData").Where(x=>x.GCRecord == null && x.DocumentDate >= fromDate).AsNoTracking().ToList();
                // var docs = context.RequiredActivitySupportingDocument.Include("MyFileData").Where(x=>x.DocumentDate >= DateTime.Parse("1/1/2024")).AsNoTracking().ToList();
                //var dateTime = DateTime.Parse("1/1/2014");

                foreach (var doc in docs)
                {

                    if (doc.RequiredActivity1 == null) continue;

                    var sqlEmployee = doc.RequiredActivity1.Employee1;

                    if (sqlEmployee == null) continue;

                    var filter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId == sqlEmployee.OID);
                    var project = Employee.Helper.ProjectionBuilder.Expression(x => x.Id);

                    var employeeId = Employee.Helper.FindWithProjection(filter, project).FirstOrDefault();

                    if (employeeId == null) continue;


                    byte[] fileBytes;
                    if (doc.MyFileData == null) continue;
                    if (doc.RequiredActivitySupportingDocumentType == null) continue;


                    try
                    {
                        fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        continue;
                    }

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    //var employeeId = doc.RequiredActivity1.Employee;
                    //var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    //if (emp == null) continue;


                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.RequiredActivitySupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    //var newProperty = PropertyBuilder.CreatePropertyValue("RequiredActivityDocumentType",
                    //    "Required Activity Document Type",
                    //    documentType, "");

                    var newProperty = PropertyBuilder.CreatePropertyValue("Required ActivitiesDocumentType",
                       "Required Activities Document Type",
                       documentType, "");

                   

                    try
                    {

                            FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate,
                                employeeId, moduleName, documentType, userId, comments, sqlEmployee.OID, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} RequiredActivity: {doc.RequiredActivity}");
                        Console.WriteLine(ex.Message);
                    }
                }
                UpdateAllRequiredActivityHasSupportingDocuments();
            }
        }

        private static void UpdateAllRequiredActivityHasSupportingDocuments()
        {
            var repRequiredActivity = new RepositoryBase<RequiredActivity>();
            var repSupportingDocs = new RepositoryBase<SupportingDocument>();
            foreach (var requiredActivity in repRequiredActivity.AsQueryable().ToList())
            {
                requiredActivity.UpdateHasSupportingDocs(repRequiredActivity, repSupportingDocs);
            }
        }

        [Test]
        public void ImportTrainingDocs()
        {
            // TrainingSupportingDocument -> Supporting Document for Attendees of TrainingEvent or for the Event without an Attendee
            // TrainingEventSupportingDocument -> Sign-up Sheet for Training Event

            //var count = FileAttachmentsHrs.RemoveAllDocumentsForModule("Training");
            //Console.WriteLine($"Removed {count} Training documents");

            var fromDate = DateTime.Parse("1/1/2014");

            var repTrainingEvent = new RepositoryBase<TrainingEvent>();
            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 120;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = string.Empty;
                var myHrsUser = new RepositoryBase<HrsUser>().AsQueryable().ToList()
                    .Single(x => x.User.LastName == "Walker" && x.User.FirstName == "Denise");


                ImportTrainingDocumentsForEvents();
                ImportTrainingDocumentsForAttendee();

                void ImportTrainingDocumentsForEvents()
                {
                    foreach (var doc in context.TrainingEventSupportingDocument.Include("MyFileData").Where(x=>x.DocumentDate >= fromDate).AsNoTracking())
                    {


                        byte[] fileBytes;
                        if (doc.MyFileData == null) continue;
                        if (doc.TrainingEventSupportingDocumentType == null) continue;

                        try
                        {
                            fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }

                        if (fileBytes == null || fileBytes.Length == 0) continue;

                        var comments = doc.Comments ?? string.Empty;
                        var fileName = doc.MyFileData.FileName;
                        var documentDate = doc.DocumentDate ?? DateTime.Now;
                        var documentType = doc.TrainingEventSupportingDocumentType.Name.Replace("/", "-");
                        var userId = myHrsUser.UserId;


                        var newProperty = PropertyBuilder.CreatePropertyValue("TrainingEventDocumentType",
                            "Training Event Document Type",
                            documentType, "Document Type");

                        try
                        {
                            if (doc.TrainingEvent != null)
                            {
                                var trainingEventId = doc.TrainingEvent;
                                var trainingEvent = repTrainingEvent.AsQueryable().SingleOrDefault(x => x.OldHrsId == trainingEventId);
                                if (trainingEvent == null) continue;

                                moduleName = "Training Events Grid";

                                FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, trainingEvent,
                                    moduleName, documentType, userId, comments, trainingEventId ?? 0, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} TrainingEventId: {doc.TrainingEvent}");
                            Console.WriteLine(ex.Message);
                        }
                    }

                }

                void ImportTrainingDocumentsForAttendee()
                {
                    foreach (var doc in context.TrainingSupportingDocument.Include("MyFileData").Where(x=>x.TrainingAttendee1 != null && x.DocumentDate >= fromDate).AsNoTracking())
                    {

                        byte[] fileBytes;
                        try
                        {
                            fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }
                        

                        if (fileBytes == null || fileBytes.Length == 0) continue;

                        var comments = doc.Comments ?? string.Empty;
                        var fileName = doc.MyFileData.FileName;
                        var documentDate = doc.DocumentDate ?? DateTime.Now;
                        var documentType = doc.TrainingSupportingDocumentType.Name.Replace("/", "-");
                        var userId = myHrsUser.UserId;

                        var newProperty = PropertyBuilder.CreatePropertyValue("TrainingAttendeeDocumentType",
                            "Training Attendee Document Type",
                            documentType, "Document Type");

                        try
                        {
                            if (doc.TrainingAttendee1 != null)
                            {
                                var employeeId = doc.TrainingAttendee1.Employee;
                                var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                                if (emp == null) continue;

                                moduleName = "Training Events Grid";

                                FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                                    moduleName, documentType, userId, comments, employeeId ?? 0, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {doc.TrainingAttendee1?.Employee1.FirstName} {doc.TrainingAttendee1?.Employee1.LastName}");
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        private HrsUser GetAdminUser()
        {
            var filter =
                HrsUser.Helper.FilterBuilder.Where(x => x.User.LastName == "Walker" && x.User.FirstName == "Denise");
            return HrsUser.Helper.Find(filter).FirstOrDefault();
        }

        [Test]
        public void ImportBenefitsDocs()
        {
            var count = FileAttachmentsHrs.RemoveAllDocumentsForModule("Benefits");
            Console.WriteLine($"Removed {count} Benefits documents");

            using (HrsContext context = new HrsContext())
            {
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Benefits";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.BenefitSupportingDocument.Include("Benefits1").Include("MyFileData"))
                {

                    if (doc.Benefits1 == null) continue;

                    if (OnlyLoadIds.Count > 0)
                    {
                        if (OnlyLoadIds.All(x => x != doc.Benefits1.Employee))
                        {
                            continue;
                        }
                    }

                    var employeeId = doc.Benefits1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.BenefitSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("BenefitsDocumentType",
                        "Type of Benefits Document",
                        documentType, "Document Type");

                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments);
                        //Console.WriteLine(
                        //    $"Success! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }

        [Test]
        public void AddNewBenefitsDocs()
        {

            using (HrsContext context = new HrsContext())
            {
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Benefits";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.BenefitSupportingDocument.Where(x=>x.GCRecord == null).Include("Benefits1").Include("MyFileData"))
                {

                    if (doc.Benefits1 == null) continue;

                    //if (OnlyLoadIds.Count > 0)
                    //{
                    //    if (OnlyLoadIds.All(x => x != doc.Benefits1.Employee))
                    //    {
                    //        continue;
                    //    }
                    //}

                    var employeeId = doc.Benefits1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.BenefitSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("BenefitsDocumentType",
                        "Type of Benefits Document",
                        documentType, "Document Type");

                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments, 0, true);
                        //Console.WriteLine(
                        //    $"Success! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }


        [Test]
        public void ImportCompensationDocs()
        {
            var count = FileAttachmentsHrs.RemoveAllDocumentsForModule("Compensation");
            Console.WriteLine($"Removed {count} Compensation documents");

            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Compensation";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.CompensationSupportingDocument.Include("Compensation1").Include("MyFileData"))
                {

                    if (doc.Compensation1 == null) continue;

                    if (OnlyLoadIds.Count > 0)
                    {
                        if (OnlyLoadIds.All(x => x != doc.Compensation1.Employee))
                        {
                            continue;
                        }
                    }


                    var employeeId = doc.Compensation1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.CompensationSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("CompensationDocumentType",
                        "Type of Compensation Document",
                        documentType, "Document Type");

                    
                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments);
                        //Console.WriteLine(
                        //    $"Success! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

        [Test]
        public void AddNewCompensationDocs()
        {
            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Compensation";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.CompensationSupportingDocument.Where(x=>x.GCRecord == null).Include("Compensation1").Include("MyFileData"))
                {

                    if (doc.Compensation1 == null) continue;

                    //if (OnlyLoadIds.Count > 0)
                    //{
                    //    if (OnlyLoadIds.All(x => x != doc.Compensation1.Employee))
                    //    {
                    //        continue;
                    //    }
                    //}


                    var employeeId = doc.Compensation1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.CompensationSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("CompensationDocumentType",
                        "Type of Compensation Document",
                        documentType, "Document Type");


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments, 0, true);
                        //Console.WriteLine(
                        //    $"Success! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

        [Test]
        public void CheckForMissingCompensationDocs()
        {
            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Compensation";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.CompensationSupportingDocument.Where(x => x.GCRecord == null).Include("Compensation1").Include("MyFileData"))
                {

                    if (doc.Compensation1 == null) continue;

                    //if (OnlyLoadIds.Count > 0)
                    //{
                    //    if (OnlyLoadIds.All(x => x != doc.Compensation1.Employee))
                    //    {
                    //        continue;
                    //    }
                    //}


                    var employeeId = doc.Compensation1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.CompensationSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var systemModuleType = SystemModuleType.Helper.Find(x => x.Name == moduleName).Single().AsSystemModuleTypeRef();

                    var docFound = SupportingDocument.Helper.Find(x =>
                        x.BaseDocumentId == emp.Id &&
                        x.Module.Id == systemModuleType.Id &&
                        x.DocumentType.Code == documentType &&
                        x.FileName == fileName).FirstOrDefault();

                    if (docFound == null)
                    {
                        Console.WriteLine(
                            $"PayrollId: {emp.PayrollId} Missing FileName: {fileName} Document Date: {documentDate} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                    }

                }

            }
        }

        [Test]
        public void CheckForMissingCompensationHistoryDocs()
        {
            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Compensation";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.CompensationHistorySupportingDocument.Where(x => x.GCRecord == null).Include("CompensationHistory1.Compensation1.Employee1").Include("MyFileData"))
                {

                    if (doc.CompensationHistory1 == null) continue;

                    //if (OnlyLoadIds.Count > 0)
                    //{
                    //    if (OnlyLoadIds.All(x => x != doc.Compensation1.Employee))
                    //    {
                    //        continue;
                    //    }
                    //}

                    //doc.CompensationHistory1.Compensation1.Employee1

                    var employeeId = doc.CompensationHistory1.Compensation1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.CompensationSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("CompensationDocumentType",
                        "Type of Compensation Document",
                        documentType, "Document Type");


                    var systemModuleType = SystemModuleType.Helper.Find(x => x.Name == moduleName).Single().AsSystemModuleTypeRef();

                    var docFound = SupportingDocument.Helper.Find(x =>
                        x.BaseDocumentId == emp.Id &&
                        x.Module.Id == systemModuleType.Id &&
                        x.DocumentType.Code == documentType &&
                        x.FileName == fileName).FirstOrDefault();

                    if (docFound == null)
                    {
                        Console.WriteLine(
                            $"PayrollId: {emp.PayrollId} Missing FileName: {fileName} Document Date: {documentDate} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                    }

                }

            }
        }



        [Test]
        public void AddNewCompensationHistoryDocs()
        {
            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Compensation";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.CompensationHistorySupportingDocument.Where(x => x.GCRecord == null).Include("CompensationHistory1.Compensation1.Employee1").Include("MyFileData"))
                {

                    if (doc.CompensationHistory1 == null) continue;

                    //if (OnlyLoadIds.Count > 0)
                    //{
                    //    if (OnlyLoadIds.All(x => x != doc.Compensation1.Employee))
                    //    {
                    //        continue;
                    //    }
                    //}


                    var employeeId = doc.CompensationHistory1.Compensation1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.CompensationSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("CompensationDocumentType",
                        "Type of Compensation Document",
                        documentType, "Document Type");


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments, 0, true);
                        //Console.WriteLine(
                        //    $"Success! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }


        [Test]
        public void ImportPerformanceDocs()
        {
            var count = FileAttachmentsHrs.RemoveAllDocumentsForModule("Performance");
            Console.WriteLine($"Removed {count} Performance documents");

            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Performance";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.PerformanceSupportingDocument.Include("Performance1").Include("MyFileData"))
                {
                    
                    if (doc.Performance1 == null) continue;

                    if (OnlyLoadIds.Count > 0)
                    {
                        if (OnlyLoadIds.All(x => x != doc.Performance1.Employee))
                        {
                            continue;
                        }
                    }

                    var employeeId = doc.Performance1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.PerformanceSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("PerformanceDocumentType",
                        "Type of Performance Document",
                        documentType, "Document Type");


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

        [Test]
        public void ImportDisciplineDocs()
        {
            var count = FileAttachmentsHrs.RemoveAllDocumentsForModule("Discipline");
            Console.WriteLine($"Removed {count} Discipline documents");

            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Discipline";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.DisciplineSupportingDocument.Include("Discipline1").Include("MyFileData"))
                {

                    if (doc.Discipline1 == null) continue;

                    if (OnlyLoadIds.Count > 0)
                    {
                        if (OnlyLoadIds.All(x => x != doc.Discipline1.Employee))
                        {
                            continue;
                        }
                    }

                    var employeeId = doc.Discipline1.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.DisciplineSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("DisciplineDocumentType",
                        "Type of Discipline Document",
                        documentType, "Document Type");


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

        [Test]
        public void AddNewDisciplineDocs()
        {

            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Discipline";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.DisciplineSupportingDocument.Where(x=>x.GCRecord == null).Include("Discipline1").Include("MyFileData"))
                {


                    var employeeId = doc.Discipline1?.Employee ?? doc.DisciplineHistory1?.Discipline1?.Employee;
                    if (employeeId == null) continue;

                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.DisciplineSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    //var newProperty = PropertyBuilder.CreatePropertyValue("DisciplineDocumentType",
                    //    "Type of Discipline Document",
                    //    documentType, "Document Type");


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments, 0,true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }



        [Test]
        public void ImportEmployeeDetailsDocs()
        {
            var count = FileAttachmentsHrs.RemoveAllDocumentsForModule("Employee Details");
            Console.WriteLine($"Removed {count} Employee Details documents");

            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Employee Details";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.EmployeeSupportingDocument.Include("Employee1").Include("MyFileData").AsNoTracking())
                {

                    if (doc.Employee1 == null) continue;

                    if (OnlyLoadIds.Count > 0)
                    {
                        if (OnlyLoadIds.All(x => x != doc.Employee))
                        {
                            continue;
                        }
                    }

                    var employeeId = doc.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;

                    

                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.EmployeeSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("EmployeeDocumentType",
                        "Type of Employee Document",
                        documentType, "Document Type");


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

        [Test]
        public void AddNewEmployeeDetailsDocs()
        {

            using (HrsContext context = new HrsContext())
            {
                context.Database.CommandTimeout = 3000;
                var encryption = Encryption.NewEncryption;
                var mongoContext = new Vulcan.Mongo.Base.Context.HrsContext();
                var moduleName = "Employee Details";
                var myHrsUser = GetAdminUser();
                foreach (var doc in context.EmployeeSupportingDocument.Where(x=>x.GCRecord == null).Include("Employee1").Include("MyFileData").AsNoTracking())
                {

                    if (doc.Employee1 == null) continue;

                    //if (OnlyLoadIds.Count > 0)
                    //{
                    //    if (OnlyLoadIds.All(x => x != doc.Employee))
                    //    {
                    //        continue;
                    //    }
                    //}

                    var employeeId = doc.Employee;
                    var emp = _repEmployee.AsQueryable().SingleOrDefault(x => x.OldHrsId == employeeId);
                    if (emp == null) continue;



                    byte[] fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);

                    if (fileBytes == null || fileBytes.Length == 0) continue;

                    var comments = doc.Comments ?? string.Empty;
                    var fileName = doc.MyFileData.FileName;
                    var documentDate = doc.DocumentDate ?? DateTime.Now;
                    var documentType = doc.EmployeeSupportingDocumentType.Name.Replace("/", "-");
                    var userId = myHrsUser.UserId;

                    var newProperty = PropertyBuilder.CreatePropertyValue("EmployeeDocumentType",
                        "Type of Employee Document",
                        documentType, "Document Type");


                    try
                    {
                        FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate, emp,
                            moduleName, documentType, userId, comments,doc.EmployeeSupportingDocumentType.OID, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType} Employee: {emp.FirstName} {emp.LastName}");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

    }
}

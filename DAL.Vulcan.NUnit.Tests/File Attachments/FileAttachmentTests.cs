using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Quotes.Mongo.DocClass.FileAttachment;
using DAL.Vulcan.Mongo.Base.FileAttachment;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.File_Attachments
{
    [TestFixture()]
    public class FileAttachmentTests
    {
        private IHelperUser _helperUser;
        private IHelperLocation _helperLocation;
        private IHelperTeam _helperTeam;
        private IHelperCompany _helperCompany;
        private IHelperGoal _helperGoal;
        private IHelperAction _helperAction;
        private IHelperUserViewConfig _helperUserViewConfig;
        private IHelperApplication _helperApplication;
        private IHelperNotifications _helperNotifications;
        private IHelperPerson _helperPerson;
        private IHelperContact _helperContact;
        private IHelperReminder _helperReminder;
        private const string AppName = "vulcancrm";

        [SetUp]
        public void SetUp()
        {
            _helperLocation = new HelperLocation();
            _helperApplication = new HelperApplication();
            _helperPerson = new HelperPerson();
            _helperContact = new HelperContact(_helperPerson, _helperUser);
            _helperUser = new HelperUser(_helperPerson);
            _helperTeam = new HelperTeam(_helperUser);
            _helperCompany = new HelperCompany();
            _helperReminder = new HelperReminder();
            _helperNotifications = new HelperNotifications(_helperUser);
            _helperAction = new HelperAction(_helperUser,_helperNotifications,_helperApplication,_helperReminder);
            _helperGoal = new HelperGoal(_helperUser, _helperNotifications, _helperAction);
            _helperUserViewConfig = new HelperUserViewConfig(_helperUser, _helperTeam);

        }

        //[Test]
        //public void UploadDocument()
        //{
        //    var user = _helperUser.LookupUserByNetworkId("mpike");
        //    var fullFilePath = @"E:\Code\Vulcan\DAL.Vulcan.NUnit.Tests\File Attachments\Test Images\IMG_20170111_093937349.jpg";

        //    var fileName = 

        //    var newAttachmentId = FileAttachmentsVulcan.SaveFileAttachment(user, FileAttachmentType.UserPhoto, fullFilePath, user.Id.ToString());
        //    Assert.IsNotNull(newAttachmentId);
        //}

        //[Test]
        //public void GetAllAttachments()
        //{
        //    var result = FileAttachmentsVulcan.GetAllAttachments();
        //    foreach (var fsFileInfo in result)
        //    {
        //        Trace.WriteLine(fsFileInfo);
        //    }

        //}

        //[Test]
        //public void GetAllAttachmentsForDocument()
        //{
        //    var rep = new RepositoryBase<ContractPriceOrder>();
        //    var order =
        //        rep.AsQueryable().First(x => x.Coid == "INC" && x.Status == ContractOrderStatus.Submitted);
        //    var result = FileAttachmentsVulcan.GetAllAttachmentsForDocument(order);
        //    foreach (var fsFileInfo in result)
        //    {
        //        Trace.WriteLine(fsFileInfo);
        //    }

        //}

        //[Test]
        //public void DownloadAsBytes()
        //{
        //    var rep = new RepositoryBase<ContractPriceOrder>();
        //    var order =
        //        rep.AsQueryable().First(x => x.Coid == "INC" && x.Status == ContractOrderStatus.Submitted);
        //    var result = FileAttachmentsVulcan.GetAllAttachmentsForDocument(order);
        //    foreach (var fsFileInfo in result)
        //    {
        //        Trace.WriteLine(FileAttachmentsVulcan.DownloadAsBytes(fsFileInfo).Length);
        //    }

        //}

        //[Test]
        //public void DownloadToStream()
        //{
        //    var rep = new RepositoryBase<ContractPriceOrder>();
        //    var order =
        //        rep.AsQueryable().First(x => x.Coid == "INC" && x.Status == ContractOrderStatus.Submitted);
        //    var result = FileAttachmentsVulcan.GetAllAttachmentsForDocument(order);
        //    foreach (var fsFileInfo in result)
        //    {
        //        var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        var fileName = folder + @"\" + Path.GetFileName(fsFileInfo.Filename);
        //        FileStream fileStream = new FileStream(fileName, FileMode.CreateNew);
        //        FileAttachmentsVulcan.DownloadToFileStream(fsFileInfo, fileStream);
        //        fileStream.Flush(true);
        //    }

        //}

        //[Test]
        //public void DownloadToFile()
        //{
        //    var rep = new RepositoryBase<ContractPriceOrder>();
        //    var order =
        //        rep.AsQueryable().First(x => x.Coid == "MSA" && x.Status == ContractOrderStatus.Submitted);
        //    var result = FileAttachmentsVulcan.GetAllAttachmentsForDocument(order);
        //    foreach (var fsFileInfo in result)
        //    {
        //        var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        var fileName = folder + @"\" + Path.GetFileName(fsFileInfo.Filename);
        //        FileAttachmentsVulcan.DownloadToFile(fsFileInfo, fileName);
        //    }

        //}

        //[Test]
        //public void CheckFileOut()
        //{
        //    try
        //    {
        //        OpenXmlValidator validator = new OpenXmlValidator();
        //        var fileName = Path.Combine("C:\\Users\\mpike\\Downloads", "FMC Orders as of 4-5-2017 12-40 PM.xlsx");
        //        foreach (ValidationErrorInfo error in validator.Validate(SpreadsheetDocument.Open(fileName, true)))
        //        {
        //            Trace.WriteLine(new String('-', 50));
        //            Trace.WriteLine(error.Path.XPath);
        //            Trace.WriteLine(error.Part);
        //            Trace.WriteLine(error.Description);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }

        //}

    }

}

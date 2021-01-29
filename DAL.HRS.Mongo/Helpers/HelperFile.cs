using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperFile : HelperBase, IHelperFile
    {
        public Dictionary<string,string> MimiTypes = new Dictionary<string, string>()
        {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"}
        };

        public HelperFile()
        {
        }

        public string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
            if (ext == null) throw new Exception("FileType is not supported");
            return MimiTypes[ext];
        }

        public FileAttachmentModel UploadDocument(HrsContext context, byte[] fileBytes, string fileName, DateTime documentDate, BaseDocument attachToDocument, string module, string documentType, string userId, string comments)
        {
            var saveFile = FileAttachmentsHrs.SaveFileAttachment(context, fileBytes, fileName, documentDate, attachToDocument, module, documentType, userId, comments);

            return new FileAttachmentModel(saveFile.supportingDocument);
        }

        public FileAttachmentModel UploadDocument(HrsContext context, byte[] fileBytes, string fileName, DateTime documentDate,
            ObjectId baseDocumentId, string module, string documentType, string userId, string comments)
        {
            var saveFile = FileAttachmentsHrs.SaveFileAttachment(context, fileBytes, fileName, documentDate, baseDocumentId, module, documentType, userId, comments);

            return new FileAttachmentModel(saveFile.supportingDocument);
        }


        public List<FileAttachmentModel> GetAllAttachmentsForDocument(BaseDocument document, string module)
        {
            var result = new List<FileAttachmentModel>();
            var supportingDocuments = new RepositoryBase<SupportingDocument>().AsQueryable()
                .Where(x => x.BaseDocumentId == document.Id && x.Module.Name == module).ToList();

            foreach (var supportingDocument in supportingDocuments)
            {
                result.Add(new FileAttachmentModel(supportingDocument));
            }

            return result;
        }

        public List<FileAttachmentModel> GetAllAttachmentsForDocument(string objectId, string module)
        {
           
            var result = new List<FileAttachmentModel>();
            var id = ObjectId.Parse(objectId);
            var supportingDocuments = new RepositoryBase<SupportingDocument>().AsQueryable()
                .Where(x => x.BaseDocumentId == id && x.Module.Name == module).ToList();

            foreach (var supportingDocument in supportingDocuments)
            {
                result.Add(new FileAttachmentModel(supportingDocument));
            }

            return result;
        }

        public Task ExportEmployeeAvatars(string path)
        {
            return Task.Run(() =>
            {
                var queryHelper = new MongoRawQueryHelper<SupportingDocument>();





            });
        }

        public static void DownloadToFileStream(GridFSFileInfo fileInfo, FileStream fileStream)
        {
            var context = new HrsContext();
            context.FileAttachmentBucket.DownloadToStream(fileInfo.Id, fileStream);
        }

    }
}
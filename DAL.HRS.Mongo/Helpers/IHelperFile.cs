using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperFile
    {
        string GetContentType(string fileName);

        FileAttachmentModel UploadDocument(HrsContext context, byte[] fileBytes, string fileName, DateTime documentDate,
            BaseDocument attachToDocument, string module, string documentType, string userId, string comments);

        FileAttachmentModel UploadDocument(HrsContext context, byte[] fileBytes, string fileName, DateTime documentDate,
            ObjectId baseDocumentId, string module, string documentType, string userId, string comments);

        List<FileAttachmentModel> GetAllAttachmentsForDocument(BaseDocument document, string module);
        List<FileAttachmentModel> GetAllAttachmentsForDocument(string objectId, string module);
        Task ExportEmployeeAvatars(string path);
    }
}
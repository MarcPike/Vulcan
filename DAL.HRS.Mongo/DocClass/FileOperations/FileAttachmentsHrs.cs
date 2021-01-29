using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.Quotes.Mongo.DocClass.FileAttachment;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.FileAttachment;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace DAL.HRS.Mongo.DocClass.FileOperations
{
    public static class FileAttachmentsHrs
    {
        private static readonly HelperUser _helperUser = new HelperUser();
        //public static ObjectId SaveFileAttachment(byte[] fileBytes, string fileName, BaseDocument attachToDocument, string role, string module, string contentType, string userId)
        //{
        //    var options = new GridFSUploadOptions()
        //    {
        //        Metadata = new BsonDocument
        //        {
        //            {"attachToDocumentId", attachToDocument.Id},
        //            {"role", role },
        //            {"module", module },
        //            {"mimeType", Mime.GetMimeType(fileName) },
        //            {"fileName", Path.GetFileName(fileName) },
        //            {"userId", userId },
        //            {"uploadDate", DateTime.UtcNow }
        //        }
        //    };
        //    var context = new HrsContext();

        //    return context.FileAttachmentBucket.UploadFromBytes(fileName, fileBytes, options);
        //}

        public static (GridFSFileInfo FileInfo, SupportingDocument supportingDocument) SaveFileAttachment(HrsContext context, byte[] fileBytes, string fileName, DateTime documentDate, BaseDocument attachToDocument, string module, string documentType, string userId, string comments, int oldHrsId=0, bool onlyNew=false)
        {
            return SaveFileAttachment(context, fileBytes, fileName, documentDate, attachToDocument.Id, module,
                documentType, userId, comments, oldHrsId, onlyNew);

        }

        public static (GridFSFileInfo FileInfo, SupportingDocument supportingDocument) SaveFileAttachment(HrsContext context, byte[] fileBytes, string fileName, DateTime documentDate, ObjectId baseDocumentId, string module, string documentType, string userId, string comments, int oldHrsId = 0, bool onlyNew=false)
        {
           
            if (module == "Required Activities") module = "Training";
            var systemModuleTypeRef = SystemModuleType.Helper.Find(x=>x.Name == module).FirstOrDefault()?.AsSystemModuleTypeRef();
            if (systemModuleTypeRef == null) throw new Exception("Unknown module");

            if (onlyNew)
            {
                var existingDoc = SupportingDocument.Helper.Find(x => x.FileName == fileName && x.Module.Name == module && x.BaseDocumentId == baseDocumentId).FirstOrDefault();
                if (existingDoc != null)
                {
                    return (existingDoc.FileInfo, existingDoc);
                }
            }

            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument
                {
                    {"attachToDocumentId", baseDocumentId},
                    {"module", module },
                    {"mimeType", Mime.GetMimeType(fileName) },
                    {"fileName", Path.GetFileName(fileName) },
                    {"fileSize", fileBytes.Length },
                    {"documentDate", documentDate },
                    {"documentType", documentType},
                    {"userId", userId },
                    {"uploadDate", DateTime.UtcNow },
                    {"comments", comments }
                }
            };

            var gridFsId = context.FileAttachmentBucket.UploadFromBytes(fileName, fileBytes, options);
            var docInfo = GetDocumentInfoForId(context, gridFsId);
            var hrsUser = _helperUser.GetHrsUser(userId);

            var supportingDoc = new SupportingDocument()
            {
                BaseDocumentId = baseDocumentId,
                Comments = comments,
                CreateDateTime = DateTime.UtcNow,
                CreatedByUserId = userId,
                DocumentDate = documentDate,
                DocumentType = PropertyBuilder.CreatePropertyValue(module + "DocumentType", module + " Document Type", documentType, "").AsPropertyValueRef(),
                FileCreateDate = DateTime.UtcNow,
                FileInfo = docInfo,
                MimeType = Mime.GetMimeType(fileName),
                FileName = fileName,
                FileSize = fileBytes.Length,
                Module = systemModuleTypeRef,
                UploadedByUser = hrsUser.AsHrsUserRef(),
                OldHrsId = oldHrsId
            };
            supportingDoc.SaveToDatabase();
            return (docInfo, supportingDoc);
        }


        public static List<GridFSFileInfo> GetAllAttachments()
        {
            var context = new HrsContext();
            return context.Database.GetCollection<GridFSFileInfo>("fileAttachments.files").AsQueryable().ToList();
        }

        public static List<GridFSFileInfo> GetAllAttachmentsForDocument(BaseDocument document, string module)
        {

            return GetAllAttachmentsForDocument(document.Id, module);
        }

        public static List<GridFSFileInfo> GetAllAttachmentsForDocument(ObjectId documentId, string module)
        {
            var filesFound = new List<GridFSFileInfo>();
            var supportingDocuments = new RepositoryBase<SupportingDocument>().AsQueryable().
                Where(x => x.BaseDocumentId == documentId && x.Module.Name == module).ToList();

            if (supportingDocuments == null) throw new Exception("File not found");

            var context = new Vulcan.Mongo.Base.Context.HrsContext();

            foreach (var doc in supportingDocuments)
            {
                var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", doc.FileInfo.Id);

                var fileFound = context.FileAttachmentBucket.Find(filter).FirstOrDefault();
                if (fileFound != null)
                {
                    filesFound.Add(fileFound);
                }
            }

            return filesFound;

        }


        public static int RemoveAllDocumentsForModule(string moduleName)
        {
            var rep = new RepositoryBase<SupportingDocument>();
            var supportingDocs = rep.AsQueryable().Where(x=>x.Module.Name == moduleName).ToList();
            var count = supportingDocs.Count;
            foreach (var supportingDocument in supportingDocs.ToList())
            {
                Remove(supportingDocument.FileInfo.Id);
                rep.RemoveOne(supportingDocument);
            }

            return count;
        }

        public static int CountDocumentsForModule(string moduleName)
        {

            return new RepositoryBase<SupportingDocument>().AsQueryable().Count(x=>x.Module.Name == moduleName);
        }

        public static GridFSFileInfo GetDocumentInfoForId(HrsContext context, ObjectId id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", id);
            return context.FileAttachmentBucket.Find(filter).SingleOrDefault();
        }

        public static byte[] DownloadAsBytes(GridFSFileInfo fileInfo)
        {
            var context = new HrsContext();
            return context.FileAttachmentBucket.DownloadAsBytes(fileInfo.Id);
        }

        public static void DownloadToFileStream(GridFSFileInfo fileInfo, FileStream fileStream)
        {
            var context = new HrsContext();
            context.FileAttachmentBucket.DownloadToStream(fileInfo.Id, fileStream);
        }

        public static void DownloadToFile(GridFSFileInfo fileInfo, string newFileName)
        {
            //File.Delete(newFileName);
            using (FileStream fileStream = new FileStream(newFileName, FileMode.CreateNew))
            {
                FileAttachmentsVulcan.DownloadToFileStream(fileInfo, fileStream);
                fileStream.Flush(true);
                fileStream.Close();
            }

        }

        public static Stream DownloadAsStream(GridFSFileInfo poFile)
        {
            var context = new HrsContext();
            var stream = new MemoryStream();
            context.FileAttachmentBucket.DownloadToStream(poFile.Id, stream);



            return stream;
        }

        public static void Remove(ObjectId id)
        {
            var context = new HrsContext();
            var rep = context.Database.GetCollection<GridFSFileInfo>("fileAttachments.files");
            {
                var repSupportingDocs = new RepositoryBase<SupportingDocument>();
                var queryHelper = new MongoRawQueryHelper<SupportingDocument>();
                var filterSupportingDoc =  queryHelper.FilterBuilder.Eq("FileInfo._id", id);
                var supportingDocs = queryHelper.Find(filterSupportingDoc);
                foreach (var supportingDocument in supportingDocs)
                {
                    repSupportingDocs.RemoveOne(supportingDocument);
                }


                var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", id);
                //var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Id, id);

                var gridFile = rep.Find(filter);
                if (gridFile != null)
                {
                    context.Database.GetCollection<GridFSFileInfo>("fileAttachments.files")
                        .DeleteOne(filter, System.Threading.CancellationToken.None);

                }

            }
        }
    }
}
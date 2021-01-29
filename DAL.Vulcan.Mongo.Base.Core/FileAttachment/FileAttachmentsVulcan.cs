using DAL.Quotes.Mongo.Core.DocClass.FileAttachment;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DAL.Vulcan.Mongo.Base.Core.FileAttachment
{
    public static class FileAttachmentsVulcan
    {
        //public class OsirisDocInfo
        //{
        //    public long ID { get; set; }
        //    public string COID { get; set; }
        //    public int TypeID { get; set; }
        //    public string TypeName { get; set; }
        //    public int FormatID { get; set; }
        //    public string FormatName { get; set; }
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public long DKL_DocID { get; set; }
        //    public string FileName => $"{Name}.{FormatName}";
        //}

        public static ObjectId SaveFileAttachment(byte[] fileBytes, string fileName, BaseDocument attachToDocument, FileAttachmentType type, string contentType, string userId)
        {
            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument
                {
                    {"attachToDocumentId", attachToDocument.Id},
                    {"fileAttachmentType", type },
                    {"mimeType", Mime.GetMimeType(fileName) },
                    {"fileName", Path.GetFileName(fileName) },
                    {"userId", userId },
                    {"uploadDate", DateTime.UtcNow }
                }
            };
            var context = new VulcanContext();

            return context.FileAttachmentBucket.UploadFromBytes(fileName, fileBytes, options);
        }

        public static ObjectId SaveFileAttachment(byte[] fileBytes, string fileName, BaseDocument attachToDocument, FileAttachmentType type, string userId)
        {
            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument
                {
                    {"attachToDocumentId", attachToDocument.Id},
                    {"fileAttachmentType", type },
                    {"mimeType", Mime.GetMimeType(fileName) },
                    {"fileName", Path.GetFileName(fileName) },
                    {"userId", userId },
                    {"uploadDate", DateTime.UtcNow }
                }
            };
            var context = new VulcanContext();

            return context.FileAttachmentBucket.UploadFromBytes(fileName, fileBytes, options);
        }

        public static ObjectId SaveFileAttachmentForOsiris(byte[] fileBytes, string fileName, BaseDocument attachToDocument, FileAttachmentType type, string userId, string typeName, string description)
        {
            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument
                {
                    {"attachToDocumentId", attachToDocument.Id},
                    {"fileAttachmentType", type },
                    {"mimeType", Mime.GetMimeType(fileName) },
                    {"fileName", Path.GetFileName(fileName) },
                    {"userId", userId },
                    {"uploadDate", DateTime.UtcNow },
                    {"osirisDocType", typeName },
                    {"osirisDocDescription", description }
                }
            };
            var context = new VulcanContext();

            return context.FileAttachmentBucket.UploadFromBytes(fileName, fileBytes, options);
        }


        public static ObjectId SaveFileAttachment(string serverFullFileName, string clientFullFileName, BaseDocument attachToDocument, FileAttachmentType type, string userId)
        {
            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument
                {
                    {"attachToDocumentId", attachToDocument.Id},
                    {"fileAttachmentType", type },
                    {"contentType", Mime.GetMimeType(serverFullFileName) },
                    {"serverFullFileName", serverFullFileName },
                    {"clientFullFileName", clientFullFileName },
                    {"fileName", Path.GetFileName(clientFullFileName) },
                    {"userId", userId },
                    {"uploadDate", DateTime.UtcNow }
                }
            };
            var context = new VulcanContext();

            using (var fileStream = new FileStream(serverFullFileName, FileMode.Open, FileAccess.Read))
            {
                return context.FileAttachmentBucket.UploadFromStream(serverFullFileName, fileStream, options);
            }

        }

        public static ObjectId SaveFileAttachment(BaseDocument document, FileAttachmentType fileAttachmentType, string fullFilePath, string userId)
        {
            var context = new VulcanContext();

            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument
                {
                    {"attachToDocumentId", document.Id},
                    {"fileAttachmentType", fileAttachmentType },
                    {"contentType", Mime.GetMimeType(fullFilePath) },
                    {"fileName", Path.GetFileName(fullFilePath) },
                    {"userId", userId },
                    {"uploadDate", DateTime.UtcNow }
                }
            };
            using (var fileStream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
            {
                return context.FileAttachmentBucket.UploadFromStream(fullFilePath, fileStream, options);
            }

        }

        public static List<GridFSFileInfo> GetAllAttachments()
        {
            var context = new VulcanContext();
            return context.Database.GetCollection<GridFSFileInfo>("fileAttachments.files").AsQueryable().ToList();
        }

        public static List<GridFSFileInfo> GetAllAttachmentsForDocument(BaseDocument document)
        {
            var context = new VulcanContext();
            
            var filter = Builders<GridFSFileInfo>.Filter.Eq("metadata.attachToDocumentId", document.Id);

            return context.FileAttachmentBucket.Find(filter).ToList();
            
        }

        public static byte[] DownloadAsBytes(GridFSFileInfo fileInfo)
        {
            var context = new VulcanContext();
            return context.FileAttachmentBucket.DownloadAsBytes(fileInfo.Id);
        }

        public static void DownloadToFileStream(GridFSFileInfo fileInfo, FileStream fileStream)
        {
            var context = new VulcanContext();
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
            var context = new VulcanContext();
            var stream = new MemoryStream();
            context.FileAttachmentBucket.DownloadToStream(poFile.Id, stream);
            return stream;
        }

        public static void Remove(ObjectId id)
        {
            var context = new VulcanContext();
            var rep = context.Database.GetCollection<GridFSFileInfo>("fileAttachments.files");
            {
                var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", id);
                //var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Id, id);

                context.Database.GetCollection<GridFSFileInfo>("fileAttachments.files")
                    .DeleteOne(filter, System.Threading.CancellationToken.None);
            }
        }
    }
}

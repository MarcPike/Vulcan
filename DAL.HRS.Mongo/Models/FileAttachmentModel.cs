using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Quotes.Mongo.DocClass.FileAttachment;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GridFS;

namespace DAL.HRS.Mongo.Models
{
    public class FileAttachmentModel
    {
        public string Id { get; set; } = string.Empty;
        public string FileId { get; set; }
        public string FileName { get; set; } = string.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UploadDateTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DocumentDate { get; set; }

        public string BaseDocumentId { get; set; }

        public string MimeType { get; set; } = string.Empty;
        public SystemModuleTypeRef Module { get; set; }
        public long FileSize { get; set; }
        public PropertyValueRef DocumentType { get; set; }
        public string JsonData { get; set; }
        public string Comments { get; set; }
        public FileAttachmentModel(SupportingDocument supportingDocument)
        {

            Id = supportingDocument.Id.ToString();
            FileName = supportingDocument.FileName;
            UploadDateTime = supportingDocument.FileCreateDate;
            MimeType = supportingDocument.MimeType;
            FileSize = supportingDocument.FileSize;
            Module = supportingDocument.Module;
            DocumentDate = supportingDocument.DocumentDate;
            DocumentType = supportingDocument.DocumentType;
            JsonData = supportingDocument.FileInfo.Metadata.ToString();
            Comments = supportingDocument.Comments;
            FileId = supportingDocument.FileInfo.Id.ToString();
            BaseDocumentId = supportingDocument.BaseDocumentId.ToString();
        }

        public FileAttachmentModel()
        {

        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path)?.ToLowerInvariant();

            if (ext != null && types.ContainsKey(ext))
            {
                return types[ext];
            }

            return "unknown-mime";
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };

        }

    }
}

using System;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.SecurityRole;

namespace DAL.HRS.Mongo.Models
{
    public class SupportingDocumentModel
    {
        public string SupportingDocumentId { get; set; }
        public string BaseDocumentId { get; set; }
        public SystemModuleTypeRef Module { get; set; }
        public PropertyValueRef DocumentType { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string MimeType { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime FileCreateDate { get; set; }
        public HrsUserRef UploadedByUser { get; set; }
        public string Comments { get; set; }
        public string FileId { get; set; }

        public SupportingDocumentModel()
        {
        }

        public SupportingDocumentModel(string supportingDocumentId, string baseDocumentId, SystemModuleTypeRef module, PropertyValueRef documentType, 
            DateTime documentDate, string fileName, int fileSize, DateTime fileCreateDate, HrsUserRef uploadedByUser, string comments, string fileId, string mimeType)
        {
            SupportingDocumentId = supportingDocumentId;
            BaseDocumentId = baseDocumentId;
            Module = module;
            DocumentType = documentType;
            DocumentDate = documentDate;
            FileName = fileName;
            FileSize = fileSize;
            FileCreateDate = fileCreateDate;
            UploadedByUser = uploadedByUser;
            Comments = comments;
            FileId = fileId;
            MimeType = mimeType;
        }
    }
}
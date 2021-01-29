using System;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Driver.GridFS;

namespace DAL.HRS.Mongo.DocClass.FileOperations
{
    public class SupportingDocumentRef : ReferenceObject<SupportingDocument> 
    {
        public string BaseDocumentId { get; set; }
        public SystemModuleTypeRef Module { get; set; }
        public PropertyValueRef DocumentType { get; set; }
        public string Comments { get; set; }
        public string FileName { get; set; }
        public GridFSFileInfo FileInfo { get; set; }
        public DateTime FileCreateDate { get; set; }


        public SupportingDocumentRef(SupportingDocument d)
        {
            BaseDocumentId = d.BaseDocumentId.ToString();
            Module = d.Module;
            DocumentType = d.DocumentType;
            Comments = d.Comments;
            FileName = d.FileName;
            FileInfo = d.FileInfo;
            FileCreateDate = d.FileCreateDate;
        }

        public SupportingDocument AsSupportingDocument()
        {
            return ToBaseDocument();
        }

    }
}
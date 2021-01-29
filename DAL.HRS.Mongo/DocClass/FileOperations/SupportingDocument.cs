using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.DocClass.FileOperations
{
    public class SupportingDocument: BaseDocument
    {
        public ObjectId BaseDocumentId { get; set; }
        public Guid SecondaryId { get; set; } = Guid.Empty;
        public SystemModuleTypeRef Module { get; set; }
        public PropertyValueRef DocumentType { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string MimeType { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] // this should only be added to the db object, no effect in the model
        public DateTime DocumentDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime FileCreateDate { get; set; }
        
        public HrsUserRef UploadedByUser { get; set; }
        public string Comments { get; set; }
        public GridFSFileInfo FileInfo { get; set; }
        public int OldHrsId { get; set; } = 0;

        public string AdminNotes { get; set; } = string.Empty;
        
        public static MongoRawQueryHelper<SupportingDocument> Helper = new MongoRawQueryHelper<SupportingDocument>();

        public SupportingDocumentRef AsSupportingDocumentRef()
        {
            return new SupportingDocumentRef(this);
        }

        public SupportingDocument ChangeModule(SystemModuleType module)
        {
            Module = module.AsSystemModuleTypeRef();
            FileInfo.Metadata.Set("module", module.Name);
            Helper.Upsert(this);
            AdminNotes += $@"-Moved Module from [{Module.Name}] to [{module.Name}] {DateTime.Now.Date}\n";
            return this;
        }

        public SupportingDocument ChangeDocumentType(string documentType, string documentTypeDescription, bool removeOld)
        {


            var oldProperty = DocumentType.AsPropertyValue();
            var code = DocumentType.Code;
            var description = DocumentType.Description;

            DocumentType = PropertyBuilder.CreatePropertyValue(documentType, documentTypeDescription, code, description).AsPropertyValueRef();
            Helper.Upsert(this);

            if (removeOld)
            {
                if (oldProperty != null)
                {
                    var oldType = 
                        PropertyType.Helper.Find(x=> x.Type == oldProperty.Type).FirstOrDefault();

                    var removePropValRef = oldType.PropertyValues.FirstOrDefault(x => x.Id == oldProperty.Id);
                    if (removePropValRef != null)
                    {
                        oldType.PropertyValues.Remove(removePropValRef);
                        PropertyType.Helper.Upsert(oldType);
                    }

                    PropertyValue.Helper.DeleteOne(oldProperty.Id);
                }
            }

            return this;
        }
    }
}

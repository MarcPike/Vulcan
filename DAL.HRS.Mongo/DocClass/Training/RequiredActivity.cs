using System;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class RequiredActivity : BaseDocument
    {
        public static MongoRawQueryHelper<RequiredActivity> Helper = new MongoRawQueryHelper<RequiredActivity>();
        public EmployeeRef Employee { get; set; }
        public Guid ChildObjectId { get; set; } = Guid.Empty;

        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public RequiredActivityType Type { get; set; } = RequiredActivityType.Unknown;

        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public RequiredActivityStatus Status { get; set; } = RequiredActivityStatus.Unknown;

        public PropertyValueRef ActivityType { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }
        public RequiredActivityRef NextRequiredActivityForCertification { get; set; }

        public Guid MedicalExamId { get; set; } = Guid.Empty;
        public string Description { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? CompletionDeadline { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? RevisedCompletionDeadline { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? DateCompleted { get; set; }

        public PropertyValueRef CompleteStatus { get; set; }
        public string Comments { get; set; }
        public int OldHrsId { get; set; }
        public bool HasSupportingDocs { get; set; }
        public Guid VerificationDocumentId { get; set; } = Guid.Empty;

        public void UpdateHasSupportingDocs(
            RepositoryBase<RequiredActivity> repRequiredActivity,
            RepositoryBase<SupportingDocument> repSupportingDocs)
        {
            var objectId = ObjectId.Parse(Employee.Id);
            var hasDocs = repSupportingDocs.AsQueryable().Any(x =>
                x.DocumentType.Type == "Required Activities" && x.BaseDocumentId == objectId);
            if (hasDocs != HasSupportingDocs)
            {
                HasSupportingDocs = hasDocs;
                repRequiredActivity.Upsert(this);
            }
        }

        public RequiredActivityRef AsRequiredActivityRef()
        {
            return new RequiredActivityRef(this);
        }
    }
}
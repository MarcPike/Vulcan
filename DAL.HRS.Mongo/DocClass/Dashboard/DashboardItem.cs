using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.HRS.Mongo.DocClass.Dashboard
{
    public class DashboardItem : BaseDocument
    {
        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo        public DashboardItemType Type { get; set; }
        public DashboardItemType Type { get; set; }
        public EmployeeRef Employee { get; set; }
        public DateTime DueDate { get; set; }
        public bool Dismissed { get; set; } = false;
        public List<SupportingDocumentRef> SupportingDocuments { get; set; } = new List<SupportingDocumentRef>();
        public TrainingEventRef TrainingEvent { get; set; }
        public Guid MedicalExamId { get; set; }
        public string Notes { get; set; } = string.Empty;

        public DashboardItemRef AsDashboardItemRef()
        {
            return new DashboardItemRef(this);
        }

        public DashboardItem() { }

        public static DashboardItemRef SetDueDate(DashboardItemRef dashboardItem, DateTime newDueDate)
        {
            var queryHelper = new MongoRawQueryHelper<DashboardItem>();
            var id = ObjectId.Parse(dashboardItem.Id);
            var filter = queryHelper.FilterBuilder.Where(x => x.Id == id);
            var update = queryHelper.UpdateBuilder.Set(x => x.DueDate, newDueDate);
            queryHelper.UpdateOne(filter, update);

            return queryHelper.Find(filter).First().AsDashboardItemRef();
        }

        public static DashboardItemRef SetDismissed(DashboardItemRef dashboardItem, bool dismissed)
        {
            var queryHelper = new MongoRawQueryHelper<DashboardItem>();
            var id = ObjectId.Parse(dashboardItem.Id);
            var filter = queryHelper.FilterBuilder.Where(x => x.Id == id);
            var update = queryHelper.UpdateBuilder.Set(x => x.Dismissed, dismissed);

            return queryHelper.Find(filter).First().AsDashboardItemRef();
        }

    }
}
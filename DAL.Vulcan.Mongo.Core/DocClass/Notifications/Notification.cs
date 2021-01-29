using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;
using DAL.Vulcan.Mongo.Base.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class Notification: BaseDocument
    {
        public static MongoRawQueryHelper<Notification> Helper = new MongoRawQueryHelper<Notification>();

        [JsonConverter(typeof(JsonStringEnumConverter))]  // JSON.Net
        public NotificationActionType ActionType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]  // JSON.Net
        public NotificationObjectType PrimaryObjectType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]  // JSON.Net
        public NotificationObjectType SecondaryObjectType { get; set; }
        public string Label { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NotificationDate { get; set; } = DateTime.UtcNow;

        public StrategyRef Strategy { get; set; }
        public GoalRef Goal { get; set; }
        public ActionRef Action { get; set; }
        public TeamRef Team { get; set; }

        public CrmUserRef CrmUser { get; set; }

        public CompanyGroupRef CompanyGroup { get; set; }
        public CompanyRef Company { get; set; }
        public ProspectRef Prospect { get; set; }

        public ContactMeetingInviteRef ContactMeetingInvite { get; set; }

        public NotificationRef AsNotificationRef()
        {
            return new NotificationRef(this);
        }
    }
}

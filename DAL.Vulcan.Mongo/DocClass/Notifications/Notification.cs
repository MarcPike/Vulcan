using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class Notification: BaseDocument
    {
        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        public NotificationActionType ActionType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        public NotificationObjectType PrimaryObjectType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
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

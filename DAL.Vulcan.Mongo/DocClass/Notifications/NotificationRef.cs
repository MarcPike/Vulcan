using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    [BsonIgnoreExtraElements]
    public class NotificationRef : ReferenceObject<Notification>
    {
        public string Label { get; set; }

        public NotificationRef(Notification document) : base(document)
        {
            
        }

        public NotificationRef()
        {
            
        }

        public Notification AsNotification()
        {
            return ToBaseDocument();
        }
    }
}
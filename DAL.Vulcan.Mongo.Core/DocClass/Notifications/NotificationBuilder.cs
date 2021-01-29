using DAL.Vulcan.Mongo.Base.Core.Repository;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilder
    {
        protected static RepositoryBase<Notification> Repository = new RepositoryBase<Notification>();

        public virtual Notification GetNotification()
        {
            return new Notification();
        }
    }
}
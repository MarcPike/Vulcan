using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
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
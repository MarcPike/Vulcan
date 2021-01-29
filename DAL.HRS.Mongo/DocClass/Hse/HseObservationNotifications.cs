namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class HseObservationNotifications
    {
        public bool EmployeeNotified { get; set; }
        public bool ManagerNotified { get; set; }
        public bool GlobalNotificationRequired { get; set; }
        public bool GlobalNotificationCompleted { get; set; }
    }
}
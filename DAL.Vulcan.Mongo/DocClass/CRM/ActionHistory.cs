using System;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class ActionHistory
    {
        public UserRef User { get; set; }
        public bool IsStarted { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public int PercentComplete { get; set; } = 0;
        public DateTime ActionDate { get; set; } = new DateTime();

    }
}
using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class MeetingInvite : BaseDocument
    {
        public ActionRef Action { get; set; }
        public CrmUserRef CrmUser { get; set; }
        public ContactRef Contact { get; set; }
        public UserRef Employee { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? InvitedOn { get; set; }
        public bool Accepted { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? AcceptedOn { get; set; }

        public MeetingInvite()
        {
            
        }

        public MeetingInviteRef AsMeetingInviteRef()
        {
            return new MeetingInviteRef(this);
        }

        public void Accept()
        {
            var meeting = Action.AsAction();
            if (CrmUser != null)
            {
                var crmUser = CrmUser.AsCrmUser();
                meeting.CrmUsers.AddReferenceObject(crmUser.AsCrmUserRef());
                meeting.SaveToDatabase();
                crmUser.Actions.AddReferenceObject(meeting.AsActionRef());
                crmUser.SaveToDatabase();
                Accepted = true;
                AcceptedOn = DateTime.Now;
            }
        }
    }
}
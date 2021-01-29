using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class ContactMeetingInvite : BaseDocument
    {
        public ActionRef MeetingAction { get; set; }
        public ContactRef Contact { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? InvitedOn { get; set; }
        public bool Accepted { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? AcceptedOn { get; set; }

        public void Accept()
        {
            var meeting = MeetingAction.AsAction();
            meeting.Contacts.AddReferenceObject(Contact);
            meeting.SaveToDatabase();
            Accepted = true;
            AcceptedOn = DateTime.Now;
            SaveToDatabase();
        }
    }
}
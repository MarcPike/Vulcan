using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class ContactMeetingInviteRef : ReferenceObject<ContactMeetingInvite>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public ContactMeetingInviteRef()
        {
            
        }

        public ContactMeetingInviteRef(ContactMeetingInvite document) : base(document)
        {
            this.GetPropertiesFromObject(document.Contact);
        }

        public ContactMeetingInvite AsContactMeetingInvite()
        {
            return this.ToBaseDocument();
        }
    }
}
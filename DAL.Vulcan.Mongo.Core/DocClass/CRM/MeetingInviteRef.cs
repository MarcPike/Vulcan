using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class MeetingInviteRef: ReferenceObject<MeetingInvite>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public MeetingInviteRef()
        {
            
        }

        public MeetingInviteRef(MeetingInvite document) : base(document)
        {
            var user = document.CrmUser.AsCrmUser();
            GetPropertiesFromObject(user);
            if (document.CrmUser != null)
            {
                var crmUser = document.CrmUser.AsCrmUser();
                this.GetPropertiesFromObject(crmUser.User);
            }
            else if (document.Contact != null)
            {
                var contact = document.Contact.AsContact();
                this.GetPropertiesFromObject(contact.Person);
            }
            else if (document.Employee != null)
            {
                var employee = document.Employee.AsUser();
                this.GetPropertiesFromObject(employee.Person);
            }
        }

        public MeetingInvite AsMeetingInvite()
        {
            return this.ToBaseDocument();
        }
    }
}
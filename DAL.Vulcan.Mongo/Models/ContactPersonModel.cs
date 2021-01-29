using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Models
{
    public class ContactPersonModel : PersonModelBase
    {
        public string Application { get; set; }
        public string UserId { get; set; }

        public string ContactId { get; set; }

        public ContactPersonModel() : base(new Person())
        {
            var contact = new Contact();
            ContactId = contact.Id.ToString();

        }

        public ContactPersonModel(Contact contact) : base(contact.Person)
        {
            UserId = contact.CreatedByUserId;
            ContactId = contact.Id.ToString();
        }
    }
}
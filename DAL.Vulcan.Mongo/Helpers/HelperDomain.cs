using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperDomain : HelperBase, IHelperDomain
    {
        public PhoneNumberModel GetNewPhoneNumber()
        {
            return new PhoneNumberModel();
        }

        public AddressModel GetNewAddressModel()
        {
            var model = new AddressModel();
            return model;
        }

        public MeetingInvite GetNewMeetingInvite()
        {
            return new MeetingInvite();
        }

        public ContactMeetingInvite GetNewContactMeetingInvite()
        {
            return new ContactMeetingInvite();
        }

        // Get new objects
        public EmailAddressModel GetNewEmailAddress()
        {
            return new EmailAddressModel(new EmailAddress());
        }

        // PreCheck Types
        public UserPersonModel GetPersonModelForUser(LdapUser user)
        {
            return new UserPersonModel(user);
        }

    }
}
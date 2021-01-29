using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Vulcan.Mongo.Core.Helpers
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
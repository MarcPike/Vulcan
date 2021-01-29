using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperDomain
    {
        AddressModel GetNewAddressModel();
        ContactMeetingInvite GetNewContactMeetingInvite();
        EmailAddressModel GetNewEmailAddress();
        MeetingInvite GetNewMeetingInvite();
        PhoneNumberModel GetNewPhoneNumber();
    }
}
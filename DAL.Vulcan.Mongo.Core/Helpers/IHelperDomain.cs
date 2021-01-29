using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Vulcan.Mongo.Core.Helpers
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
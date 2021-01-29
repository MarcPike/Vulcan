using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperPerson
    {
        List<string> GetAddressTypes();
        List<string> GetEmailTypes();
        List<string> GetPhoneTypes();
        AddressType VerifyAddressType(string addressType);
        EmailType VerifyEmailType(string emailType);
        PhoneType VerifyPhoneType(string phoneType);
        void ValidateModel(PersonModelBase model);
    }
}
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Vulcan.Mongo.Core.Helpers
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
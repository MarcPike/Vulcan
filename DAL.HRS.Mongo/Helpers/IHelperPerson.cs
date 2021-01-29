using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Ldap;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
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
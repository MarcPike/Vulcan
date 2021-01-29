using System.Collections.Generic;
using BI.DAL.Mongo.Models;
using DAL.Common.DocClass;
using DAL.Common.Models;
using DAL.HRS.Mongo.DocClass.Ldap;

namespace BI.DAL.Mongo.Helpers
{
    public interface IHelperPerson
    {
        List<string> GetAddressTypes();
        List<string> GetEmailTypes();
        List<string> GetPhoneTypes();
        AddressType VerifyAddressType(string addressType);
        EmailType VerifyEmailType(string emailType);
        PhoneType VerifyPhoneType(string phoneType);
        void ValidateModel(BiUserModel model);
    }
}
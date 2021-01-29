using System;
using System.Collections.Generic;
using System.Linq;
using BI.DAL.Mongo.Models;
using DAL.Common.DocClass;
using DAL.Common.Models;
using DAL.HRS.Mongo.DocClass.Ldap;
using EmailType = DAL.Common.DocClass.EmailType;

namespace BI.DAL.Mongo.Helpers
{
    public class HelperPerson : HelperBase, IHelperPerson
    {
        public EmailType VerifyEmailType(string emailType)
        {
            if (GetEmailTypes().All(x => x != emailType))
            {
                throw new Exception($"{emailType} is not a valid EmailType");
            }
            var result = (EmailType) Enum.Parse(typeof(EmailType), emailType, true);
            return result;
        }

        public PhoneType VerifyPhoneType(string phoneType)
        {
            if (GetPhoneTypes().All(x => x != phoneType))
            {
                throw new Exception($"{phoneType} is not a valid PhoneType");
            }
            var result = (PhoneType) Enum.Parse(typeof(PhoneType), phoneType, true);
            return result;
        }

        public AddressType VerifyAddressType(string addressType)
        {
            if (GetAddressTypes().All(x => x != addressType))
            {
                throw new Exception($"{addressType} is not a valid AddressType");
            }
            var result = (AddressType) Enum.Parse(typeof(AddressType), addressType, true);
            return result;
        }

        public List<string> GetEmailTypes()
        {
            return Enum.GetNames(typeof(EmailType)).ToList();
        }

        public List<string> GetPhoneTypes()
        {
            return Enum.GetNames(typeof(PhoneType)).ToList();
        }


        public List<string> GetAddressTypes()
        {
            return Enum.GetNames(typeof(AddressType)).ToList();
        }

        public void ValidateModel(BiUserModel model)
        {
            foreach (var address in model.Person.Addresses)
            {
                VerifyAddressType(address.Type.ToString());
            }

            foreach (var phoneNumber in model.Person.PhoneNumbers)
            {
                VerifyPhoneType(phoneNumber.Type.ToString());
            }

            foreach (var email in model.Person.EmailAddresses)
            {
                VerifyEmailType(email.Type.ToString());
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
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

        public void ValidateModel(PersonModelBase model)
        {
            foreach (var address in model.Addresses)
            {
                VerifyAddressType(address.Type);
            }

            foreach (var phoneNumber in model.PhoneNumbers)
            {
                VerifyPhoneType(phoneNumber.Type);
            }

            foreach (var email in model.EmailAddresses)
            {
                VerifyEmailType(email.Type);
            }
        }
    }
}
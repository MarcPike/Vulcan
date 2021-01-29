using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EmployeeEmailAddress: ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef EmailType { get; set; }
        public string EmailAddress { get; set; }

        public EmployeeEmailAddress()
        {
        }

        public EmployeeEmailAddress(string emailType, string emailAddress)
        {
            EmailType = PropertyBuilder
                .CreatePropertyValue("EmailType", "Type of email address", emailType, "Type of email address")
                .AsPropertyValueRef();
            EmailAddress = emailAddress;
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = EmailType.ChangeOfficeName(locationId, newName, modified);
            return modified;
        }
    }
}
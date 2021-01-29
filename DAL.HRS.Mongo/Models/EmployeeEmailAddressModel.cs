using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeEmailAddressModel: BaseModel, IHavePropertyValues
    {
        public string Id { get; set; }
        public PropertyValueRef EmailType { get; set; }
        public string EmailAddress { get; set; }

        public EmployeeEmailAddressModel()
        {
        }

        public EmployeeEmailAddressModel(EmployeeEmailAddress add)
        {
            Id = add.Id.ToString();
            EmailType = add.EmailType;
            EmailAddress = add.EmailAddress;
        }

        public static List<EmployeeEmailAddressModel> ConvertListForEmployee(Employee emp)
        {
            var result = new List<EmployeeEmailAddressModel>();
            foreach (var emailAddress in emp.EmailAddresses)
            {
                result.Add(new EmployeeEmailAddressModel(emailAddress));
            }

            return result;
        }

        public override string ToString()
        {
            return $"Id: {Id} Type: {EmailType?.Code} Email Address: {EmailAddress}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, EmailType);
        }
    }
}
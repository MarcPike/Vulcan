using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeePhoneNumberModel : BaseModel, IHavePropertyValues
    {
        public string Id { get; set; } 
        public PropertyValueRef PhoneType { get; set; }
        public string PhoneNumber { get; set; }
        public PropertyValueRef Country { get; set; }

        public EmployeePhoneNumberModel()
        {
        }

        public EmployeePhoneNumberModel(EmployeePhoneNumber ph)
        {
            Id = ph.Id.ToString();
            PhoneType = ph.PhoneType;
            PhoneNumber = ph.PhoneNumber;
            Country = ph.Country;
        }

        public static List<EmployeePhoneNumberModel> ConvertListForEmployee(Employee emp)
        {
            var result = new List<EmployeePhoneNumberModel>();
            foreach (var phoneNumber in emp.PhoneNumbers)
            {
                result.Add(new EmployeePhoneNumberModel(phoneNumber));
            }
            return result;
        }

        public override string ToString()
        {
            return $"Id: {Id} Type: {PhoneType?.Code} Number: {PhoneNumber} Country: {Country}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRefs(entity, new List<PropertyValueRef>()
            {
                PhoneType,
                Country
            });

        }
    }
}

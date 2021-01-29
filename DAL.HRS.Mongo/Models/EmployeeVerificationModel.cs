using System;
using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeVerificationModel : BaseModel, IHavePropertyValues
    {
        private static readonly Encryption _encryption = Encryption.NewEncryption;

        public EmployeeVerificationModel()
        {
        }

        public EmployeeVerificationModel(EmployeeVerification ver)
        {
            Id = ver.Id.ToString();
            DocumentNumber = ver.DocumentNumber;
            DocumentType = ver.DocumentType;
            DocumentExpiration = ver.DocumentExpiration;


            AdditionalGovernmentId1 = _encryption.Decrypt<string>(ver.AdditionalGovernmentId1);
            AdditionalGovernmentId2 = _encryption.Decrypt<string>(ver.AdditionalGovernmentId2);
            Dismissed = ver.Dismissed;
        }

        public string Id { get; set; }
        public string DocumentNumber { get; set; }
        public PropertyValueRef DocumentType { get; set; }
        public DateTime? DocumentExpiration { get; set; }
        public string AdditionalGovernmentIdString { get; set; }
        public int AdditionalGovernmentIdNumber { get; set; }

        public string AdditionalGovernmentId1 { get; set; } = string.Empty;
        public string AdditionalGovernmentId2 { get; set; } = string.Empty;

        public bool Dismissed { get; set; }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, DocumentType);
        }

        public override string ToString()
        {
            var nullString = "null";
            return
                $"Id: {Id} Doc#: {DocumentNumber} Type: {DocumentType?.Code ?? nullString} Expiration: {DocumentExpiration?.ToShortDateString() ?? nullString} GovId#1: {AdditionalGovernmentId1} GovId#2: {AdditionalGovernmentId2}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public static List<EmployeeVerificationModel> ConvertBaseListToModelList(List<EmployeeVerification> list)
        {
            var result = new List<EmployeeVerificationModel>();
            foreach (var employeeVerification in list) result.Add(new EmployeeVerificationModel(employeeVerification));

            return result;
        }

        public static List<EmployeeVerification> ConvertModelListToBaseList(List<EmployeeVerificationModel> list)
        {
            var result = new List<EmployeeVerification>();
            foreach (var employeeVerificationModel in list)
                result.Add(new EmployeeVerification
                {
                    Id = Guid.Parse(employeeVerificationModel.Id),
                    DocumentNumber = employeeVerificationModel.DocumentNumber,
                    DocumentType = employeeVerificationModel.DocumentType,
                    DocumentExpiration = employeeVerificationModel.DocumentExpiration,
                    AdditionalGovernmentId1 = _encryption.Encrypt(employeeVerificationModel.AdditionalGovernmentId1),
                    AdditionalGovernmentId2 = _encryption.Encrypt(employeeVerificationModel.AdditionalGovernmentId2),
                    Dismissed = employeeVerificationModel.Dismissed
                });

            return result;
        }
    }
}
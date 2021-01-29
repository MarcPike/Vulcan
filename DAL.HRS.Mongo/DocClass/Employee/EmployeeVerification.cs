using System;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.Encryption;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    [BsonIgnoreExtraElements]
    public class EmployeeVerification
    {
        private static readonly Encryption _encryption = Encryption.NewEncryption;

        public Guid Id { get; set; } = Guid.NewGuid();
        public string DocumentNumber { get; set; }
        public PropertyValueRef DocumentType { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DocumentExpiration { get; set; }

        public byte[] AdditionalGovernmentIdString { get; set; } = _encryption.Encrypt("<Empty>");
        public byte[] AdditionalGovernmentIdNumber { get; set; } = _encryption.Encrypt(0);

        public byte[] AdditionalGovernmentId1 { get; set; } = _encryption.Encrypt(string.Empty);
        public byte[] AdditionalGovernmentId2 { get; set; } = _encryption.Encrypt(string.Empty);

        public bool Dismissed { get; set; } = false;


        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? RequiredActivityDueDate { get; set; }

        public RequiredActivityRef RequiredActivity { get; set; }

        public override string ToString()
        {
            return
                $"Id: {Id} Doc#: {DocumentNumber} Type: {DocumentType?.Code} Expires: {DocumentExpiration} Dismissed: {Dismissed}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }
    }
}
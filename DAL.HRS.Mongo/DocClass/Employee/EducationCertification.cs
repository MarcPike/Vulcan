using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EducationCertification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DegreeCert { get; set; }
        public string Institution { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? IssueDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CertExpiration { get; set; }

        public bool Dismissed { get; set; } = false;
        public string File { get; set; }

        public override string ToString()
        {
            return
                $"Id:{Id} DegreeCert: {DegreeCert} Institution: {Institution} IssueDate: {IssueDate} Expires: {CertExpiration} Dismissed: {Dismissed}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }
    }
}
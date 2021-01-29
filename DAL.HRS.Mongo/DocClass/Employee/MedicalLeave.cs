using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EmployeeMedicalLeave
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? FromDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ToDate { get; set; }

        public PropertyValueRef MedicalLeaveType { get; set; } =
            PropertyBuilder.New("MedicalLeaveType", "Type of medical leave", "Unspecified", "");
        public string Notes { get; set; }

        public List<SupportingDocumentRef> SupportingDocuments { get; set; } = new List<SupportingDocumentRef>();

    }
}

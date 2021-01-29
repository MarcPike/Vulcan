using System;
using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    [BsonIgnoreExtraElements]
    public class EmployeeMedicalLeaveHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool EligibleMedicalLeave { get; set; }
        public byte[] FromDate { get; set; }
        public byte[] ToDate { get; set; }
        public PropertyValueRef MedicalLeaveReason { get; set; }
        public PropertyValueRef MedicalLeaveType { get; set; }
        public byte[] Notes { get; set; }
        public List<SupportingDocument> SupportingDocuments { get; set; } = new List<SupportingDocument>();
    }
}
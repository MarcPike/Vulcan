using System;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class IncidentLeave
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public byte[] FromDate { get; set; }
        public byte[] ToDate { get; set; }
        public PropertyValueRef LeaveType { get; set; }
        public bool EligibleMedicalLeave { get; set; }
        public byte[] Notes { get; set; }
        public PropertyValueRef ReasonCode { get; set; }
        public PropertyValueRef MedicalLeaveType { get; set; }
    }
}
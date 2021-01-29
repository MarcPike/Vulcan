using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeMedicalLeaveHistoryModel
    {
        public string Id { get; set; }
        public bool EligibleMedicalLeave { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public PropertyValueRef MedicalLeaveReason { get; set; }
        public PropertyValueRef MedicalLeaveType { get; set; }
        public string Notes { get; set; }

        public int DaysAway
        {
            get
            {
                if ((FromDate != null) && (ToDate != null))
                {
                    var diff = (ToDate - FromDate);
                    return (int)diff.Value.TotalDays;
                }
                else if ((FromDate != null) && (ToDate == null))
                {
                    var now = DateTime.Now;
                    var diff = (now - FromDate);
                    return (int)diff.Value.TotalDays;
                }

                return 0;
            }
        }

        public EmployeeMedicalLeaveHistoryModel()
        {
            
        }

        public EmployeeMedicalLeaveHistoryModel(EmployeeMedicalLeaveHistory leave)
        {
            var enc = Encryption.NewEncryption;
            Id = leave.Id.ToString();
                
            EligibleMedicalLeave = leave.EligibleMedicalLeave;
            FromDate = enc.Decrypt<DateTime?>(leave.FromDate);
            ToDate = enc.Decrypt<DateTime?>(leave.ToDate);
            Notes = enc.Decrypt<string>(leave.Notes);
            MedicalLeaveReason = leave.MedicalLeaveReason;
            MedicalLeaveType = leave.MedicalLeaveType;
        }
    }

}

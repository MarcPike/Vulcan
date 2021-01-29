using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.Models
{



    public class EmployeeMedicalLeaveModel
    {
        public string Id { get; set; } 
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int DaysAway
        {
            get
            {
                if ((FromDate != null) && (ToDate != null))
                {
                    var diff = (ToDate - FromDate);
                    return (int)diff.Value.TotalDays;
                }

                return 0;
            }
        }

        public PropertyValueRef MedicalLeaveType { get; set; } =
            PropertyBuilder.New("MedicalLeaveType", "Type of medical leave", "Unspecified", "");
        public string Notes { get; set; }

        public List<SupportingDocumentRef> SupportingDocuments { get; set; } = new List<SupportingDocumentRef>();

        public EmployeeMedicalLeaveModel()
        {
            
        }

        public EmployeeMedicalLeaveModel(EmployeeMedicalLeave l)
        {
            Id = l.Id.ToString();
            FromDate = l.FromDate;
            ToDate = l.ToDate;
            MedicalLeaveType = l.MedicalLeaveType;
            Notes = l.Notes;
            SupportingDocuments = l.SupportingDocuments;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class QngTrainingInfoModel
    {
        public string PayrollId { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public LocationRef Location { get; set; }
        public bool IsActive { get; set; }
        public string CourseType { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string VenueType { get; set; }
        public decimal TrainingHours { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CertificateExpiration { get; set; }
        public decimal Cost { get; set; }
        public decimal Reimbursement { get; set; }
        public string ExternalInstructor { get; set; }
        public EmployeeRef InternalInstructor { get; set; }

        public QngTrainingInfoModel()
        {

        }

    }
}

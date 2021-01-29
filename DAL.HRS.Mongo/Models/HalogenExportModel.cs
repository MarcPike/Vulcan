using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DocumentFormat.OpenXml.Office2013.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Models
{
    public class HalogenExportModel
    {
        public string SubjectUserName { get; set; }
        public string ManagerId { get; set; }
        public string HrRepUsername { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; }
        public DateTime? HireDate { get; set; }
        public string JobTitle { get; set; }
        public string PerfEvalType { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Location { get; set; }
        public string LMS { get; set; }
        public bool IsInactive { get; set; }

        public HalogenExportModel()
        {

        }

        public HalogenExportModel(
            string subjectUserName, EmployeeRef manager,
            string firstName, string lastName, List<EmployeeEmailAddress> emailAddresses, string employeeId, DateTime? hireDate, 
            JobTitleRef jobTitle, PropertyValueRef performanceEvaluationType, PropertyValueRef costCenterCode, PropertyValueRef kronosDepartmentCode, 
            LocationRef location, PropertyValueRef lms, DateTime? terminationDate, List<HrRepresentative> hrRepCache
            )
        {

            var hrRep = hrRepCache.FirstOrDefault(x => x.Location.Id == location.Id);


            SubjectUserName = subjectUserName;
            ManagerId = manager?.PayrollId;
            HrRepUsername = hrRep?.Representative.Login;
            FirstName = firstName;
            LastName = lastName;
            EmployeeId = employeeId;
            HireDate = hireDate;
            JobTitle = jobTitle.Name;
            PerfEvalType = performanceEvaluationType?.Code ?? "";
            Department = costCenterCode?.Code ?? "";
            Division = kronosDepartmentCode?.Code ?? "";
            Location = location.Office;
            LMS = lms?.Description ?? "";
            IsInactive = terminationDate != null;
            
            if (emailAddresses.Any())
            {
                var workAddress = emailAddresses.FirstOrDefault(x => x.EmailType.Code == "Work");

                if (workAddress != null)
                {
                    Email = workAddress.EmailAddress;
                }
                else
                {
                        Email = "";
                }
            }

        }


    }
}

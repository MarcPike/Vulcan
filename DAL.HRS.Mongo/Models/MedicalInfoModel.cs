using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.Models
{
    public class MedicalInfoModel
    {
        public EmployeeRef Employee { get; set; }
        public List<EmployeeDrugTestModel> DrugTests { get; set; } = new List<EmployeeDrugTestModel>();
        //public List<EmployeeMedicalLeaveModel> MedicalLeaves { get; set; } = new List<EmployeeMedicalLeaveModel>();
        public List<EmployeeMedicalExamModel> MedicalExams { get; set; } = new List<EmployeeMedicalExamModel>();
        public List<EmployeeOtherMedicalInfoModel> OtherMedicalInfo { get; set; } = new List<EmployeeOtherMedicalInfoModel>();
        public List<EmployeeMedicalLeaveHistoryModel> LeaveHistory { get; set; } = new List<EmployeeMedicalLeaveHistoryModel>();
        public PropertyValueRef GenderCode { get; set; }
        public int Age { get; set; }
        public DateTime? HireDate { get; set; }
        public JobTitleRef JobTitle { get; set; }
        public EmployeeRef Manager { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }

        public HrsUserRef HrsUser { get; set; }

        public MedicalInfoModel()
        {
        }

        public MedicalInfoModel(EmployeeRef employee, HrsUserRef hrsUser)
        {
            Employee = employee;



            var emp = employee.AsEmployee();
            DrugTests = emp.MedicalInfo.DrugTests.Select(x=>  new EmployeeDrugTestModel(x)).ToList();
            //MedicalLeaves = emp.MedicalInfo.MedicalLeaves.Select(x=> new EmployeeMedicalLeaveModel(x)).ToList();
            MedicalExams = emp.MedicalInfo.MedicalExams.Select(x=> new EmployeeMedicalExamModel(x)).ToList();
            OtherMedicalInfo = emp.MedicalInfo.OtherMedicalInfo.Select(x=> new EmployeeOtherMedicalInfoModel(x)).ToList();
            LeaveHistory = emp.MedicalInfo.LeaveHistory.Select(x=> new EmployeeMedicalLeaveHistoryModel(x)).ToList();
            
            GenderCode = emp.GenderCode;
            Age = emp.GetAge();
            HireDate = emp.OriginalHireDate;
            JobTitle = emp.JobTitle;
            Manager = emp.Manager;
            CostCenterCode = emp.CostCenterCode;

            HrsUser = hrsUser;
        }
    }
}

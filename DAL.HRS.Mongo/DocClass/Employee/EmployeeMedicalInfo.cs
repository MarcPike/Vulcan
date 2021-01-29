using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using MongoDB.Bson.Serialization.Attributes;
using DrugTest = DAL.HRS.Mongo.DocClass.Hse.DrugTest;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    [BsonIgnoreExtraElements]
    public class EmployeeMedicalInfo
    {
        public List<EmployeeDrugTest> DrugTests { get; set; } = new List<EmployeeDrugTest>();
        //public List<EmployeeMedicalLeave> MedicalLeaves { get; set; } = new List<EmployeeMedicalLeave>();
        public List<EmployeeMedicalExam> MedicalExams { get; set; } = new List<EmployeeMedicalExam>();
        public List<EmployeeOtherMedicalInfo> OtherMedicalInfo { get; set; } = new List<EmployeeOtherMedicalInfo>();
        public List<EmployeeMedicalLeaveHistory> LeaveHistory { get; set; } = new List<EmployeeMedicalLeaveHistory>();
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Dashboard;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeMedicalExamModel
    {
        public string Id { get; set; }
        public PropertyValueRef MedicalExamType { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ExamDate { get; set; }
        public DateTime? Completed { get; set; }

        public bool IsCompleted { get; set; }
    

        public string Facility { get; set; }
        public string Doctor { get; set; }
        public RequiredActivityRef RequiredActivity { get; set; }
        public int RepeatMonths { get; set; } = 0;
        public string Comments { get; set; }

        public EmployeeMedicalExamModel()
        {
            
        }

        public EmployeeMedicalExamModel(DocClass.MedicalExams.EmployeeMedicalExam e)
        {
            Id = e.Id.ToString();
            MedicalExamType = e.MedicalExamType;
            DueDate = e.DueDate;
            ExamDate = e.ExamDate;
            Completed = e.Completed;
            IsCompleted = e.IsCompleted;
            Facility = e.Facility;
            Doctor = e.Doctor;
            RequiredActivity = e.RequiredActivity;
            RepeatMonths = e.RepeatMonths;
            Comments = e.Comments;
        }
    }
}

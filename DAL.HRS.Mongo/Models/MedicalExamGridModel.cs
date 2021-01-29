using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace DAL.HRS.Mongo.Models
{
    public class MedicalExamGridModel
    {
        public string Id { get; set; }
        public EmployeeRef Employee { get; set; }
        public PropertyValueRef MedicalExamType { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? Completed { get; set; }
        public string Facility { get; set; }
        public string Doctor { get; set; }
        public RequiredActivityRef RequiredActivity { get; set; }

        public int RepeatMonths { get; set; } = 0;

        public MedicalExamGridModel() { }

        public MedicalExamGridModel(DocClass.MedicalExams.EmployeeMedicalExam exam)
        {
            Id = exam.Id.ToString();
            Employee = exam.Employee;
            MedicalExamType = exam.MedicalExamType;
            DueDate = exam.DueDate;
            Completed = exam.Completed;
            Facility = exam.Facility;
            Doctor = exam.Doctor;
            RequiredActivity = exam.RequiredActivity;
            RepeatMonths = exam.RepeatMonths;

        }

    }
}

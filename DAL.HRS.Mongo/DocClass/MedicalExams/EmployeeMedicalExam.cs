using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Dashboard;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.MedicalExams
{
    [BsonIgnoreExtraElements]
    public class EmployeeMedicalExam 
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public EmployeeRef Employee { get; set; }
        public PropertyValueRef MedicalExamType { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DueDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Completed { get; set; }

        public bool IsCompleted { get; set; } 
        public string Facility { get; set; }
        public string Doctor { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ExamDate { get; set; }
        public string Comments { get; set; }
        public RequiredActivityRef RequiredActivity { get; set; }

        public int RepeatMonths { get; set; } = 0;

        //public MedicalExamRef AsMedicalExamRef()
        //{
        //    return new MedicalExamRef(this);
        //}
    }

    //public class MedicalExamRef : ReferenceObject<EmployeeMedicalExam>
    //{
    //    public EmployeeRef Employee { get; set; }
    //    public PropertyValueRef MedicalExamType { get; set; }
    //    public DateTime? DueDate { get; set; }
    //    public DateTime? ExamDate { get; set; }

    //    public MedicalExamRef()
    //    {
    //    }

    //    public MedicalExamRef(EmployeeMedicalExam m)
    //    {
    //        Employee = m.Employee;
    //        MedicalExamType = m.MedicalExamType;
    //        DueDate = m.DueDate;
    //        ExamDate = m.ExamDate;
    //    }

}



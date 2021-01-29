using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Dashboard;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.HRS.Mongo.Models
{
    //public class DashboardItemModel
    //{
    //    public string Id { get; set; }
    //    [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
    //    [BsonRepresentation(BsonType.String)]         // Mongo        public DashboardItemType Type { get; set; }
    //    public DashboardItemType Type { get; set; }
    //    public EmployeeRef Employee { get; set; }
    //    public DateTime DueDate { get; set; }
    //    public bool Dismissed { get; set; } = false;
    //    public List<SupportingDocumentRef> SupportingDocuments { get; set; } = new List<SupportingDocumentRef>();
    //    public TrainingEventRef TrainingEvent { get; set; }
    //    public MedicalExamRef EmployeeMedicalExam { get; set; }
    //    public string Notes { get; set; }

    //    public DashboardItemModel() { }

    //    public DashboardItemModel(DashboardItem t)
    //    {
    //        Id = t.Id.ToString();
    //        Type = t.Type;
    //        Employee = t.Employee;
    //        DueDate = t.DueDate;
    //        Dismissed = t.Dismissed;
    //        SupportingDocuments = t.SupportingDocuments;
    //        TrainingEvent = t.TrainingEvent;
    //        Notes = t.Notes;
    //        EmployeeMedicalExam = t.EmployeeMedicalExam;
    //    }

    //}
}

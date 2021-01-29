using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeOtherMedicalInfoModel
    {
        public string Id { get; set; } 
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Date { get; set; }
        public string Comments { get; set; }

        public EmployeeOtherMedicalInfoModel()
        {
            
        }

        public EmployeeOtherMedicalInfoModel(EmployeeOtherMedicalInfo o)
        {
            Id = o.Id.ToString();
            Date = o.Date;
            Comments = o.Comments;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EmployeeDrugTest 
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public PropertyValueRef DrugTestType { get; set; } =
            PropertyBuilder.New("DrugTestType", "Type of drug test", "Unspecified", "");

        public PropertyValueRef DrugTestResult { get; set; } =
            PropertyBuilder.New("DrugTestResult", "Result of drug test", "Unspecified", "");
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? TestDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ResultDate { get; set; }
        public string Comments { get; set; }
    }
}

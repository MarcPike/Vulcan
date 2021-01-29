using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Models
{
    public class RegulatoryIncidentGridModel
    {
        public ObjectId Id { get; set; }
        public int IncidentId { get; set; }
        public DateTime? IncidentDate { get; set; }
        public PropertyValueRef RegulatoryType { get; set; }
        public PropertyValueRef IncidentType { get; set; }
        public EmployeeRef Complainant { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef IncidentStatus { get; set; }
    }
}

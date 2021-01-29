using System;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.DocClass.AuditTrails
{
    [BsonIgnoreExtraElements]
    public class EmployeeAuditTrail : BaseDocument
    {
        public static MongoRawQueryHelper<EmployeeAuditTrail> Helper = new MongoRawQueryHelper<EmployeeAuditTrail>();

        public EmployeeAuditTrail()
        {
        }

        public EmployeeAuditTrail(Employee.Employee original, HrsUserRef updatedBy)
        {
            UpdatedAt = DateTime.Now;
            UpdatedBy = updatedBy;
            Original = (Employee.Employee) original.DeepClone();
        }

        public DateTime UpdatedAt { get; set; }
        public HrsUserRef UpdatedBy { get; set; }
        public Employee.Employee Original { get; set; }
        public Employee.Employee Current { get; set; }

        public EmployeeAuditTrailModel AuditTrail { get; set; }

        public void Save(Employee.Employee current)
        {
            Current = current;
            Helper.Upsert(this);
        }

        public static void CalculateRequired()
        {
            var audits =
                Helper.Find(x => x.AuditTrail == null).ToList();

            foreach (var employeeAuditTrail in audits)
            {
                employeeAuditTrail.AuditTrail = new EmployeeAuditTrailModel(employeeAuditTrail);
                Helper.Upsert(employeeAuditTrail);
            }
        }
    }
}
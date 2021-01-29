using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeIncidentGridModel
    {
        public string Id { get; set; }
        public string IncidentId { get; set; }
        public PropertyValueRef NearMissTypeCode { get; set; }
        public PropertyValueRef IncidentType { get; set; }
        public EmployeeRef Employee { get; set; }
        public DateTime? IncidentDate { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef IncidentStatus { get; set; }
        public bool SubmittedFromWeb { get; set; }
        public bool Osha { get; set; }
        public bool Riddor { get; set; }
        public bool OneDayNotificationSent { get; set; }
        public bool TenDayNotificationSent { get; set; }

        public EmployeeIncidentGridModel() { }

        public EmployeeIncidentGridModel(ObjectId id, int incidentId, PropertyValueRef nearMissTypeCode, PropertyValueRef incidentType, EmployeeRef employee, DateTime? incidentDate,
            LocationRef location, PropertyValueRef incidentStatus, bool submittedFromWeb, bool osha, bool riddor, bool oneDayNotificationSent, bool tenDayNotificationSent)
        {
            Id = id.ToString();

            var incidentIdString = nearMissTypeCode?.Code ?? "?";
            incidentIdString += incidentId.ToString();

            IncidentId = incidentIdString;

            NearMissTypeCode = nearMissTypeCode;
            IncidentType = incidentType;
            Employee = employee;
            IncidentDate = incidentDate;
            Location = location;
            IncidentStatus = incidentStatus;
            SubmittedFromWeb = submittedFromWeb;
            Osha = osha;
            Riddor = riddor;
            OneDayNotificationSent = oneDayNotificationSent;
            TenDayNotificationSent = tenDayNotificationSent;

        }
    }
}
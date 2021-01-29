using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using DAL.Common.DocClass;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class HseObservation : BaseDocument
    {
        public EmployeeRef Employee { get; set; }
        public EmployeeRef Manager { get; set; }
        public decimal Cost { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateOf { get; set; }
        public string Description { get; set; }
        public HseObservationNotifications Notifications { get; set; } = new HseObservationNotifications();
        public List<HseObservationSuggestion> Suggestions { get; set; } = new List<HseObservationSuggestion>();
        public PropertyValueRef ObservationClass { get; set; } 
        public PropertyValueRef ObservationType { get; set; }
        public PropertyValueRef Status { get; set; }
        public List<HseObservationAction> ObservationActions { get; set; } = new List<HseObservationAction>();
        public LocationRef Location { get; set; }
        public string PhysicalLocation { get; set; }
        public int OldHrsId { get; set; }
    }
}

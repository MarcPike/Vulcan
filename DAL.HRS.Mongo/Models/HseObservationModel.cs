using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hse;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public class HseObservationModel
    {
        public string Id { get; set; }
        public EmployeeRef Employee { get; set; }
        public EmployeeRef Manager { get; set; }
        public decimal Cost { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateOf { get; set; }
        public string Description { get; set; }
        public HseObservationNotifications Notifications { get; set; } = new HseObservationNotifications();
        public List<HseObservationSuggestionModel> Suggestions { get; set; } = new List<HseObservationSuggestionModel>();
        public PropertyValueRef ObservationClass { get; set; }
        public PropertyValueRef ObservationType { get; set; }
        public PropertyValueRef Status { get; set; }
        public List<HseObservationActionModel> ObservationActions { get; set; } = new List<HseObservationActionModel>();
        public LocationRef Location { get; set; }
        public string PhysicalLocation { get; set; }

        public HseObservationModel()
        {
        }

        public HseObservationModel(HseObservation o)
        {
            Id = o.Id.ToString();
            Employee = o.Employee;
            Manager = o.Manager;
            Cost = o.Cost;
            DateOf = o.DateOf;
            Description = o.Description;
            Notifications = o.Notifications;
            Suggestions = o.Suggestions.Select(x=> new HseObservationSuggestionModel(x)).ToList();
            ObservationClass = o.ObservationClass;
            ObservationType = o.ObservationType;
            Status = o.Status;
            ObservationActions = o.ObservationActions.Select(x => new HseObservationActionModel(x)).ToList();
            Location = o.Location;
            PhysicalLocation = o.PhysicalLocation;
        }

    }
}

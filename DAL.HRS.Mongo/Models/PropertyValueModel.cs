using System;
using DAL.Common.DocClass;
using System.Collections.Generic;
using System.Security;
using System.Text.RegularExpressions;
using PropertyValue = DAL.HRS.Mongo.DocClass.Properties.PropertyValue;

namespace DAL.HRS.Mongo.Models
{
    public class PropertyValueModel
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; } 
        public EntityRef Entity { get; set; } 
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();

        public bool IsDirty { get; set; } = false;

        public PropertyValueModel()
        {
        }

        public PropertyValueModel(PropertyValue propertyValue)
        {
            Id = propertyValue.Id.ToString();
            Code = propertyValue.Code;
            Description = propertyValue.Description;
            Entity = propertyValue.Entity;
            Locations = propertyValue.Locations;
            Type = propertyValue.Type;
            Active = propertyValue.Active;
        }
    }
}
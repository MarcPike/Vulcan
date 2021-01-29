using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.DocClass.Properties
{
    [BsonIgnoreExtraElements]
    public class PropertyValueRef : ReferenceObject<PropertyValue>, ISupportLocationNameChangesNested
    {
        public string Type { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();
        public EntityRef Entity { get; set; }

        public PropertyValueRef(PropertyValue val) : base(val)
        {
            if (val == null) return;

            Type = val.Type;
            Code = val.Code;
            Description = val.Description;
            Locations = val.Locations;
            Entity = val.Entity;
        }

        public PropertyValue AsPropertyValue()
        {
            return ToBaseDocument();
        }

        public PropertyValueRef Refresh()
        {
            var current = ToBaseDocument();
            if (current == null)
            {
                Code = $"!{Code}";
                Description = $"!{Description}";
                return this;
            }

            if (Code != current.Code) Code = current.Code;
            if (Description != current.Description) Description = current.Description;
            return this;
        }

        public void Load(EntityRef entity)
        {
            if (entity == null) return;
            if (Entity.Id == entity.Id)
            {
                Entity = entity; // just to make sure
                return;
            }

            var code = Code;
            var type = Type;

            var correctPropertyValue = PropertyValue.Helper.Find(x => x.Code == code && x.Type == type && x.Entity.Id == entity.Id)
                .FirstOrDefault();

            if (correctPropertyValue == null)
            {
                return;
            }

            var changeTo = correctPropertyValue.AsPropertyValueRef();
            Id = changeTo.Id;
            Code = changeTo.Code;
            Description = changeTo.Description;
            Entity = changeTo.Entity;
        }

        public override string ToString()
        {
            return $"{Type}:{Code}:{Description}";
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            if (Locations == null) return modified;

            var row = Locations.FirstOrDefault(x => x.Id == locationId);
            if (row == null) return modified;

            row.Office = newName;
            return true;
        }
    }
}
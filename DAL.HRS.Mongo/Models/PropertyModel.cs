using DAL.Vulcan.Mongo.Base.DocClass;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.Models
{
    public class PropertyModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<PropertyValueModel> PropertyValues { get; set; } = new List<PropertyValueModel>();
        public bool IsDirty { get; set; } = false;
        public EntityRef Entity { get; set; }

        public PropertyModel()
        {
        }

        public PropertyModel(PropertyType prop, string entityId)
        {
            Id = prop.Id.ToString();
            Type = prop.Type;
            Description = prop.Description;
            //PropertyValues = prop.PropertyValues.Select(x=> new PropertyValueModel(x)).OrderBy(x=>x.Code).ThenBy(x=>x.Description).ToList();

            Entity = new MongoRawQueryHelper<Entity>().FindById(entityId).AsEntityRef();

            //if (prop.Type == "CostCenter")
            //{
            //    PropertyValues = prop.PropertyValues.Where(x => x.Entity.Id == entityId && x.Active == true).
            //        OrderBy(x=>x.CodeAsInteger).Select(x => new PropertyValueModel(x)).ToList();
            //    PropertyValues.AddRange(prop.PropertyValues.Where(x => x.Entity.Id == entityId && x.Active == false).
            //        OrderBy(x => x.CodeAsInteger).Select(x => new PropertyValueModel(x)).ToList());

            //}
            //else
            //{
                PropertyValues = prop.PropertyValues.Where(x => x.Entity.Id == entityId && x.Active == true).
                    Select(x => new PropertyValueModel(x)).OrderBy(x => x.Code).ToList();
                PropertyValues.AddRange(prop.PropertyValues.Where(x => x.Entity.Id == entityId && x.Active == false).
                    Select(x => new PropertyValueModel(x)).OrderBy(x => x.Code).ToList());

            //}


        }
    }
}

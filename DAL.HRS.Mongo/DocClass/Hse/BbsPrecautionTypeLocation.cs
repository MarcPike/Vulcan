using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsPrecautionTypeLocation : BaseDocument
    {
        public static MongoRawQueryHelper<BbsPrecautionTypeLocation> Helper = new MongoRawQueryHelper<BbsPrecautionTypeLocation>();

        public LocationRef Location { get; set; }

        public List<PropertyValueRef> PrecautionTypeProperties { get; set; } = new List<PropertyValueRef>();

        public static PropertyValueRef GetPropertyValueRefForLocation(string name, LocationRef location, string entityName)
        {
            var entity = Entity.GetRefByName(entityName);
            var prec = Helper.Find(x => x.Location.Id == location.Id).FirstOrDefault();
            if (prec == null)
            {
                prec = new BbsPrecautionTypeLocation();

                var property = PropertyValue.Helper.Find(x => x.Code == name && x.Entity.Name == entityName)
                    .FirstOrDefault();
                if (property == null)
                {
                    property = new PropertyValue()
                    {
                        Code = name,
                        Entity = entity,
                        Type = "BbsPrecautionType",
                        Description = "Bbs Precaution Type"
                    };
                    PropertyValue.Helper.Upsert(property);
                }

                var propertyValueRef = property.AsPropertyValueRef();
                prec.PrecautionTypeProperties.Add(propertyValueRef);
                Helper.Upsert(prec);
                return propertyValueRef;

            }

            var existingProp = prec.PrecautionTypeProperties.SingleOrDefault(x => x.Code == name && x.Entity == entity);
            if (existingProp == null)
            {
                var newProp = new PropertyValue()
                {
                    Code = name,
                    Entity = entity,
                    Type = "BbsPrecautionType",
                    Description = "Bbs Precaution Type"
                };

                PropertyValue.Helper.Upsert(newProp);
                prec.PrecautionTypeProperties.Add(newProp.AsPropertyValueRef());
                Helper.Upsert(prec);
                existingProp = newProp.AsPropertyValueRef();
            }

            return existingProp;
        }
    }
}

using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using System.Linq;

namespace DAL.HRS.Mongo.DocClass.Properties
{
    public static class PropertyBuilder
    {

        public static PropertyValue CreatePropertyValue(string type, string typeDescription,  string code, string description, EntityRef entity = null)
        {
            var repPropertyType = new RepositoryBase<PropertyType>();
            var repPropertyValue = new RepositoryBase<PropertyValue>();

            var propertyType = repPropertyType.AsQueryable().SingleOrDefault(x => x.Type == type);
            if (propertyType == null)
            {
                propertyType = new PropertyType()
                {
                    Type = type,
                    Description = typeDescription
                };
                repPropertyType.Upsert(propertyType);
            }

            code = code ?? "<Null Value>";
            description = description ?? "<Null Value>";

            entity = entity ?? Entity.GetRefByName("Howco");

            var propertyValue = repPropertyValue.AsQueryable().SingleOrDefault(x => x.Type == type && x.Code == code && x.Entity.Id == entity.Id);
            if (propertyValue == null)
            {
                propertyValue = new PropertyValue()
                {
                    Type = propertyType.Type,
                    Code = code ?? "<Null Value>",
                    Description = description ?? "<Null Value>",
                    Entity = entity
                };
                repPropertyValue.Upsert(propertyValue);
            }

            return propertyValue;
        }

        public static PropertyValueRef New(string type, string description, string code,
            string codeDescription, EntityRef entity = null)
        {
            if (entity == null)
            {
                entity = Entity.GetRefByName("Howco");
            }

            var newPropertyValue = CreatePropertyValue(type,description, code, codeDescription, entity );
            CreatePropertyValue(type, description, code, codeDescription, Entity.GetRefByName("Edgen Murray"));

            return newPropertyValue.AsPropertyValueRef();
        }


    }
}
